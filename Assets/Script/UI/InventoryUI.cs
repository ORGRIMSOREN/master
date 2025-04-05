using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Mfram.Inventory

{
public class InventoryUI : MonoBehaviour
{

    [SerializeField]public ItemToolTip itemToolTip;
    
    [Header("玩家背包ui")]
    [SerializeField]private GameObject bagui;
    private bool bagopened;
    [SerializeField]private SlotUI[] playerSlot;
    
    [Header("拖曳圖片")]
    public Image dragitem;
    
    private void OnEnable() 
    {
        EventHandler.UpdateInventoryUI+=OnUpdateInventoryUI;
    }
    private void OnDisable() 
    {
        EventHandler.UpdateInventoryUI-=OnUpdateInventoryUI;
    }
        private void Start() 
    {
        //給格子添加序號
        for (int i = 0; i < playerSlot.Length; i++)
        {
            playerSlot[i].slotIndex = i;
        }    
        bagopened=bagui.activeInHierarchy;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenBagUI();
        }
    }

    private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        
        switch (location)
        {
            case InventoryLocation.Player:
                for (int i = 0; i < playerSlot.Length; i++)
                {
                    if (list[i].itemAmount>0)
                    {
                        var item=InventoryManager.Instance.GetItemDetails(list[i].itemID);
                        playerSlot[i].UpdateSlot(item,list[i].itemAmount);
                    }
                    else
                    {
                     playerSlot[i].UpdateEmptySlot();   
                    }
                }
                break;
        }
    }

    public void OpenBagUI()
    {
        bagopened= !bagopened;
        bagui.SetActive(bagopened);
    }

    public void UpdateSlotHightlight(int index)
    {
        foreach (var slot in playerSlot)
        {
            if (slot.isSelected && slot.slotIndex == index)
            {
                slot.slotHightlight.gameObject.SetActive(true);
            }
            else
            {
                slot.isSelected = false;
                slot.slotHightlight.gameObject.SetActive(false);
            }
        }
    }

}
}