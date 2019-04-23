using UnityEngine;

public class AgentFactory
{
    public static GameObject GetAgent()
    {
        string prefab = Config.useML ? "Prefabs/shepherd-ML" : "Prefabs/shepherd";
        //string prefab = "Prefabs/shepherd-Mat";
        GameObject go = Resources.Load<GameObject>(prefab) as GameObject;
        return go;
    }
}
