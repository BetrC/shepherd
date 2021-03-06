﻿using UnityEngine;
/// <summary>
/// 模型相关配置
/// </summary>
public class Config
{

    #region 初始化位置信息

    /// <summary>
    /// 地图宽度
    /// </summary>
    public static float MapWidth = 150f;
    /// <summary>
    /// 地图高度
    /// </summary>
    public static float MapLength = 75f;

    /// <summary>
    /// 牧羊犬位置
    /// </summary>
    public static Vector3 shepherdPos = new Vector3(60, 60, 0);
    /// <summary>
    /// 目标点位置
    /// </summary>
    public static Vector3 targetPos = new Vector3(-60, 60, 0);
    /// <summary>
    /// 羊只生成点位置
    /// </summary>
    public static Vector2 bornPos = Vector2.zero;

    #endregion

    #region 羊群相关参数

    /// <summary>
    /// 地图宽度
    /// </summary>
    public static float L = 150f;

    public static float bornRange = 20.0f;

    /// <summary>
    /// 羊只数量(1-201)
    /// </summary>
    public static int N = 46;

    /// <summary>
    /// 牧羊犬检测距离
    /// </summary>
    public static float R_s = 60f;

    /// <summary>
    /// 羊群间交互距离
    /// 依据为羊只半径为1.5单位，取直径稍大点的值
    /// </summary>
    public static float R_a = 3.2f;


    // 三种力之间的关系为 Ro_a > c > Ro_s

    /// <summary>
    /// 来自另一个代理人的相对排斥力
    /// </summary>
    public static float Ro_a = 2f;

    /// <summary>
    /// 来自邻近邻居的相对吸引力
    /// </summary>
    public static float c = 1.05f;

    /// <summary>
    /// 来自牧羊犬的相对排斥力
    /// </summary>
    public static float Ro_s = 1f;

    /// <summary>
    /// 朝着前一个方向前进的相对作用力
    /// </summary>
    public static float h = 0.5f;

    /// <summary>
    /// 角度噪音作用力
    /// </summary>
    public static float e = 0.3f;

    /// <summary>
    /// 羊只每帧位移
    /// </summary>
    public static float sheepSpeed = 1f;

    /// <summary>
    /// 放牧时每步移动的概率
    /// </summary>
    public static float p = 0.05f;

    #endregion

    #region 牧羊犬相关参数

    /// <summary>
    /// 牧羊犬每帧位移
    /// </summary>
    public static float shephardSpeed = 1.5f;

    #endregion

    #region for local shephard

    /// <summary>
    /// 局部牧羊犬所能看到的羊只数量
    /// </summary>
    public static int N_s = 20;

    /// <summary>
    /// 局部牧羊犬可视角度
    /// </summary>
    public static float beta = 180;
    #endregion

    #region 功能参数
    /// <summary>
    /// 是否使用机器学习代理
    /// </summary>
    public static bool useML = false;

    /// <summary>
    /// 是否使用噪音
    /// </summary>
    public static bool useNoise = true;

    /// <summary>
    /// 是否显示移动轨迹
    /// </summary>
    public static bool showLocus = true;

    public static bool useStatistical = false;

    public static bool randomBorn = true;

    #endregion
}
