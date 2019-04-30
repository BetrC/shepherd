using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 羊群类，对羊群做各种计算
/// </summary>
class GroupMod : UnitySingleton<GroupMod>
{

    public List<Vector2> sheeps;
    public List<GameObject> objs;

    private List<Vector2> matS;
    private List<Vector2> matA;
    private List<Vector2> matC;
    private List<Vector2> matNoise;


    public float time = 0;

    private void Start()
    {
        UnityEngine.Debug.Log("开始生成羊群");
        if (sheeps == null)
        {
            sheeps = new List<Vector2>();
        }
        if (objs == null)
        {
            objs = new List<GameObject>();
        }
        Init();
    }

    private void FixedUpdate()
    {
        // 性能统计
        //var watch = Stopwatch.StartNew();
        var matH = GetH();
        //watch.Stop();
        //var elapsed = watch.Elapsed;
        //UnityEngine.Debug.Log(elapsed);
        for (int i = 0; i < sheeps.Count; i++)
        {
            objs[i].GetComponent<Sheep>().Action(matH[i]);
            sheeps[i] = new Vector2(objs[i].transform.position.x, objs[i].transform.position.y);
        }
        time += Time.fixedDeltaTime;
    }

    private void Init()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/sheep") as GameObject;
        for (int i = 0; i < Config.N; i++)
        {
            // 生成羊只
            float x = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.x;
            float y = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.y;
            GameObject go = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            go.name = "" + i;
            sheeps.Add(new Vector2(x, y));
            objs.Add(go);
        }
    }

    public List<Vector2> GetH()
    {
        List<Vector2> matH = Enumerable.Repeat(Vector2.zero, sheeps.Count).ToList();

        matS = GetS();
        matA = GetA();
        matC = GetC();
        matNoise = GetNoise();

        for (int i = 0; i < sheeps.Count; i++)
        {
            matH[i] = Config.Ro_s * matS[i] + Config.e * matNoise[i] + Config.c * matC[i] + Config.Ro_a * matA[i];
            matH[i] = matH[i].normalized;
        }
        return matH;
    }

    private List<Vector2> GetS()
    {
        var matS = Enumerable.Repeat(Vector2.zero, sheeps.Count).ToList();
        if (Generator.Instance.shepherd == null)
        {
            return matS;
        }
        Vector3 vec = Generator.Instance.shepherd.transform.position;
        Vector2 pos = new Vector2(vec.x, vec.y);
        for (int i = 0; i < sheeps.Count; i++)
        {
            Vector2 temp = sheeps[i] - pos;
            matS[i] = temp.magnitude < Config.R_s ? temp : Vector2.zero;
            matS[i] = matS[i].normalized;
        }
        return matS;
    }

    private List<Vector2> GetA()
    {
        // 羊群排斥力计算方式为计算与当前羊只邻近的羊群对其排斥力的合力
        var matA = Enumerable.Repeat(Vector2.zero, sheeps.Count).ToList();
        for (int i = 0; i < sheeps.Count; i++)
        {
            //var query = from s in Sheeps
            //            where (Sheeps[i] - s).magnitude < Config.R_a
            //            select Sheeps[i] - s;
            //Debug.Log(query.ToList<Vector2>().ToString());

            var temp = new List<Vector2>();
            foreach (var s in sheeps)
            {
                Vector2 vec = sheeps[i] - s;
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
        var matC = Enumerable.Repeat(Vector2.zero, sheeps.Count).ToList();
        Vector2 GCM = GetGCM();
        for (int i = 0; i < sheeps.Count; i++)
        {
            if (Vector2.zero != matS[i])
            {
                matC[i] = GCM - sheeps[i];
                matC[i] = matC[i].normalized;
            }
        }
        return matC;
    }

    private List<Vector2> GetNoise()
    {
        // 噪声以一定概率出现
        var matNoise = Enumerable.Repeat(Vector2.zero, sheeps.Count).ToList();
        for (int i = 0; i < sheeps.Count; i++)
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
        return Mean(sheeps);
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
        sheeps.Clear();
        // 获取点坐标集
        foreach (var obj in objs)
        {
            sheeps.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
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
        Vector3 temp = Generator.Instance.target.transform.position;
        Vector2 vec = new Vector2(temp.x, temp.y);

        // 5是一个暂定参数
        if ((GCM - vec).magnitude <= 5)
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
        foreach (var a in sheeps)
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
        foreach (var agent in sheeps)
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
        Vector2 vec = Generator.Instance.shepherd.transform.position;
        foreach (var agent in sheeps)
        {
            float d = (agent - vec).magnitude;
            if (d < min)
            {
                // nearest = agent;
                min = d;
            }
        }
        return min;
    }

    public void Reset()
    {
        // 重定位羊群位置
        for (int i = 0; i < Config.N; i++)
        {
            float x = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.x;
            float y = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.y;
            objs[i].transform.position = new Vector3(x, y, 0);
        }
        time = 0;
    }

    public void ReInit()
    {
        DestroyObjs();
        sheeps = new List<Vector2>();
        GameObject prefab = Resources.Load<GameObject>("Prefabs/sheep") as GameObject;
        for (int i = 0; i < Config.N; i++)
        {
            // 生成羊只
            float x = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.x;
            float y = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPos.y;
            GameObject go = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            go.name = "" + i;
            sheeps.Add(new Vector2(x, y));
            objs.Add(go);

            time = 0;
        }
    }

    public void DestroyObjs()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
        objs = new List<GameObject>();
    }
}

