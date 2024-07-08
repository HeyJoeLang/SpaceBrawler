using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace LIV.SDK.Unity
{
    [CustomEditor(typeof(LIV))]
    public class LIVEditor : Editor
    {        
        const string EXCLUDE_BEHAVIOURS_FOLDOUT_KEY = "liv_excludeBehavioursfoldout";

        const string STAGE_PROPERTY = "_stage";
        const string STAGE_TRANSFORM_PROPERTY = "_stageTransform";
        const string HMD_CAMERA_PROPERTY = "_HMDCamera";
        const string MR_CAMERA_PREFAB_PROPERTY = "_MRCameraPrefab";
        const string DISABLE_STANDARD_ASSETS_PROPERTY = "_disableStandardAssets";
        const string SPECTATOR_LAYER_MASK_PROPERTY = "_spectatorLayerMask";
        const string PASSTHROUGH_LAYER_MASK_PROPERTY = "_passthroughLayerMask";
        const string EXCLUDE_BEHAVIOURS_PROPERTY = "_excludeBehaviours";
        const string FIX_POST_EFFECTS_ALPHA_PROPERTY = "_fixPostEffectsAlpha";
        const string TRACKING_ID_PROPERTY = "_trackingID";

        static GUIContent REQUIRED_FOLDOUT_GUICONTENT = new GUIContent("Required");
        static GUIContent OPTIONAL_FOLDOUT_GUICONTENT = new GUIContent("Optional");
        
        static GUIContent STAGE_GUICONTENT = new GUIContent("Stage");
        static GUIContent STAGE_INFO_GUICONTENT = new GUIContent(
            "The origin of tracked space.\n" +
            "This is the object that you use to move the player around."
            );

        static GUIContent STAGE_TRANSFORM_GUICONTENT = new GUIContent("Stage Transform");
        static GUIContent STAGE_TRANSFORM_INFO_GUICONTENT = new GUIContent(
            "This transform defines the stage transform."
            );

        static GUIContent HMD_CAMERA_GUICONTENT = new GUIContent("HMD Camera");
        static GUIContent HMD_CAMERA_INFO_GUICONTENT = new GUIContent(
            "Set this to the camera used to render the HMD's point of view."
            );

        static GUIContent MR_CAMERA_PREFAB_GUICONTENT = new GUIContent("MR Camera Prefab");
        static GUIContent MR_CAMERA_PREFAB_INFO_GUICONTENT = new GUIContent(
            "Set custom camera prefab."
            );

        static GUIContent DISABLE_STANDARD_ASSETS_GUICONTENT = new GUIContent("Disable Standard Assets");
        static GUIContent DISABLE_STANDARD_INFO_ASSETS_GUICONTENT = new GUIContent(
            "If you're using Unity's standard effects and they're interfering with MR rendering, check this box."
            );

        static GUIContent SPECTATOR_LAYER_MASK_GUICONTENT = new GUIContent("Spectator Layer Mask");
        static GUIContent SPECTATOR_LAYER_MASK_INFO_GUICONTENT = new GUIContent(
            "By default, we'll show everything on the spectator camera. If you want to disable certain objects from showing, update this mask property."
            );
        
        static GUIContent PASSTHROUGH_LAYER_MASK_GUICONTENT = new GUIContent("Passthrough Layer Mask");
        static GUIContent PASSTHROUGH_LAYER_MASK_INFO_GUICONTENT = new GUIContent(
            "Render only certain layers when passthrough rendering is turned on."
        );

        static GUIContent FIX_POST_EFFECTS_ALPHA_GUICONTENT = new GUIContent("Fix Post-Effects alpha channel");
        static GUIContent FIX_POST_EFFECTS_ALPHA_INFO_GUICONTENT = new GUIContent(
            "Some post-effects corrupt the alpha channel, this fix tries to recover it."
            );


        static GUIContent EXCLUDE_BEHAVIOURS_GUICONTENT = new GUIContent("Exclude Behaviours");

        public static GUIStyle VERSION_STYLE {
            get {
                GUIStyle g = new GUIStyle(EditorStyles.label);
                g.alignment = TextAnchor.LowerLeft;
                g.normal.textColor = Color.white;
                g.fontStyle = FontStyle.Bold;
                return g;
            }
        }

        public static GUIStyle LIV_BUTTON_STYLE {
            get {
                GUIStyle g = new GUIStyle();                
                return g;
            }
        }

        public static Color darkBGColor {
            get {
                if (EditorGUIUtility.isProSkin)
                {
                    return new Color(0f, 0f, 0f, 0.5f);
                } else
                {
                    return new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }
        public static Color lightRedBGColor {
            get {
                return new Color(1f, 0.5f, 0.5f, 1f);
            }
        }
        public static Color lightBGColor {
            get {
                return new Color(1f, 1, 1, 1f);
            }
        }

        ReorderableList excludeBehavioursList;

        static bool excludeBehavioursFoldoutValue {
            get {
                if (!EditorPrefs.HasKey(EXCLUDE_BEHAVIOURS_FOLDOUT_KEY)) return false;
                return EditorPrefs.GetBool(EXCLUDE_BEHAVIOURS_FOLDOUT_KEY);
            }
            set {
                EditorPrefs.SetBool(EXCLUDE_BEHAVIOURS_FOLDOUT_KEY, value);
            }
        }

        SerializedProperty _stageProperty = null;
        SerializedProperty _stageTransformProperty = null;
        SerializedProperty _hmdCameraProperty = null;
        SerializedProperty _mrCameraPrefabProperty = null;
        SerializedProperty _disableStandardAssetsProperty = null;
        SerializedProperty _spectatorLayerMaskProperty = null;
        SerializedProperty _passthroughLayerMaskProperty = null;
        SerializedProperty _excludeBehavioursProperty = null;
        SerializedProperty _fixPostEffectsAlphaProperty = null;

        static Texture2D _livLogo;
        void OnEnable()
        {
            string[] livLogoGUID = AssetDatabase.FindAssets("LIVLogo t:texture2D");            
            if (livLogoGUID.Length > 0)
            {                
                _livLogo = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(livLogoGUID[0]));
            }

            _stageProperty = serializedObject.FindProperty(STAGE_PROPERTY);
            _stageTransformProperty = serializedObject.FindProperty(STAGE_TRANSFORM_PROPERTY);
            _hmdCameraProperty = serializedObject.FindProperty(HMD_CAMERA_PROPERTY);
            _mrCameraPrefabProperty = serializedObject.FindProperty(MR_CAMERA_PREFAB_PROPERTY);
            _disableStandardAssetsProperty = serializedObject.FindProperty(DISABLE_STANDARD_ASSETS_PROPERTY);            
            _spectatorLayerMaskProperty = serializedObject.FindProperty(SPECTATOR_LAYER_MASK_PROPERTY);
            _passthroughLayerMaskProperty = serializedObject.FindProperty(PASSTHROUGH_LAYER_MASK_PROPERTY);
            _excludeBehavioursProperty = serializedObject.FindProperty(EXCLUDE_BEHAVIOURS_PROPERTY);
            _fixPostEffectsAlphaProperty = serializedObject.FindProperty(FIX_POST_EFFECTS_ALPHA_PROPERTY);

            excludeBehavioursList = new ReorderableList(serializedObject, _excludeBehavioursProperty, true, true, true, true);
            excludeBehavioursList.drawElementCallback = DrawListItems;
            excludeBehavioursList.headerHeight = 2;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RenderLIVLOGO(_livLogo);
            EditorGUILayout.LabelField(REQUIRED_FOLDOUT_GUICONTENT, EditorStyles.boldLabel);
            RenderTrackingID();
            RenderStageField();
            RenderHMDField();
            DrawProperty(_disableStandardAssetsProperty, DISABLE_STANDARD_ASSETS_GUICONTENT, DISABLE_STANDARD_INFO_ASSETS_GUICONTENT, lightBGColor);
            
            RenderSpectatorLayerMaskField();
            RenderPassthroughLayerMaskField();

            Color lastAccentColor = GUI.color;
            GUI.color = darkBGColor;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = lastAccentColor;

            EditorGUI.indentLevel++;
            excludeBehavioursFoldoutValue = EditorGUILayout.Foldout(excludeBehavioursFoldoutValue, EXCLUDE_BEHAVIOURS_GUICONTENT);
            EditorGUI.indentLevel--;
            if (excludeBehavioursFoldoutValue)
            {
                excludeBehavioursList.DoLayoutList();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.LabelField(OPTIONAL_FOLDOUT_GUICONTENT, EditorStyles.boldLabel);
            DrawProperty(_stageTransformProperty, STAGE_TRANSFORM_GUICONTENT, STAGE_TRANSFORM_INFO_GUICONTENT, lightBGColor);
            DrawProperty(_mrCameraPrefabProperty, MR_CAMERA_PREFAB_GUICONTENT, MR_CAMERA_PREFAB_INFO_GUICONTENT, lightBGColor);
            DrawProperty(_fixPostEffectsAlphaProperty, FIX_POST_EFFECTS_ALPHA_GUICONTENT, FIX_POST_EFFECTS_ALPHA_INFO_GUICONTENT, lightBGColor);
            serializedObject.ApplyModifiedProperties();
            
            GUIContent helpContent = new GUIContent(EditorGUIUtility.IconContent("_Help"));
            helpContent.text = "Help";
            if (GUILayout.Button(helpContent))
            {
                Application.OpenURL(@"https://liv.tv/sdk-unity-docs");
            }
            EditorGUILayout.Space();
        }
        
        void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            EditorGUI.PropertyField(rect, excludeBehavioursList.serializedProperty.GetArrayElementAtIndex(index), new GUIContent(""), false);
        }

        void DrawList(SerializedProperty property, GUIContent guiContent)
        {
            EditorGUILayout.PropertyField(property, guiContent);
            while (true)
            {                
                if(!property.Next(true))
                {
                    break;
                }
                EditorGUILayout.PropertyField(property);
            }
        }

        void DrawProperty(SerializedProperty property, GUIContent label, GUIContent content, Color color)
        {
            Color lastAccentColor = GUI.color;
            GUI.color = darkBGColor;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = lastAccentColor;
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(property, label);            
            Color lastBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            EditorGUILayout.LabelField(content, EditorStyles.helpBox);
            GUI.backgroundColor = lastBackgroundColor;
            EditorGUILayout.EndVertical();
        }
        
        public static void RenderLIVLOGO(Texture2D livLogo)
        {
            if (livLogo == null)
                return;
            
            Color lastBackgroundColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.5f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = lastBackgroundColor;
            Rect originalRect = EditorGUILayout.GetControlRect(GUILayout.Height(livLogo.height));                
            Rect imageRect = originalRect;
            imageRect.x = (originalRect.width - livLogo.width);
            imageRect.width = livLogo.width;
            GUI.DrawTexture(imageRect, livLogo);
            GUI.Label(originalRect, "LIV SDK v"+LivApi.GetVersion(), VERSION_STYLE);
            if(GUI.Button(originalRect, new GUIContent(), LIV_BUTTON_STYLE)) {
                Application.OpenURL("https://liv.tv/");
            }
            EditorGUILayout.EndVertical();
        }

        void RenderTrackingID()
        {
            Color color = lightBGColor;
            GUIContent content = new GUIContent(SDKSettingsEditor.TRACKING_ID_INFO_GUICONTENT);
            string trackingID = SDKSettings.instance.trackingID;
            var isTrackingIDMissing = string.IsNullOrEmpty(trackingID);
            
            Color lastAccentColor = GUI.color;
            GUI.color = darkBGColor;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = lastAccentColor;
            GUILayout.Space(2);
            
            if (isTrackingIDMissing)
            {
                color = lightRedBGColor;
                content.text += "\nThe tracking ID has to be set! Click the button bellow to get the tracking ID for your game.";
                content.image = EditorGUIUtility.IconContent("console.erroricon").image;
            }
            
            EditorGUILayout.LabelField("trackingID: "+trackingID);
            Color lastBackgroundColor = GUI.backgroundColor;
            if (GUILayout.Button("Settings"))
            {
                Selection.activeObject = SDKSettings.instance;
            }
            GUI.backgroundColor = color;
            EditorGUILayout.LabelField(content, EditorStyles.helpBox);
            GUI.backgroundColor = lastBackgroundColor;
            
            EditorGUILayout.EndVertical();
        }

        void RenderStageField()
        {
            Color color = lightBGColor;
            GUIContent content = new GUIContent(STAGE_INFO_GUICONTENT);
            if (_stageProperty.objectReferenceValue == null)
            {
                color = lightRedBGColor;
                content.text += "\nThe stage has to be set!";
                content.image = EditorGUIUtility.IconContent("console.erroricon").image;
            }

            DrawProperty(_stageProperty, STAGE_GUICONTENT, content, color);
        }

        void RenderHMDField()
        {
            Color color = lightBGColor;
            GUIContent content = new GUIContent(HMD_CAMERA_INFO_GUICONTENT);
            if (_hmdCameraProperty.objectReferenceValue == null)
            {
                color = lightRedBGColor;
                content.text += "\nThe camera has to be set!";
                content.image = EditorGUIUtility.IconContent("console.erroricon").image;
            }

            DrawProperty(_hmdCameraProperty, HMD_CAMERA_GUICONTENT, content, color);
        }

        void RenderSpectatorLayerMaskField()
        {
            Color color = lightBGColor;
            GUIContent content = new GUIContent(SPECTATOR_LAYER_MASK_INFO_GUICONTENT);            
            if (_spectatorLayerMaskProperty.intValue == 0)
            {
                color = lightRedBGColor;
                content.text += "\nAre you sure you want to render nothing?";
                content.image = EditorGUIUtility.IconContent("console.warnicon").image;
            } 

            DrawProperty(_spectatorLayerMaskProperty, SPECTATOR_LAYER_MASK_GUICONTENT, content, color);
        }
        
        void RenderPassthroughLayerMaskField()
        {
            Color color = lightBGColor;
            GUIContent content = new GUIContent(PASSTHROUGH_LAYER_MASK_INFO_GUICONTENT);            
            if (_passthroughLayerMaskProperty.intValue == 0)
            {
                color = lightRedBGColor;
                content.text += "\nAre you sure you want to render nothing?";
                content.image = EditorGUIUtility.IconContent("console.warnicon").image;
            } 

            DrawProperty(_passthroughLayerMaskProperty, PASSTHROUGH_LAYER_MASK_GUICONTENT, content, color);
        }
    }
}