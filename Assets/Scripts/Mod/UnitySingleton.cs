using UnityEngine;

/// <summary>
/// 单例模式基类，若要实现单例继承此类即可
/// </summary>
/// <typeparam name="T">要实现单例的类</typeparam>
public class UnitySingleton<T> : MonoBehaviour
        where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    _instance = (T)obj.AddComponent(typeof(T));
                }
            }
            return _instance;
        }
    }
    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
