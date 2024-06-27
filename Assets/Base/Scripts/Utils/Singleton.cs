using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private const string DestroyInstance = "Destroying extra Instance of {0}";

    private static T _instance = null;
    private static bool _isQuiting = false;

    public virtual string ObjectName { get { return typeof(T).Name; } }

    public static T Instance
    {
        get
        {
            if (_instance == null && !_isQuiting)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying && Application.isEditor)
                {
                    return null;
                }
#endif

                Debug.LogFormat("Singleton: Creating new {0} gameObject", typeof(T).Name);
                var go = new GameObject().AddComponent<T>();
                if (go != null)
                {
                    go.name = go.ObjectName;
                }
            }
            return _instance;
        }
    }

    public static bool InstanceIsNull
    { get { return _instance == null; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
        if (_instance != null && _instance != this)
        {
            Debug.LogWarningFormat(DestroyInstance, typeof(T).Name);
            Destroy(this.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        _isQuiting = true;
    }
}