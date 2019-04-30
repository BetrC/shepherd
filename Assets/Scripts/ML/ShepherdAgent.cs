using MLAgents;
using UnityEngine;

public class ShepherdAgent : Agent
{

    private GroupMod groupMod;

    public Vector3 shepherdPoint = new Vector3(60, 60, 0);

    public float lastDis = 0;

    public Vector2 target;

    public override void InitializeAgent()
    {
        groupMod = GroupMod.Instance;
        target = new Vector2(Config.targetPos.x, Config.targetPos.y);
        lastDis = (target - Config.bornPos).magnitude;
    }

    public override void CollectObservations()
    {
        Vector2 GCM = groupMod.GetGCM();
        AddVectorObs(GCM);
        Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        AddVectorObs(pos);
        Vector2 furthest = groupMod.GetFurthestSheep();
        AddVectorObs(furthest);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            Vector3 vec = new Vector3(vectorAction[0], vectorAction[1], 0);
            vec.Normalize();
            float speed = Mathf.Clamp(vectorAction[2], 0f, 1.5f);
            gameObject.transform.position += vec;
        }

        if (groupMod.IsTargetOk())
        {
            SetReward(100f);
            Done();
        }
        else
        {
            float dis = CalDistance();
            AddReward(Mathf.Clamp((lastDis - dis), -1, 1));
            //if (groupMod.WithinGCM())
            //{
            //    AddReward(1f);
            //}
            if (groupMod.time > 20)
            {
                AddReward(-100f);
                Done();
            }
        }
    }

    private float CalDistance()
    {
        Vector2 GCM = groupMod.GetGCM();
        return (GCM - target).magnitude;
    }

    public override void AgentReset()
    {
        Debug.Log("Agent Reset");
        GroupMod.Instance.Reset();

        // 重定位牧羊犬位置
        transform.position = shepherdPoint;

        lastDis = (target - Config.bornPos).magnitude;
    }
}
