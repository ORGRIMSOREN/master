using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    public const  float fadeDuration    = 0.35f;
    public const  float targetAlpha     = 0.45f;
    public static float secondThreshold = 0.012f; // 每 1 秒更新一次遊戲秒數
    public static int   secondHold      = 59; // 0~59 秒
    public static int   minuteHold      = 59; // 0~59 分
    public static int   hourHold        = 23; // 0~23 時
    public static int   dayHold         = 30; // 每個月 30 天（可改）
    public static int   seasonHold      = 3;  // 春夏秋冬 共 4 個（索引 0~3）
}
