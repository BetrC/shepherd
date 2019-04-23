using NumSharp;
using UnityEngine;
class Test
{

    public static void NumPyTest()
    {
        //int[] size = { 1, 2 };
        //var nd = new NumPyRandom().normal(0.0, 20, size);
        ////var shepherd = Function.Vec3ToNDArray(new Vector3(1, 1, 0));
        ////var res = Function.Duplicate(shepherd, 10);

        //NDArray res = Function.Duplicate(nd, 10);
        //int[] size2 = { 10, 2 };
        //var nc = new NumPyRandom().normal(0.0, 20, size2);

        //Debug.Log(res.ToString());
        //Debug.Log(nc.ToString());

        //var end = res - nc;
        //Debug.Log(end.ToString());
        //int[] size = { 1, 2 };
        //var nd = new NumPyRandom().normal(0.0, 20, size);
        //Function.Normalize(nd);
        //Debug.Log(nd.ToString());


        /////////////////////////////测试NDArray与Vector的相互转换////////////////////////////////
        ///NDArray to Vector3
        //int[] size = { 2, 2 };
        //var nd = new NumPyRandom().normal(0.0, 20, size);
        //Vector3 vec = Function.NDArrayToVec3(nd);
        //Debug.Log(vec.ToString());
        //Debug.Log(nd);
        //Function.Normalize(nd);
        //Debug.Log(nd);
        // Vector3 To NDArray
        //var res = Function.Vec3ToNDArray(vec);
        ///////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////Test GetS()////////////////////////////////////////

        //int[] size2 = { Config.N, 2 };
        //var sheeps = new NumPyRandom().normal(0.0, 20, size2);
        ////if (Generator.Instance.shepherd == null)
        ////{
        ////    return matS;
        ////}
        //var arr = Function.Vec3ToNDArray(new Vector3(60, 60, 0));
        //int[] size = { 1, 2 };
        //var temp = new NDArray(arr, size);
        //var matShepherd = Function.Duplicate(temp, sheeps.shape[0]);
        //Debug.Log(sheeps);
        //Debug.Log(matShepherd);
        //var sub = sheeps - matShepherd;
        //var matS = Function.Adjust(sub, Config.R_s);
        //Debug.Log(matS);
        ////////////////////////////////////Test GetS()////////////////////////////////////////
        ///


        ////////////////////////////////////Test Mean()////////////////////////////////////////
        int[] size = { 3, 2 };
        var nd = new NumPyRandom().normal(0.0, 20, size);
        Debug.Log(nd);
        var mean = Function.Mean(nd);
        Debug.Log(mean);
        ////////////////////////////////////Test Mean()////////////////////////////////////////
        


    }

}
