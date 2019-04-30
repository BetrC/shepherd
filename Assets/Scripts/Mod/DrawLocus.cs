using UnityEngine;

public class DrawLocus : MonoBehaviour
{

    public LineRenderer lineRenderer;

    public int index = 0;
    public int length = 0;

    // Use this for initialization
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = Resources.Load<Material>("Materials/line");

        //lineRenderer.startColor = Color.black;
        //lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    private void FixedUpdate()
    {
        if (!Config.showLocus)
        {
            return;
        }
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        SetPosition(transform.position);
    }

    public void SetPosition(Vector3 pos)
    {
        length++;
        lineRenderer.positionCount = length;
        while (index < length)
        {
            lineRenderer.SetPosition(index, pos);
            index++;
        }
    }

    public void Reset()
    {
        index = 0;
        length = 0;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }
}
