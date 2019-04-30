using NumSharp;
using UnityEngine;

public class MatShepherd : MonoBehaviour
{

    #region mouse control

    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;

    #endregion

    private NDArray Inertia = np.zeros(new Shape(1, 2));

    #region Instances

    private MatGroupMod groupMod;

    private Generator generator;

    #endregion

    public NDArray pos;

    // Use this for initialization
    void Start()
    {
        groupMod = MatGroupMod.Instance;
        generator = Generator.Instance;

        double[] arr = Global.Vec3ToArray(transform.position);
        pos = new NDArray(arr, new Shape(1, 2));
    }

    // Update is called once per frame
    void Update()
    {
        // KeyController();

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

    /// <summary>
    /// 驱赶位置
    /// </summary>
    /// <returns></returns>
    private NDArray DrivingPosition()
    {
        NDArray GCM = groupMod.GetGCM();
        NDArray target = new NDArray(Global.Vec3ToArray(Config.targetPos), new Shape(1, 2));
        NDArray vec = GCM - target;
        Global.Normalize(vec);
        return GCM + vec * (2 * Config.R_a * Mathf.Sqrt(Config.N));
    }


    /// <summary>
    /// 收集位置
    /// </summary>
    /// <returns></returns>
    private NDArray CollectPosition()
    {
        NDArray furthest = groupMod.GetFurthestSheep();
        NDArray GCM = groupMod.GetGCM();
        NDArray vec = furthest - GCM;
        Global.Normalize(vec);
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

    private NDArray GetNoise()
    {
        NDArray matNoise = np.zeros(new Shape(1, 2));

        float p = Random.Range(0.0f, 1.0f);
        if (p < Config.p)
        {
            Vector2 noise = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            noise.Normalize();
            (matNoise[0, 0], matNoise[0, 1]) = (noise.x, noise.y);
        }
        return matNoise;
    }


    private void AutoMove()
    {
        NDArray H = np.zeros(new Shape(1, 2));

        if (GetAction() == Action.Collecting)
        {
            NDArray Pc = CollectPosition();
            H = Pc - pos;
        }
        else
        {
            NDArray Pd = DrivingPosition();
            H = Pd - pos;
        }
        Global.Normalize(H);
        H += Inertia * Config.h + Config.e * GetNoise();
        Global.Normalize(H);
        Inertia = H;
        pos += GetSpeed() * H;
        transform.position = Global.NDArrayToVec3(pos);
    }
}