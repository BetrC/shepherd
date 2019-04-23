using UnityEngine;

public class Generator : UnitySingleton<Generator>
{
    public Vector3 shepherdPoint = new Vector3(60, 60, 0);
    public Vector3 targetPoint = new Vector3(-60, 60, 0);

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
        target = Instantiate(go, targetPoint, Quaternion.identity);

        // 添加牧羊犬
        go = AgentFactory.GetAgent();
        shepherd = Instantiate(go, shepherdPoint, Quaternion.identity);

    }

    public void ReSet()
    {
        shepherd.transform.position = shepherdPoint;
    }
}
