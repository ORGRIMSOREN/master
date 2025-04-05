using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Mfram.Inventory
{
public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private SlotUI slotUI;
    InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
    private void Awake()
    {
        slotUI = GetComponent<SlotUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotUI.itemAmount !=0)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(true);
            inventoryUI.itemToolTip.SetUpItemToolTip(slotUI.itemDetails, slotUI.slotType);
            inventoryUI.itemToolTip.transform.position=transform.position + Vector3.up*60 ;
        }
        else
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
        
    }
    public void OnPointerExit(PointerEventData  eventData)
    {
        inventoryUI.itemToolTip.gameObject.SetActive(false);
    }

    
}
}
