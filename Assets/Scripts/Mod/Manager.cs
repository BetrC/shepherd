public class Manager : UnitySingleton<Manager>
{

    //private MatGroupMod groupMod;

    private GroupMod groupMod;

    void Start()
    {
        //groupMod = MatGroupMod.Instance;

        groupMod = GroupMod.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (groupMod.IsTargetOk() && groupMod.WithinGCM())
        {
            ReLoad();
        }
    }

    private void ReLoad()
    {
        groupMod.Reset();

        // 重定位牧羊犬位置
        Generator.Instance.ReSet();
    }
}
