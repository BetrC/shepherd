using System;
using UnityEngine;
using UnityEngine.UI;

class UIInputMgr : UIBase
{

    public InputField targetPosX;
    public InputField targetPosY;

    public InputField shepherdPosX;
    public InputField shepherdPosY;

    public InputField bornPosX;
    public InputField bornPosY;

    public override void Init()
    {
        targetPosX = GameObject.Find("Target-Pos/InputField-x").GetComponent<InputField>();
        targetPosY = GameObject.Find("Target-Pos/InputField-y").GetComponent<InputField>();

        shepherdPosX = GameObject.Find("Shepherd-Pos/InputField-x").GetComponent<InputField>();
        shepherdPosY = GameObject.Find("Shepherd-Pos/InputField-y").GetComponent<InputField>();

        bornPosX = GameObject.Find("Born-Pos/InputField-x").GetComponent<InputField>();
        bornPosY = GameObject.Find("Born-Pos/InputField-y").GetComponent<InputField>();

    }

    public override void PreOnShow()
    {
        targetPosX.onEndEdit.AddListener(delegate { ChangeTargetPosX(targetPosX); });
        targetPosY.onEndEdit.AddListener(delegate { ChangeTargetPosY(targetPosY); });

        shepherdPosX.onEndEdit.AddListener(delegate { ChangeShepherdPosX(shepherdPosX); });
        shepherdPosY.onEndEdit.AddListener(delegate { ChangeShepherdPosY(shepherdPosY); });

        bornPosX.onEndEdit.AddListener(delegate { ChangeBornPosX(bornPosX); });
        bornPosY.onEndEdit.AddListener(delegate { ChangeBornPosY(bornPosY); });
    }

    public override void OnShow()
    {
        targetPosX.text = Config.targetPos.x.ToString();
        targetPosY.text = Config.targetPos.y.ToString();

        shepherdPosX.text = Config.shepherdPos.x.ToString();
        shepherdPosY.text = Config.shepherdPos.y.ToString();

        bornPosX.text = Config.bornPos.x.ToString();
        bornPosY.text = Config.bornPos.y.ToString();

    }


    #region 输入值映射

    /// <summary>
    /// 目标点X
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeTargetPosX(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapWidth, Config.MapWidth);
        inputField.text = val.ToString();
        Config.targetPos.x = Convert.ToSingle(val);
        Debug.Log(Config.targetPos);
    }

    /// <summary>
    /// 目标点Y
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeTargetPosY(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapLength, Config.MapLength);
        inputField.text = val.ToString();
        Config.targetPos.y = Convert.ToSingle(val);
        Debug.Log(Config.targetPos);
    }

    /// <summary>
    /// 牧羊犬X
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeShepherdPosX(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapWidth, Config.MapWidth);
        inputField.text = val.ToString();
        Config.shepherdPos.x = Convert.ToSingle(val);
        Debug.Log(Config.shepherdPos);
    }

    /// <summary>
    /// 牧羊犬Y
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeShepherdPosY(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapLength, Config.MapLength);
        inputField.text = val.ToString();
        Config.shepherdPos.y = Convert.ToSingle(val);
        Debug.Log(Config.shepherdPos);
    }

    /// <summary>
    /// 羊群X
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeBornPosX(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapWidth, Config.MapWidth);
        inputField.text = val.ToString();
        Config.bornPos.x = Convert.ToSingle(val);
        Debug.Log(Config.bornPos);
    }

    /// <summary>
    /// 羊群Y
    /// </summary>
    /// <param name="inputField"></param>
    public void ChangeBornPosY(InputField inputField)
    {
        var val = Correct(inputField.text, -Config.MapLength, Config.MapLength);
        inputField.text = val.ToString();
        Config.bornPos.y = Convert.ToSingle(val);
        Debug.Log(Config.bornPos);
    }

    #endregion


    /// <summary>
    /// 对于用户输入进行边界校正，防止坐标超出边界
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public float Correct(string value, float min, float max)
    {
        float val = Convert.ToSingle(value);
        if (val < min)
        {
            val = min;
        }
        else if (val > max)
        {
            val = max;
        }
        return val;
    }
}
