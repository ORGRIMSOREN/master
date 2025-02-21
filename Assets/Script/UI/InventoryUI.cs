using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mfram.Inventory
{
public class InventoryUI : MonoBehaviour
{
    [SerializeField]private SlotUI[] playerSlot;

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



}
}