public class Manager : UnitySingleton<Manager>
{

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
        GroupMod.Instance.Reset();

        // 重定位牧羊犬位置
        Generator.Instance.ReSet();
    }
}
