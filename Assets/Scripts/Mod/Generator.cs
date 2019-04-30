using UnityEngine;

public class Generator : UnitySingleton<Generator>
{
    /// <summary>
    /// 牧羊犬对象
    /// </summary>
    public GameObject shepherd;

    /// <summary>
    /// 目标点
    /// </summary>
    public GameObject target;

    // Use this for initialization
    void Start()
    {
        // 生成羊群,已在SheepGroup类中实现

        // 添加目标点
        GameObject go = Resources.Load<GameObject>("Prefabs/target") as GameObject;
        target = Instantiate(go, Config.targetPos, Quaternion.identity);

        // 添加牧羊犬
        go = AgentFactory.GetAgent();
        shepherd = Instantiate(go, Config.shepherdPos, Quaternion.identity);
    }

    public void ReSet()
    {
        shepherd.transform.position = Config.shepherdPos;
        target.transform.position = Config.targetPos;
    }

}
