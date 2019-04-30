using UnityEngine;

/// <summary>
/// 管理所有UI的基类
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    private void Start()
    {
        Init();
        PreOnShow();
        OnShow();
    }

    public abstract void Init();

    public abstract void PreOnShow();

    public abstract void OnShow();
}
