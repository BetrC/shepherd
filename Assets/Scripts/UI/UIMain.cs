using UnityEngine;
using UnityEngine.UI;

class UIMain : MonoBehaviour
{
    public Button buttonSetting;

    public Button buttonStop;

    public Button buttonClose;

    public Button ButtonReload;

    public GameObject prefab;

    public bool reset = false;

    public bool stop = false;

    private void Start()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Setting") as GameObject;

        buttonSetting = GameObject.Find("Buttons/Button-Setting").GetComponent<Button>();

        buttonClose = GameObject.Find("Buttons/Button-Close").GetComponent<Button>();

        buttonStop = GameObject.Find("Buttons/Button-Stop").GetComponent<Button>();

        ButtonReload = GameObject.Find("Buttons/Button-Reload").GetComponent<Button>();


        buttonSetting.onClick.AddListener(delegate { OpenUISetting(); });
        buttonStop.onClick.AddListener(delegate { OnClickButtonStop(); });
        buttonClose.onClick.AddListener(delegate { OnclickButtonClose(); });
        ButtonReload.onClick.AddListener(delegate { OnClickButtonReload(); });
    }


    public void OnClickButtonStop()
    {
        if (stop)
        {
            Time.timeScale = 1;
            stop = false;
            buttonStop.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PNG/stop-circle");
        }
        else
        {
            Time.timeScale = 0;
            stop = true;
            buttonStop.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/PNG/play-circle");
        }
    }


    public void OnclickButtonClose()
    {
        Application.Quit();
    }

    public void OnClickButtonReload()
    {
        Manager.Instance.ReSet();
    }


    public void OpenUISetting()
    {
        var UISetting = Instantiate(prefab);
        UISetting.SetActive(true);
        Time.timeScale = 0;
        UISetting.transform.localScale = Vector3.zero;
    }
}