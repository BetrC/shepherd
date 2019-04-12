using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 羊群类，对羊群做各种计算
/// </summary>
class GroupMod : UnitySingleton<GroupMod>
{

    public List<Vector2> Sheeps;
    public List<GameObject> Objs;

    private List<Vector2> Inertia;
    private List<Vector2> MatS;
    private List<Vector2> MatA;
    private List<Vector2> MatC;
    private List<Vector2> MatNoise;


    private void Start()
    {
        Debug.Log("开始生成羊群");
        if (Sheeps == null)
        {
            Sheeps = new List<Vector2>();
        }
        if (Objs == null)
        {
            Objs = new List<GameObject>();
        }
        if (Inertia == null)
        {
            Inertia = Enumerable.Repeat(Vector2.zero, Config.N).ToList();
        }
        Init();
    }

    private void Update()
    {
        var matH = GetH();
        for (int i = 0; i < Sheeps.Count; i++)
        {
            Sheeps[i] += Config.sheepSpeed * matH[i];
            Inertia[i] = matH[i];
            Objs[i].transform.position = new Vector3(Sheeps[i].x, Sheeps[i].y, 0);
        }
    }
     
    private void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/sheep") as GameObject;
        for (int i = 0; i < Config.N; i++)
        {
            // 生成羊只
            float x = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPoint.x;
            float y = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPoint.y;
            GameObject go = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            go.name = "" + i;
            Sheeps.Add(new Vector2(x, y));
            Objs.Add(go);
        }
    }

    public List<Vector2> GetH()
    {
        List<Vector2> matH = Enumerable.Repeat(Vector2.zero, Sheeps.Count).ToList();

        MatS = GetS();
        MatA = GetA();
        MatC = GetC();
        MatNoise = GetNoise();

        for (int i = 0; i < Sheeps.Count; i++)
        {
            matH[i] = Config.h * Inertia[i] + Config.Ro_s * MatS[i] + Config.e * MatNoise[i] + Config.c * MatC[i] + Config.Ro_a * MatA[i];
            matH[i] = matH[i].normalized;
            //Debug.Log("第" + i + "个： S : " + MatS[i].ToString() + "\n Noise : " + MatNoise[i].ToString() +
            //    "\n C : " + MatC[i].ToString() + "\n Inertia : " + Inertia[i].ToString() + "\n A : " + MatA[i].ToString());
        }
        return matH;
    }

    private List<Vector2> GetS()
    {
        var matS = Enumerable.Repeat(Vector2.zero, Sheeps.Count).ToList();
        Vector3 vec = Manager.Instance.shepherd.transform.position;
        Vector2 pos = new Vector2(vec.x, vec.y);
        for (int i = 0; i < Sheeps.Count; i++)
        {
            Vector2 temp = Sheeps[i] - pos;
            matS[i] = temp.magnitude < Config.R_s ? temp : Vector2.zero;
            matS[i] = matS[i].normalized;
        }
        return matS;
    }

    private List<Vector2> GetA()
    {
        // 羊群排斥力计算方式为计算与当前羊只邻近的羊群对其排斥力的合力
        var matA = Enumerable.Repeat(Vector2.zero, Sheeps.Count).ToList();
        for (int i = 0; i < Sheeps.Count; i++)
        {
            //var query = from s in Sheeps
            //            where (Sheeps[i] - s).magnitude < Config.R_a
            //            select Sheeps[i] - s;
            //Debug.Log(query.ToList<Vector2>().ToString());

            var temp = new List<Vector2>();
            foreach (var s in Sheeps)
            {
                Vector2 vec = Sheeps[i] - s;
                if (vec.magnitude < Config.R_a)
                {
                    temp.Add(vec);
                }
            }
            matA[i] = Mean(temp);
            matA[i] = matA[i].normalized;
        }
        return matA;
    }

    private List<Vector2> GetC()
    {
        // 羊群中心吸引力只有在羊只在牧羊犬巡视范围的时候才会有
        var matC = Enumerable.Repeat(Vector2.zero, Sheeps.Count).ToList();
        Vector2 GCM = GetGCM();
        for (int i = 0; i < Sheeps.Count; i++)
        {
            if (Vector2.zero != MatS[i])
            {
                matC[i] = GCM - Sheeps[i];
                matC[i] = matC[i].normalized;
            }
        }
        return matC;
    }

    private List<Vector2> GetNoise()
    {
        // 噪声以一定概率出现
        var matNoise = Enumerable.Repeat(Vector2.zero, Sheeps.Count).ToList();
        for (int i = 0; i < Sheeps.Count; i++)
        {
            float p = Random.Range(0.0f, 1.0f);
            if (p <= Config.p)
            {
                Vector2 noise = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                noise.Normalize();
                matNoise[i] = noise;
            }
        }
        return matNoise;
    }

    public Vector2 GetGCM()
    {
        return Mean(Sheeps);
    }

    private Vector2 Mean(List<Vector2> vec)
    {
        Vector2 res = Vector2.zero;
        foreach (var v in vec)
        {
            res += v;
        }
        res = vec.Count == 0 ? Vector2.zero : res / vec.Count;
        return res;
    }


    /// <summary>
    /// 刷新点坐标集
    /// </summary>
    private void RefreshAgents()
    {
        Sheeps.Clear();
        // 获取点坐标集
        foreach (var obj in Objs)
        {
            Sheeps.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
        }
    }

    /// <summary>
    /// 任务完成检测
    /// </summary>
    /// <returns></returns>
    public bool IsTargetOk()
    {
        Vector2 GCM = GetGCM();
        // TODO 重复获取参数 未变变量应设为全局
        Vector3 point = Manager.Instance.targetPoint;
        Vector2 target = new Vector2(point.x, point.y);

        // 5是一个暂定参数
        if ((GCM - target).magnitude <= 5)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 所有羊只是否与全局中心点在一定距离内
    /// </summary>
    /// <returns></returns>
    public bool WithinGCM()
    {
        Vector2 GCM = GetGCM();
        float fn = Config.R_a * Mathf.Sqrt(Config.N);
        foreach (var a in Sheeps)
        {
            if ((a - GCM).magnitude > fn)
            {
                return false;
            }
        }
        return true;
    }

    public Vector2 GetFurthestSheep()
    {
        Vector2 furthest = Vector2.zero;
        float max = 0;
        Vector2 GCM = GetGCM();
        foreach (var agent in Sheeps)
        {
            float d = (agent - GCM).magnitude;
            if (d > max)
            {
                furthest = agent;
                max = d;
            }
        }
        return furthest;
    }

    public float GetNearestSheep()
    {
        // Vector2 nearest = Vector2.zero;
        float min = float.MaxValue;
        Vector2 shepherd = Manager.Instance.shepherd.transform.position;
        foreach (var agent in Sheeps)
        {
            float d = (agent - shepherd).magnitude;
            if (d < min)
            {
                // nearest = agent;
                min = d;
            }
        }
        return min;
    }
}

