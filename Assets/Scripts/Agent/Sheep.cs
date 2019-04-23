using UnityEngine;
/// <summary>
/// 羊只相关类
/// </summary>
public class Sheep : MonoBehaviour
{

    private Vector2 Inertia = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Action(Vector2 vec)
    {
        Inertia = Config.h * Inertia + vec;
        transform.position += new Vector3(Inertia.x, Inertia.y, 0);
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
