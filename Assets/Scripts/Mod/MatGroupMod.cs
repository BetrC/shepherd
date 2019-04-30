using NumSharp;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 使用矩阵运算的羊群控制代码
/// </summary>
public class MatGroupMod : UnitySingleton<MatGroupMod>
{
    #region sheeps' position info
    public NDArray sheeps;
    public List<GameObject> objs;
    #endregion

    #region Instances
    private Generator generator;
    #endregion

    #region matrixs -- Temp res 
    private NDArray matS;
    /// <summary>
    /// 没有羊狼排斥力的羊只坐标
    /// </summary>
    private List<int> indexs;
    #endregion

    #region 状态参量
    public float time = 0;
    #endregion
    private void Start()
    {

        indexs = new List<int>();
        InitInstance();
        InitSheeps();
        InitObjs();

        //Test.NumPyTest();
    }


    private void FixedUpdate()
    {
        RefreshSheeps();
    }

    /// <summary>
    /// 羊群坐标初始化
    /// </summary>
    private void InitSheeps()
    {
        int[] size = { Config.N, 2 };
        sheeps = new NumPyRandom().normal(0.0, 20, size);
    }

    private void InitObjs()
    {
        objs = new List<GameObject>();
        // Initial sheeps' object
        GameObject prefab = Resources.Load<GameObject>("Prefabs/sheep") as GameObject;
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            GameObject go = Instantiate(prefab, Global.NDArrayToVec3(sheeps[i]), Quaternion.identity);
            go.name = "" + i;
            objs.Add(go);
        }
    }

    /// <summary>
    /// 单例对象的实例化
    /// </summary>
    private void InitInstance()
    {
        generator = Generator.Instance;
    }

    public NDArray GetH()
    {
        NDArray matH = np.zeros(sheeps.shape);
        matS = GetS();
        matH = Config.Ro_s * matS + Config.e * GetNoise() + Config.c * GetC() + Config.Ro_a * GetA();
        Global.Normalize(matH);
        return matH;
    }

    /// <summary>
    /// 获取羊狼排斥力
    /// </summary>
    /// <returns></returns>
    private NDArray GetS()
    {
        if (generator.shepherd == null)
        {
            return np.zeros(sheeps.shape);
        }
        var temp = generator.shepherd.GetComponent<MatShepherd>().pos;
        var matShepherd = Global.Duplicate(temp, sheeps.shape[0]);
        var sub = sheeps - matShepherd;
        var matS = Adjust(sub, Config.R_s);
        return matS;
    }

    private NDArray GetA()
    {
        var matA = np.zeros(sheeps.shape);
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            NDArray temp = new NDArray(sheeps[i].Array as double[], new Shape(1, 2));
            var matTemp = Global.Duplicate(temp, sheeps.shape[0]);
            var sub = matTemp - sheeps;
            var arr = AdjustWithCal(sub, Config.R_a);
            (matA[i, 0], matA[i, 1]) = (arr[0], arr[1]);
        }
        Global.Normalize(matA);
        return matA;
    }

    private NDArray GetC()
    {
        NDArray GCM = Global.Mean(sheeps);
        NDArray matGCM = Global.Duplicate(GCM, sheeps.shape[0]);
        NDArray sub = matGCM - sheeps;
        Global.Normalize(sub);

        // 当有羊狼排斥力的时候才会有中心力
        foreach (var i in indexs)
        {
            (sub[i, 0], sub[i, 1]) = (0, 0);
        }
        return sub;
    }

    private NDArray GetNoise()
    {
        NDArray matNoise = np.zeros(sheeps.shape);
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            float p = Random.Range(0.0f, 1.0f);
            if (p < Config.p)
            {
                Vector2 noise = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                noise.Normalize();
                (matNoise[i, 0], matNoise[i, 1]) = (noise.x, noise.y);
            }
        }
        return matNoise;
    }

    public NDArray GetGCM()
    {
        return Global.Mean(sheeps);
    }

    private void RefreshSheeps()
    {
        // 性能统计
        var watch = Stopwatch.StartNew();
        NDArray matH = GetH();
        watch.Stop();
        var elapsed = watch.Elapsed;
        UnityEngine.Debug.Log(elapsed);
        sheeps = sheeps + matH;
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            objs[i].GetComponent<Sheep>().MatAction(sheeps[i]);
        }
    }

    public bool IsTargetOK()
    {
        NDArray temp = GetGCM();
        Vector3 GCM = Global.NDArrayToVec3(temp);
        // 5是一个暂定参数
        if ((GCM - Config.targetPos).magnitude <= 5)
        {
            return true;
        }
        return false;
    }

    public bool WithinGCM()
    {
        NDArray GCM = GetGCM();
        float fn = Config.R_a * Mathf.Sqrt(Config.N);
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            if (Global.NDArrayToVec3(GCM[0] - sheeps[i]).magnitude > fn)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 计算最远的羊只的坐标
    /// </summary>
    /// <returns></returns>
    public NDArray GetFurthestSheep()
    {
        NDArray furthest = np.zeros(new Shape(1, 2));
        float max = 0;
        NDArray GCM = GetGCM();
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            float d = Global.NDArrayToVec3(GCM[0] - sheeps[i]).magnitude;
            if (d > max)
            {
                (furthest[0, 0], furthest[0, 1]) = (sheeps[i, 0], sheeps[i, 1]);
                max = d;
            }
        }
        return furthest;
    }

    /// <summary>
    /// 计算最近的羊只的距离
    /// </summary>
    /// <returns></returns>
    public float GetNearestSheep()
    {
        float min = float.MaxValue;
        NDArray shepherd = generator.shepherd.GetComponent<MatShepherd>().pos;
        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            float d = Global.NDArrayToVec3(shepherd[0] - sheeps[i]).magnitude;
            if (d < min)
            {
                min = d;
            }
        }
        return min;
    }


    public NDArray Adjust(NDArray nd, float dis)
    {
        var res = np.zeros(nd.shape);
        indexs.Clear();
        for (int i = 0; i < nd.shape[0]; i++)
        {
            Vector3 vec = Global.NDArrayToVec3(nd[i]);
            if (vec.magnitude < dis)
            {
                vec.Normalize();
                double[] arr = Global.Vec3ToArray(vec);
                (res[i, 0], res[i, 1]) = (arr[0], arr[1]);
            }
            else
            {
                indexs.Add(i);
            }
        }
        return res;
    }

    /// <summary>
    /// 根据指定距离调整矩阵并计算均值
    /// </summary>
    /// <param name="nd"></param>
    /// <param name="dis"></param>
    /// <returns></returns>
    public double[] AdjustWithCal(NDArray nd, float dis)
    {
        double[] puffer = new double[2] { 0, 0 };
        int count = 0;
        for (int i = 0; i < nd.shape[0]; i++)
        {
            Vector3 vec = Global.NDArrayToVec3(nd[i]);
            if (vec.magnitude < dis)
            {
                vec.Normalize();
                double[] arr = Global.Vec3ToArray(vec);
                puffer[0] += arr[0];
                puffer[1] += arr[1];
                count++;
            }
        }
        for (int i = 0; i < puffer.Length; i++)
        {
            puffer[i] /= count;
        }
        return puffer;
    }

    public void Reset()
    {
        int[] size = { Config.N, 2 };
        sheeps = new NumPyRandom().normal(0.0, 20, size);

        for (int i = 0; i < sheeps.shape[0]; i++)
        {
            objs[i].transform.position = Global.NDArrayToVec3(sheeps[i]);
        }
    }
}
