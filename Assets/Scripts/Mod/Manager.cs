using UnityEngine;

public class Manager : UnitySingleton<Manager>
{

    //private MatGroupMod groupMod;

    private GroupMod groupMod;

    private Statistical statistical;

    void Start()
    {
        //groupMod = MatGroupMod.Instance;

        groupMod = GroupMod.Instance;

        statistical = Statistical.Instance;
    }

    private void LateUpdate()
    {
        // 数据统计
        if (Config.useStatistical)
        {
            // 统计占比
            if (groupMod.WithinGCM())
            {
                statistical.driveStep++;
            }
            else
            {
                statistical.collectStep++;
            }

            statistical.step++;
            statistical.sheepNum = Config.N;
            statistical.timeOK += Time.fixedDeltaTime;
        }
        if (groupMod.IsTargetOk() && groupMod.WithinGCM())
        {
            ReLoad();
        }

    }

    private void ReLoad()
    {

        // 数据统计
        if (Config.useStatistical)
        {

            statistical.circle++;
            if (statistical.circle == 5)
            {
                statistical.step /= 5;

                RaTest();

                statistical.ResetData();
                ReSet();
                return;
            }
        }

        groupMod.Reset();

        // 重定位牧羊犬位置
        Generator.Instance.ReSet();
        Generator.Instance.shepherd.GetComponent<DrawLocusMod>().Reset();
    }


    public void ReSet()
    {
        groupMod.ReInit();
        Generator.Instance.ReSet();

        statistical.UpdateLength();
    }


    /// <summary>
    /// 完成所需步数与羊群数量关系测试
    /// </summary>
    private void FinishStepTest()
    {
        statistical.UpLoadFinishStep();
        Config.N += 3;
    }

    /// <summary>
    /// 完成所需步数与牧羊犬检测距离关系测试
    /// </summary>
    private void DistanceTest()
    {
        statistical.UpLoadDistanceTest();
        Config.R_s += 2;
    }

    /// <summary>
    /// 羊群间排斥距离与完成所需步数关系测试
    /// </summary>
    private void RaTest()
    {
        statistical.UpLoadRaTest();
        Config.R_a += 0.1f;
    }

    private void ShepherdActionTest()
    {
        statistical.UpLoadShepherdAction();
        Config.N += 3;
    }

    private void RandomGenerateTest()
    {
        statistical.UpLoadRandomGenerateTest();
    }
}
