using MLAgents;
using UnityEngine;

public class ShepherdAcademy : Academy
{

    public override void InitializeAcademy()
    {

    }

    public override void AcademyReset()
    {
        //ReLoad();
    }

    public override void AcademyStep()
    {
        //if (GroupMod.Instance.IsTargetOk() && GroupMod.Instance.WithinGCM())
        //{
        //    Done();
        //}
    }

    private void ReLoad()
    {
        Debug.Log("Academy Reload");
        GroupMod.Instance.Reset();

        // 重定位牧羊犬位置
        Generator.Instance.ReSet();
    }
}
