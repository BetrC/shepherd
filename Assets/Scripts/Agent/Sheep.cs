using NumSharp;
using UnityEngine;
/// <summary>
/// 羊只相关类
/// </summary>
public class Sheep : MonoBehaviour
{

    private Vector2 inertia = Vector2.zero;

    public void Action(Vector2 vec)
    {
        inertia = Config.h * inertia + vec;
        transform.position += new Vector3(inertia.x, inertia.y, 0);
    }

    public void MatAction(NDArray nd)
    {
        transform.position = Function.NDArrayToVec3(nd);
    }

    ///// <summary>
    ///// 获取合力
    ///// </summary>
    ///// <returns></returns>
    //public Vector2 GetH()
    //{
    //    Vector2 S = Config.Ro_s * GetS();
    //    Vector2 H = Config.e * GetNoise();
    //    if (S != Vector2.zero)
    //    {
    //        Vector2 Iner = Config.h * GetInertia();
    //        Vector2 C = Config.c * GetC();
    //        Vector2 A = Config.Ro_a * GetA();
    //        H += Iner + C + A + S;
    //    }
    //    H.Normalize();
    //    return Config.sheepSpeed * H;
    //}
}
