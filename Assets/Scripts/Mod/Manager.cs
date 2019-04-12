using UnityEngine;

public class Manager : UnitySingleton<Manager>
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

    // Use this for initializatioqn
    void Start()
    {
        // 生成羊群,已在SheepGroup类中实现

        // 添加牧羊犬
        GameObject go = Resources.Load<GameObject>("Prefabs/shepherd") as GameObject;
        shepherd = Instantiate(go, shepherdPoint, Quaternion.identity);

        // 添加目标点
        go = Resources.Load<GameObject>("Prefabs/target") as GameObject;
        target = Instantiate(go, targetPoint, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if (GroupMod.Instance.IsTargetOk() && GroupMod.Instance.WithinGCM())
        {
            ReLoad();
        }
    }

    private void ReLoad()
    {
        // 重定位羊群位置
        for (int i = 0; i < Config.N; i++)
        {
            float x = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPoint.x;
            float y = Random.Range(-Config.bornRange, Config.bornRange) + Config.bornPoint.y;
            GroupMod.Instance.Sheeps[i] = new Vector2(x, y);
        }

        // 重定位牧羊犬位置
        shepherd.transform.position = shepherdPoint;
    }
}
