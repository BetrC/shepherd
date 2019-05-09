using UnityEngine;
using UnityEngine.UI;


public class UISliderMgr : UIBase
{

    public Slider sheepNum;

    public Slider R_s;

    public Slider R_a;

    public Slider Ro_a;

    public Slider Ro_s;

    public Slider c;

    public Slider h;

    public Slider e;

    public Slider p;

    public Slider shepherdSpeed;

    public Slider sheepSpeed;


    public override void Init()
    {
        sheepNum = Global.GetSlider("Sheep-Num");
        R_s = Global.GetSlider("R_s");
        R_a = Global.GetSlider("R_a");
        Ro_a = Global.GetSlider("Ro_a");
        Ro_s = Global.GetSlider("Ro_s");
        c = Global.GetSlider("c");
        h = Global.GetSlider("h");
        e = Global.GetSlider("e");
        p = Global.GetSlider("p");
        shepherdSpeed = Global.GetSlider("Speed-Shepherd");
        sheepSpeed = Global.GetSlider("Speed-Sheep");
    }

    public override void PreOnShow()
    {

        sheepNum.onValueChanged.AddListener(delegate
        {
            Config.N = (int)sheepNum.value;
            sheepNum.handleRect.GetComponentInChildren<Text>().text = Config.N.ToString();
        });
        R_s.onValueChanged.AddListener(delegate { ValueChange(ref Config.R_s, R_s); });
        R_a.onValueChanged.AddListener(delegate { ValueChange(ref Config.R_a, R_a); });
        Ro_a.onValueChanged.AddListener(delegate { ValueChange(ref Config.Ro_a, Ro_a); });
        Ro_s.onValueChanged.AddListener(delegate { ValueChange(ref Config.Ro_s, Ro_s); });
        c.onValueChanged.AddListener(delegate { ValueChange(ref Config.c, c); });
        h.onValueChanged.AddListener(delegate { ValueChange(ref Config.h, h); });
        e.onValueChanged.AddListener(delegate { ValueChange(ref Config.e, e); });
        p.onValueChanged.AddListener(delegate { ValueChange(ref Config.p, p); });
        shepherdSpeed.onValueChanged.AddListener(delegate { ValueChange(ref Config.shephardSpeed, shepherdSpeed); });
        sheepSpeed.onValueChanged.AddListener(delegate { ValueChange(ref Config.sheepSpeed, sheepSpeed); });
    }

    public override void OnShow()
    {
        sheepNum.value = Config.N;
        R_s.value = Config.R_s;
        R_a.value = Config.R_a;
        Ro_a.value = Config.Ro_a;
        Ro_s.value = Config.Ro_s;
        c.value = Config.c;
        h.value = Config.h;
        e.value = Config.e;
        p.value = Config.p;
        shepherdSpeed.value = Config.shephardSpeed;
        sheepSpeed.value = Config.sheepSpeed;
    }

    public void ValueChange(ref float target, Slider slider)
    {
        target = slider.value;
        slider.handleRect.GetComponentInChildren<Text>().text = target.ToString();
    }
}

