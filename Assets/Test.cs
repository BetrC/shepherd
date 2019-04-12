using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> vec = new List<Vector2>();
        vec.Add(Vector2.up);
        vec.Add(new Vector2(1, 1));
        vec.Add(new Vector2(2, 3));
        vec.Add(new Vector2(0, 0));

        for (int i = 0; i < vec.Count; i++)
        {
            vec[i].Normalize();
            Debug.Log(vec[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
