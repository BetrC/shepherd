using UnityEngine;
using UnityEngine.UI;


class UIToggleMgr : UIBase
{
    public Toggle useML;

    public Toggle useNoise;

    public Toggle showLocus;

    public override void Init()
    {
        useML = Global.GetToggle("ML");
        useNoise = Global.GetToggle("Noise");
        showLocus = Global.GetToggle("Locus");
    }

    public override void OnShow()
    {
        useML.isOn = Config.useML;
        useNoise.isOn = Config.useNoise;
        showLocus.isOn = Config.showLocus;
    }

    public override void PreOnShow()
    {
        useML.onValueChanged.AddListener(delegate { ValueChange(ref Config.useML, useML); });
        useNoise.onValueChanged.AddListener(delegate { ValueChange(ref Config.useNoise, useNoise); });
        showLocus.onValueChanged.AddListener(delegate { ValueChange(ref Config.showLocus, showLocus); });
    }


    public void ValueChange(ref bool target, Toggle toggle)
    {
        target = toggle.isOn;
    }
}

