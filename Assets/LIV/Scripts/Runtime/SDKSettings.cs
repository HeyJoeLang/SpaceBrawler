using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LIV.SDK.Unity
{
    public class SDKSettings : ScriptableObject
    {
        private const string FILE_NAME = "LIVSDKSettings";
        
#pragma warning disable 0649
        [SerializeField] private string _trackingID = "debug";
        [SerializeField] private SDKBridge.CaptureProtocolType _captureProtocolType = SDKBridge.CaptureProtocolType.BRIDGE;
#pragma warning restore 0649
        public string trackingID
        {
            get { return _trackingID; }
        }

        public SDKBridge.CaptureProtocolType captureProtocolType
        {
            get { return _captureProtocolType; }
        }

        private static SDKSettings _instance;

        public static SDKSettings instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<SDKSettings>(FILE_NAME);
                
#if UNITY_EDITOR
                if (_instance == null)
                {
                    SDKSettings scriptableObject = ScriptableObject.CreateInstance<SDKSettings>();
                    UnityEditor.MonoScript monoScript = UnityEditor.MonoScript.FromScriptableObject(scriptableObject);
                    string path = UnityEditor.AssetDatabase.GetAssetPath(monoScript);
                    path = System.IO.Path.GetDirectoryName(path).Replace("Scripts", "");
                    path = System.IO.Path.Combine(path, "Resources");
                    string filePath = System.IO.Path.Combine(path, FILE_NAME+".asset");
                    UnityEditor.AssetDatabase.CreateAsset(scriptableObject, filePath);
                    UnityEditor.AssetDatabase.SaveAssets();
                    _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<SDKSettings>(filePath);
                }
#endif
                if (_instance == null)
                    Debug.LogError("LIV: Unable to find LIV SDK Settings!");
                
                return _instance;
            }
        }
    }
}