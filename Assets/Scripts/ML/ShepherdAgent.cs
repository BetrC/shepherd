using MLAgents;
using UnityEngine;

public class ShepherdAgent : Agent
{

    private GroupMod groupMod;

    public float lastDis = 0;

    public Vector2 target;

    public float time = 0;

    public override void InitializeAgent()
    {
        groupMod = GroupMod.Instance;
        target = new Vector2(Config.targetPos.x, Config.targetPos.y);
        lastDis = (target - Config.bornPos).magnitude;
    }

    public override void CollectObservations()
    {
        AddVectorObs(target);
        Vector2 GCM = groupMod.GetGCM();
        AddVectorObs(GCM);
        Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        AddVectorObs(pos);
        Vector2 furthest = groupMod.GetFurthestSheep();
        AddVectorObs(furthest);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        time += Time.deltaTime;
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            Vector3 vec = new Vector3(vectorAction[0], vectorAction[1]);

            Vector2 GCM = groupMod.GetGCM();
            if (Vector2.Dot(vec, (GCM - new Vector2(transform.position.x, transform.position.y))) < 0)
            {
                vec = new Vector2(-vec.x, -vec.y);
            }
            vec.Normalize();
            float speed = Mathf.Clamp(vectorAction[2], 1f, 2f);
            gameObject.transform.position += speed * vec;
        }

        if (groupMod.IsTargetOk())
        {
            AddReward(1000f);
            Done();
        }
        else
        {
            if (time >= 20)
            {
                AddReward(-200f);
                Done();
            }
            if (groupMod.WithinGCM())
            {
                AddReward(1);
            }
            float dis = CalDistance();
            AddReward(Mathf.Clamp((lastDis - dis), -0.1f, 0.1f));
        }
        AddReward(-0.05f);
    }

    private float CalDistance()
    {
        Vector2 GCM = groupMod.GetGCM();
        return (GCM - target).magnitude;
    }

    public override void AgentReset()
    {
        Debug.Log("Agent Reset");
        time = 0;
        GroupMod.Instance.Reset();

        // 重定位牧羊犬位置
        Generator.Instance.ReSet();

        lastDis = (target - Config.bornPos).magnitude;
    }
}
