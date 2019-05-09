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

        if (Config.randomBorn)
        {
            RandomBorn();
        }

        // 添加目标点
        GameObject go = Resources.Load<GameObject>("Prefabs/target") as GameObject;
        target = Instantiate(go, Config.targetPos, Quaternion.identity);

        // 添加牧羊犬
        go = AgentFactory.GetAgent();
        shepherd = Instantiate(go, Config.shepherdPos, Quaternion.identity);
    }

    public void ReSet()
    {
        if (Config.randomBorn)
        {
            RandomBorn();
        }
        shepherd.transform.position = Config.shepherdPos;
        target.transform.position = Config.targetPos;
    }


    /// <summary>
    /// 随机生成位置
    /// </summary>
    public void RandomBorn()
    {
        Config.targetPos = new Vector3(GenerateRandomX(), GenerateRandomY(), 0);
        Config.shepherdPos = new Vector3(GenerateRandomX(), GenerateRandomY(), 0);
    }

    private float GenerateRandomX()
    {
        return Random.Range(-Config.MapWidth, Config.MapWidth);
    }

    private float GenerateRandomY()
    {
        return Random.Range(-Config.MapLength, Config.MapLength);
    }
}
