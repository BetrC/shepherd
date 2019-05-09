using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏的数据信息
/// </summary>
public class Statistical : UnitySingleton<Statistical>
{

    public DBConnect dBConnect;

    /// <summary>
    /// 当前模拟的羊只数量
    /// I Love You Three Thousands Times
    /// </summary>
    public int sheepNum = 0;

    /// <summary>
    /// 当前循环数
    /// </summary>
    public int circle = 0;

    /// <summary>
    /// 当前步数
    /// </summary>
    public int step;

    /// <summary>
    /// 收集羊群步数
    /// </summary>
    public int collectStep = 0;

    /// <summary>
    /// 驱赶羊群步数
    /// </summary>
    public int driveStep = 0;

    /// <summary>
    /// 每一循环的牧羊犬距羊群中心点的距离
    /// </summary>
    public List<float> disToGCM = new List<float>();

    /// <summary>
    /// 完成时间
    /// </summary>
    public float timeOK = 0;

    /// <summary>
    /// 期望完成时间
    /// </summary>
    public float expectTime = 0;

    /// <summary>
    /// 羊群中心与牧羊犬距离+羊群中心与目标点距离
    /// </summary>
    public float length = 0f;

    private void Start()
    {
        dBConnect = DBConnect.Instance;
    }

    public void UpdateLength()
    {
        Vector2 gcm = GroupMod.Instance.GetGCM();
        Vector2 target = Config.targetPos;
        Vector2 shepherd = Config.shepherdPos;
        length = (gcm - target).magnitude + (gcm - shepherd).magnitude;
    }

    /// <summary>
    /// 牧羊犬检测距离与步数
    /// </summary>
    public void UpLoadDistanceTest()
    {
        Debug.Log("上传牧羊犬检测距离测试相关数据");
        dBConnect.InsertIntoDistanceTest(Config.R_s, step);

    }

    /// <summary>
    /// 牧羊犬数量与完成时间
    /// </summary>
    public void UpLoadFinishStep()
    {
        Debug.Log("上传数据：");
        Debug.Log("Num: " + sheepNum + "  Step: " + step);
        dBConnect.InsertIntoFinishStep(sheepNum, step);
    }


    public void UpLoadRaTest()
    {
        Debug.Log("上传数据：");
        dBConnect.InsertIntoRaTest(Config.R_a, step);
    }

    public void UpLoadShepherdAction()
    {
        Debug.Log("上传数据：");
        float all = collectStep + driveStep;
        float collectPercent = (float)collectStep / all;
        float drivePercent = (float)driveStep / all;
        dBConnect.InsertIntoShepherdAction(sheepNum, collectPercent, drivePercent);
    }

    public void UpLoadRandomGenerateTest()
    {
        Debug.Log("上传数据：");
        dBConnect.InsertIntoRandomGenerateTest(length, step);
    }

    /// <summary>
    /// 重置统计数据
    /// </summary>
    public void ResetData()
    {
        timeOK = 0;
        step = 0;
        collectStep = 0;
        driveStep = 0;
        circle = 0;
        length = 0;
    }

}
