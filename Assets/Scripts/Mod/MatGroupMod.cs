using NumSharp;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用矩阵运算的羊群控制代码
/// </summary>
class MatGroupMod : UnitySingleton<MatGroupMod>
{
    #region sheeps' position info
    public NDArray sheeps;
    public List<GameObject> objs;
    #endregion

    #region Instances
    public Generator generator;
    #endregion

    private void Start()
    {
        //InitInstance();
        //InitSheeps();
        Test.NumPyTest();
    }


    private void FixedUpdate()
    {
    }

    /// <summary>
    /// 羊群坐标初始化
    /// </summary>
    private void InitSheeps()
    {
        int[] size = { Config.N, 2 };
        sheeps = new NumPyRandom().normal(0.0, 20, size);

    }

    /// <summary>
    /// 单例对象的实例化
    /// </summary>
    private void InitInstance()
    {
        generator = Generator.Instance;
    }

    public NDArray GetH()
    {
        return null;
    }

    /// <summary>
    /// 获取羊狼排斥力
    /// </summary>
    /// <returns></returns>
    private NDArray GetS()
    {
        //if (Generator.Instance.shepherd == null)
        //{
        //    return matS;
        //}
        var arr = Function.Vec3ToNDArray(generator.shepherd.transform.position);
        int[] size = { 1, 2 };
        var temp = new NDArray(arr, size);
        var matShepherd = Function.Duplicate(temp, sheeps.shape[0]);
        Debug.Log(sheeps);
        Debug.Log(matShepherd);
        var sub = sheeps - matShepherd;
        var matS = Function.Adjust(sub, Config.R_s);
        return matS;

    }

    //private NDArray GetA()
    //{
    //    for(int i = 0; i < sheeps.shape[0]; i++)
    //    {
    //        NDArray temp = sheeps[i];
    //        var matTemp = Function.Duplicate(temp, sheeps.shape[0]);
    //        var sub = matTemp - sheeps;
            
    //    }
    //}

    //private NDArray GetC()
    //{

    //}

    //private NDArray GetNoise()
    //{

    //}

    //private NDArray GetGCM()
    //{

    //}

    private void RefreshAgents()
    {

    }

    public bool IsTargetOK()
    {
        return false;
    }

    public bool WithinGCM()
    {
        return false;
    }

    public float GetNearestSheep()
    {
        return 0f;
    }

    public void Reset()
    {

    }
}
