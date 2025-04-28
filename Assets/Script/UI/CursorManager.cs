using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{

    public  Sprite        normal , tool , seed , item;
    private Sprite        currentSprite;
    private Image         cursorImage;
    private RectTransform cursorCanvas;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;

    }

    private void Start()
    {
        cursorCanvas  = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage   = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;
        if (!IntractWithUi())
        {
            
        SetCursorImage(currentSprite);
        }
        else
        {
            SetCursorImage(normal );
        }
    }
    /// <summary>
    /// 設置鼠標圖片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color  = new Color(1 , 1 , 1 , 1);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails , bool isSelected)
    {
        if (!isSelected)//判斷是否選中
        {
            currentSprite = normal;
        }
        else//添加對應類型圖片
        {
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed      => seed ,
                ItemType.ChopTool  => tool ,
                ItemType.ReapTool  => tool ,
                ItemType.HoeTool   => tool ,
                ItemType.WaterTool => tool ,
                ItemType.BreakTool => tool ,
                ItemType.Commodity => item ,
                _                  => normal ,
            };
        }


    }
    /// <summary>
    /// 判斷是否與ui互動
    /// </summary>
    /// <returns></returns>
    private bool IntractWithUi()
    {
        if (EventSystem.current!=null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}

