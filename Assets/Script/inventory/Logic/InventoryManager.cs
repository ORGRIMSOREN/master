using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Mfram.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [SerializeField] public ItemDataList_SO itemDataList_SO;
        [SerializeField] public InventoryBag_SO PlayerBag;
        [SerializeField] public InventoryBag_SO HotbarBag;

        private void Start()
        {

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
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
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
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

        public void SwapItem(int fromIndex, int targetIndex,InventoryLocation location)
        {
            InventoryItem currentItem = new InventoryItem
            {
                itemID = PlayerBag.itemList[fromIndex].itemID,
                itemAmount = PlayerBag.itemList[fromIndex].itemAmount
            };

            InventoryItem targetItem = new InventoryItem
            {
                itemID = PlayerBag.itemList[targetIndex].itemID,
                itemAmount = PlayerBag.itemList[targetIndex].itemAmount
            };

            if (targetItem.itemID != 0)
            {
                PlayerBag.itemList[fromIndex] = targetItem;
                PlayerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                PlayerBag.itemList[targetIndex] = currentItem;
                PlayerBag.itemList[fromIndex] = new InventoryItem();
            }

            // 確保 UI 更新
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, PlayerBag.itemList);
        }

        public void SwapItemBetweenContainers(
            InventoryLocation fromLocation, int fromIndex,
            InventoryLocation toLocation, int toIndex)
        {
            List<InventoryItem> fromList = GetItemList(fromLocation);
            List<InventoryItem> toList = GetItemList(toLocation);

            if (fromList != null && toList != null)
            {
                InventoryItem fromItem = new InventoryItem
                {
                    itemID = fromList[fromIndex].itemID,
                    itemAmount = fromList[fromIndex].itemAmount
                };

                InventoryItem toItem = new InventoryItem
                {
                    itemID = toList[toIndex].itemID,
                    itemAmount = toList[toIndex].itemAmount
                };

                fromList[fromIndex] = toItem;
                toList[toIndex] = fromItem;

                EventHandler.CallUpdateInventoryUI(fromLocation, fromList);
                EventHandler.CallUpdateInventoryUI(toLocation, toList);
            }
        }

        private List<InventoryItem> GetItemList(InventoryLocation location)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    return PlayerBag.itemList;
                case InventoryLocation.Hotbar:
                    return HotbarBag.itemList;
                default:
                    return null;
            }

        }

    }
}
