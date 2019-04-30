using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public Button btnClose;

    public bool reload = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Inputs").AddComponent<UIInputMgr>();
        GameObject.Find("Sliders").AddComponent<UISliderMgr>();
        GameObject.Find("CheckBoxs").AddComponent<UIToggleMgr>();
        btnClose = GameObject.Find("TopBar/Button-Close").GetComponent<Button>();
        btnClose.onClick.AddListener(delegate { Close(); });
    }

    public void Close()
    {

        Destroy(gameObject);
        Manager.Instance.ReSet();
        Time.timeScale = 1;
    }
}
