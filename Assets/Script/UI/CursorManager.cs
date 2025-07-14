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
    //鼠標檢測
    private Camera mainCamera;
    private Grid currentGrid;
    
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnabled ;
    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent     += OnItemSelectedEvent;
        EventHandler.BeforeTransitionEvent += OnBeforeTransitionEvent;
        EventHandler.AfterTransitionEvent  += OnAfterTransitionEvent;
    }


    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent     -= OnItemSelectedEvent;
        EventHandler.BeforeTransitionEvent -= OnBeforeTransitionEvent;
        EventHandler.AfterTransitionEvent  -= OnAfterTransitionEvent;
    }
    private void Start()
    {
        cursorCanvas  = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage   = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(normal);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;
        if (!IntractWithUi()&& cursorEnabled)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
        }
        else
        {
            SetCursorImage(normal );
        }
    }

    private void OnBeforeTransitionEvent()
    {
        cursorEnabled = false;
    }
    private void OnAfterTransitionEvent()
    {
        cursorEnabled = true;
        currentGrid   = FindObjectOfType<Grid>();
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

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        
        Debug.Log("WorldPos"+mouseWorldPos+"GridPos"+mouseGridPos);
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

