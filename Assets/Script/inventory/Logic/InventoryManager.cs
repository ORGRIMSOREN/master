using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Mfram.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField]public ItemDataList_SO itemDataList_SO;
        [SerializeField]public InventoryBag_SO PlayerBag;
        private void Start() 
        {
            
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,PlayerBag.itemList);
        }
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.ItemDetailsList.Find(i => i.itemID == ID);
        }
        
        /// <summary>
        /// 添加物品到Player背包里
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestroy">是否要销毁物品</param>
        public void AddItem(Item item, bool toDestroy)
        {
            var index = GetItemIndexInBag(item.itemID);
            AddItemIndex(item.itemID, index, 1);

            Debug.Log(GetItemDetails(item.itemID).itemID + " Name:" + GetItemDetails(item.itemID).itemName);
            if (toDestroy)
            {
                Destroy(item.gameObject);
            }
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,PlayerBag.itemList);
        }

        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns>是否有空位</returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if (PlayerBag.itemList[i].itemID == 0)
                    return true;
            }
            return false;
            
        }

        /// <summary>
        /// 通过物品ID找到背包已有物品位置
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <returns>-1则没有这个物品否则返回序号</returns>
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < PlayerBag.itemList.Count; i++)
            {
                if (PlayerBag.itemList[i].itemID == ID)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="index">序号</param>
        /// <param name="amount">数量</param>
        private void AddItemIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity()) // 背包沒有該物品 同時有空位
            {
                var playerBagItem = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < PlayerBag.itemList.Count; i++)
                {
                    if (PlayerBag.itemList[i].itemID == 0)
                    {
                        PlayerBag.itemList[i] = playerBagItem; // 放置新物品到空位
                        break;
                    }
                    
                }
            }
            else if (index >= 0 && index < PlayerBag.itemList.Count)
            {
                int currentAmount = PlayerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                PlayerBag.itemList[index] = item;
            }
            

        }

        public void SwapItem(int SlotIndex, int targetIndex)
        {
            InventoryItem currentItem = PlayerBag.itemList[SlotIndex];
            InventoryItem targeItem = PlayerBag.itemList[targetIndex];

            if (targeItem.itemID !=0)
            {
                
                PlayerBag.itemList[SlotIndex] = targeItem;
                PlayerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                PlayerBag.itemList[targetIndex] = currentItem;
                PlayerBag.itemList[SlotIndex] = new InventoryItem();
            }

            
            
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,PlayerBag.itemList);
        }
        
    }
    
    
}
