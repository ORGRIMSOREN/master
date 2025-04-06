using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // 儲存遊戲內時間：秒、分、時、天、月、年
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    // 遊戲季節初始為春天（假設 Season 是 enum，例：春天 = 0, 夏天 = 1, 秋天 = 2, 冬天 = 3）
    private Season gameSeason = Season.春天;

    // 每個季節有幾個月，這裡設定為 3
    private int monthInSeason = 3;

    // 遊戲時間是否暫停
    public bool gameClockPause;

    // tikTime 是累積的時間（會加上 Time.deltaTime）
    private float tikTime;

    // 遊戲開始時會執行 NewGameTime，初始化時間
    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameDateEvent(gameHour, gameDay , gameMonth , gameYear , gameSeason);
        EventHandler.CallGameMinuteEvent(gameMinute , gameHour);
        
    }

    // 每一幀執行：如果遊戲時間沒被暫停，就加上經過的實際時間
    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            // 如果累積的時間超過「1 遊戲秒」（由 Settings.secondThreshold 決定）
            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime(); // 遊戲時間加一秒（同時會進位分鐘、時、日...）
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0 ; i < 60 ; i++)
            {
                UpdateGameTime();
            }
            
        }
    }

    // 初始化時間（新遊戲時用）
    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;        // 一開始時間設定為早上 7 點
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.春天;
    }

    // 時間遞進機制，每當遊戲過了一秒就呼叫這個方法
    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateGameTime()
    {
        gameSecond++;

        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if (gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 12)
                            gameMonth = 1;

                        // 月份進位的同時也要追蹤季節中的月份數
                        monthInSeason--;
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            // 換季節
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++; // 年份 +1
                            }

                            gameSeason = (Season)seasonNumber;

                            // 超過最大年限就重設（避免溢位）
                            if (gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear,gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute,gameHour);
        }

        // Debug用：輸出秒數與分鐘
         //Debug.Log("Second: " + gameSecond + " Minute: " + gameMinute);
    }
}

