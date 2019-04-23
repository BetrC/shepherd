using NumSharp;
using System;
using UnityEngine;

/// <summary>
/// 保存一些全局方法
/// </summary>
class Function
{
    /// <summary>
    /// 将vector3坐标转化为 1×2 NDArray矩阵
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static double[] Vec3ToNDArray(Vector3 vec)
    {
        double[] arr = { vec.x, vec.y };
        return arr;
    }

    /// <summary>
    /// 将vector2坐标转化为 1×2 NDArray矩阵
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static double[] Vec2ToNDArray(Vector2 vec)
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
        (double x, double y) = (nd[0], nd[1]);
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

    public static NDArray Adjust(NDArray nd, float dis)
    {
        var res = np.zeros(nd.shape);
        for (int i = 0; i < nd.shape[0]; i++)
        {
            Vector3 vec = NDArrayToVec3(nd[i]);
            if (vec.magnitude < dis)
            {
                vec.Normalize();
                double[] arr = Vec3ToNDArray(vec);
                (res[i, 0], res[i, 1]) = (arr[0], arr[1]);
            }
        }
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
            double[] arr = Vec3ToNDArray(vec);
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
}
