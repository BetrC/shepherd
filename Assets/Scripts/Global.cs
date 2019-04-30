using NumSharp;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 保存一些全局方法
/// </summary>
class Global
{

    #region NumSharp的一些矩阵运算方法   不可用
    /// <summary>
    /// 将vector3坐标转化为 1×2 NDArray矩阵
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static double[] Vec3ToArray(Vector3 vec)
    {
        double[] arr = { vec.x, vec.y };
        return arr;
    }

    /// <summary>
    /// 将vector2坐标转化为 1×2 NDArray矩阵
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static double[] Vec2ToArray(Vector2 vec)
    {
        double[] arr = { vec.x, vec.y };
        return arr;
    }

    /// <summary>
    /// 将NDArray对象转化为Vector3对象
    /// 注意： 此处的NDArray应为 1×2
    /// </summary>
    /// <param name="nd"></param>
    /// <returns></returns>
    public static Vector3 NDArrayToVec3(NDArray nd)
    {
        double[] arr = nd.Array as double[];
        (double x, double y) = (arr[0], arr[1]);
        return new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), 0);
    }

    /// <summary>
    /// 创建一个以某nd为基准的重复NDArray
    /// </summary>
    /// <param name="nd"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static NDArray Duplicate(NDArray nd, int length)
    {
        var arr = new double[nd.size * length];

        // init raw data
        double x = nd[0, 0];
        double y = nd[0, 1];
        for (int i = 0; i < length; i++)
        {
            arr[2 * i] = x;
            arr[2 * i + 1] = y;
        }
        // create the NDArray
        var res = new NDArray(arr, new Shape(nd.shape[0] * length, nd.shape[1]));
        return res;
    }


    /// <summary>
    /// 矩阵每行标准化
    /// </summary>
    /// <param name="nd"></param>
    public static void Normalize(NDArray nd)
    {
        int[] size = { 1, 2 };
        for (int i = 0; i < nd.shape[0]; i++)
        {
            Vector3 vec = NDArrayToVec3(nd[i]).normalized;
            double[] arr = Vec3ToArray(vec);
            (nd[i, 0], nd[i, 1]) = (arr[0], arr[1]);
        }
    }

    public static NDArray Mean(NDArray nd)
    {
        double[] puffer = new double[nd.shape[1]];
        for (int i = 0; i < nd.shape[1]; i++)
        {
            double sum = 0;
            for (int j = 0; j < nd.shape[0]; j++)
            {
                sum += nd[j, i];
            }
            puffer[i] = sum / nd.shape[0];
        }
        var mean = new NDArray(puffer, new Shape(1, nd.shape[1]));
        return mean;
    }
    #endregion

    #region UI相关的一些方法

    /// <summary>
    /// 根据路径获取Slider组件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Slider GetSlider(string path)
    {
        return GameObject.Find(path + "/Slider").GetComponent<Slider>();
    }

    /// <summary>
    /// 根据路径获取开关组件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Toggle GetToggle(string path)
    {
        return GameObject.Find(path + "/Toggle").GetComponent<Toggle>();
    }


    #endregion
}
