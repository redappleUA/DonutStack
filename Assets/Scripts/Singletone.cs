using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Singletone<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new()
                {
                    name = typeof(T).Name,
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}

public class SingletonePersistent<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get 
        {
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if(_instance == this)
            _instance = null;
    }

    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
}
