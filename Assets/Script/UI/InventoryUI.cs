using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mfram.Inventory

{
public class InventoryUI : MonoBehaviour
{
    
    [SerializeField]private GameObject bagui;
    [SerializeField] private SlotUI[] playerSlot; // 背包槽位
    [SerializeField] private SlotUI[] hotbarSlot;
    private bool bagopened;
    [Header("拖曳圖片")] public Image dragitem;
    
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
        // 初始化背包槽位
        for (int i = 0; i < playerSlot.Length; i++)
        {
            playerSlot[i].slotIndex = i;
            playerSlot[i].containerType = InventoryLocation.Player;
        }
    
        // 初始化快捷欄槽位
        for (int i = 0; i < hotbarSlot.Length; i++)
        {
            hotbarSlot[i].slotIndex = i;
            hotbarSlot[i].containerType = InventoryLocation.Hotbar;
        }
    
        bagopened = bagui.activeInHierarchy;
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
                UpdateSlots(playerSlot, list);
                break;
            case InventoryLocation.Hotbar:
                UpdateSlots(hotbarSlot, list);
                break;
        }
    }

    private void UpdateSlots(SlotUI[] slots, List<InventoryItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count && items[i].itemAmount > 0)
            {
                var item = InventoryManager.Instance.GetItemDetails(items[i].itemID);
                slots[i].UpdateSlot(item, items[i].itemAmount);
            }
            else
            {
                slots[i].UpdateEmptySlot();
            }
        }
    }

    public void OpenBagUI()
    {
        bagopened= !bagopened;
        bagui.SetActive(bagopened);
    }

    public void UpdateSlotHightlight(int index)
    {
        // 更新背包槽位高亮
        UpdateSlotArrayHighlight(playerSlot, index);
    
        // 更新快捷欄槽位高亮
        UpdateSlotArrayHighlight(hotbarSlot, index);
    }

    private void UpdateSlotArrayHighlight(SlotUI[] slots, int index)
    {
        foreach (var slot in slots)
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