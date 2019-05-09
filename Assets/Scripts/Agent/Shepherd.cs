using UnityEngine;

/// <summary>
/// 牧羊犬相关类
/// </summary>
public class Shepherd : MonoBehaviour
{

    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;

    private Vector2 Inertia = Vector2.zero;

    #region Instances
    private GroupMod groupMod;
    #endregion

    // Use this for initialization
    void Start()
    {
        groupMod = GroupMod.Instance;

    }

    private void FixedUpdate()
    {
        AutoMove();
    }

    /// <summary>
    /// 鼠标驱动
    /// </summary>
    private void KeyController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            lastMousePosition = Vector3.zero;
        }
        if (isMouseDown)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private Vector2 DrivingPosition()
    {
        Vector2 GCM = groupMod.GetGCM();
        Vector2 target = new Vector3(Config.targetPos.x, Config.targetPos.y);
        Vector2 vec = GCM - target;
        vec.Normalize();

        return GCM + vec * (2 * Config.R_a * Mathf.Sqrt(Config.N));
    }


    private Vector2 CollectPosition()
    {
        Vector2 furthest = groupMod.GetFurthestSheep();
        Vector2 GCM = groupMod.GetGCM();
        Vector2 vec = furthest - GCM;
        vec.Normalize();

        return furthest + vec * Config.R_a;
    }

    /// <summary>
    /// 获取牧羊犬的速度
    /// </summary>
    /// <returns></returns>
    private float GetSpeed()
    {
        float nearestDis = groupMod.GetNearestSheep();
        if (nearestDis < Config.R_a * 3)
        {
            return Config.shephardSpeed / 2;
        }
        return Config.shephardSpeed;
    }


    private Action GetAction()
    {
        if (groupMod.WithinGCM())
        {
            return Action.Driving;
        }
        return Action.Collecting;
    }

    /// <summary>
    /// 获取噪声
    /// </summary>
    /// <returns></returns>
    public Vector2 GetNoise()
    {
        float p = Random.Range(0.0f, 1.0f);
        if (p <= Config.p)
        {
            Vector2 noise = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            noise.Normalize();
            return noise;
        }
        return Vector2.zero;
    }


    private void AutoMove()
    {
        Vector2 H = Vector2.zero;

        if (GetAction() == Action.Collecting)
        {
            Vector2 Pc = CollectPosition();
            H = Pc - new Vector2(transform.position.x, transform.position.y);
        }
        else
        {
            Vector2 Pd = DrivingPosition();
            H = Pd - new Vector2(transform.position.x, transform.position.y);
        }
        H.Normalize();

        Vector2 repulsion = transform.position - Config.targetPos;
        repulsion.Normalize();

        H += Inertia * Config.h + repulsion * 0.5f;

        if (Config.useNoise)
        {
            H += Config.e * GetNoise();
        }

        H.Normalize();
        Inertia = H;

        Vector3 newPos = transform.position + GetSpeed() * new Vector3(H.x, H.y, 0);
        transform.position = newPos;
    }
}
