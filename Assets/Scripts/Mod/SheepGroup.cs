//using NumSharp.Core;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 羊群类
///// </summary>
//public class SheepGroup : UnitySingleton<SheepGroup>
//{
//    /// <summary>
//    /// 羊只生成参考点
//    /// </summary>
//    // public Vector3 pos = Vector3.zero;

//    // public List<GameObject> sheepGroup;

//    static Shape shape = new Shape(Config.N, 2);
//    /// <summary>
//    /// 绵羊位置坐标
//    /// </summary>
//    public NDArray sheeps;

//    public List<Vector2> Agents { get; set; } = new List<Vector2>();

//    // Use this for initialization
//    void Start()
//    {
//        //Debug.Log(sheeps.ToString());
//        // sheepGroup = new List<GameObject>();
//        Debug.Log("随机生成羊群");
//        Init();
//    }

//    /// <summary>
//    /// 初始化，生成羊群,46只
//    /// </summary>
//    private void Init()
//    {
//        sheeps = np.random.normal(0, 1.0, shape.Dimensions) * 10;
//        GameObject prefab = Resources.Load<GameObject>("Prefabs/sheep") as GameObject;
//        for (int i = 0; i < sheeps.shape[0]; i++)
//        {
//            // 生成羊只
//            GameObject go = Instantiate(prefab, new Vector3((float)sheeps[i, 0], (float)sheeps[i, 1], 0), Quaternion.identity);
//            go.name = i.ToString();
//        }

//        //// 获取点坐标集
//        //foreach (var obj in sheepGroup)
//        //{
//        //    Agents.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
//        //}
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        RefreshAgents();
//    }

//    /// <summary>
//    /// 羊群排斥力
//    /// 计算优化，所有羊群一起计算
//    /// </summary>
//    public Vector2 GetA()
//    {
//        // 羊群排斥力计算方式为计算与当前羊只邻近的羊群对其排斥力的合力
        
//        return Vector2.zero;
//    }

//    public Vector2 GetA(Vector2 agent)
//    {
//        Vector2 force = Vector2.zero;
//        foreach (Vector2 pos in Agents)
//        {
//            Vector2 vec = agent - pos;
//            if (vec.magnitude <= Config.R_a)
//            {
//                vec.Normalize();
//                force += vec;
//            }
//        }
//        force.Normalize();
//        return force;
//    }

//    /// <summary>
//    /// 中心力-计算优化
//    /// 对所有羊只计算
//    /// </summary>
//    public void GetC()
//    {
//        // 中心力为羊群局部中心对该羊只的吸引力

//    }

//    /// <summary>
//    /// 获取某一agent的中心力
//    /// </summary>
//    /// <param name="agent"></param>
//    /// <returns></returns>
//    public Vector2 GetC(Vector2 agent)
//    {
//        Vector2 vec = Vector2.zero;
//        // 局部中心点
//        //Vector2 center = GetLCM(agent);
//        Vector2 center = GetGCM();
//        vec = center - agent;
//        vec.Normalize();
//        return vec;
//    }

//    /// <summary>
//    /// 羊狼排斥力
//    /// </summary>
//    public Vector2 GetS(Vector2 agent)
//    {
//        GameObject shepherd = Manager.Instance.shepherd;
//        Vector2 shepherdPos = new Vector2(shepherd.transform.position.x, shepherd.transform.position.y);
//        Vector2 vec = agent - shepherdPos;
//        if (vec.magnitude <= Config.R_s)
//        {
//            vec.Normalize();
//            return vec;
//        }
//        return Vector2.zero;
//    }

//    /// <summary>
//    /// 获取羊群局部中心点LCM
//    /// </summary>
//    /// <param name="agent">参照🐏只</param>
//    /// <returns></returns>
//    public Vector2 GetLCM(Vector2 agent)
//    {
//        Vector2 center = Vector2.zero;
//        List<Vector2> agents = SortByDis(agent);
//        for (int i = 0; i < Config.n; i++)
//        {
//            center += agents[i];
//        }
//        return center / Config.n;
//    }

//    /// <summary>
//    /// 获取GCM，羊群中心点
//    /// </summary>
//    /// <returns></returns>
//    public Vector2 GetGCM()
//    {
//        Vector2 center = Vector2.zero;

//        foreach (var vec in Agents)
//        {
//            center += vec;
//        }
//        return center / sheepGroup.Count;
//    }


//    /// <summary>
//    /// 以某只🐏为基准对羊群进行排序
//    /// </summary>
//    /// <param name="agent">排序参照羊只</param>
//    /// <returns></returns>
//    public List<Vector2> SortByDis(Vector2 agent)
//    {
//        Agents.Sort(
//                delegate (Vector2 v1, Vector2 v2)
//                {
//                    return (v1 - agent).magnitude.CompareTo((v2 - agent).magnitude);
//                }
//                );
//        return Agents;
//    }

//    /// <summary>
//    /// 刷新点坐标集
//    /// </summary>
//    private void RefreshAgents()
//    {
//        Agents.Clear();
//        // 获取点坐标集
//        foreach (var obj in sheepGroup)
//        {
//            Agents.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
//        }
//    }

//    /// <summary>
//    /// 任务完成检测
//    /// </summary>
//    /// <returns></returns>
//    public bool IsTargetOk()
//    {
//        Vector2 GCM = GetGCM();
//        // TODO 重复获取参数 未变变量应设为全局
//        Vector3 point = Manager.Instance.targetPoint;
//        Vector2 target = new Vector2(point.x, point.y);

//        // 5是一个暂定参数
//        if ((GCM - target).magnitude <= 5)
//        {
//            return true;
//        }
//        return false;
//    }

//    /// <summary>
//    /// 所有羊只是否与全局中心点在一定距离内
//    /// </summary>
//    /// <returns></returns>
//    public bool WithinGCM()
//    {
//        Vector2 GCM = GetGCM();
//        float fn = Config.R_a * Mathf.Sqrt(Config.N);
//        foreach (var a in Agents)
//        {
//            if ((a - GCM).magnitude > fn)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    public Vector2 GetFurthestSheep()
//    {
//        Vector2 furthest = Vector2.zero;
//        float dis = 0;
//        Vector2 GCM = GetGCM();
//        foreach (var agent in Agents)
//        {
//            float d = (agent - GCM).magnitude;
//            if (d > dis)
//            {
//                furthest = agent;
//                dis = d;
//            }
//        }
//        return furthest;
//    }

//    public float GetNearestSheep()
//    {
//        // Vector2 nearest = Vector2.zero;
//        float dis = float.MaxValue;
//        Vector2 shepherd = Manager.Instance.shepherd.transform.position;
//        foreach (var agent in Agents)
//        {
//            float d = (agent - shepherd).magnitude;
//            if (d < dis)
//            {
//                // nearest = agent;
//                dis = d;
//            }
//        }
//        return dis;
//    }
//}

///// 计算优化 每帧通用的数据保证只计算一次
///// 如： 全局中心点等