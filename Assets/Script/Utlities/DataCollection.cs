using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemonWorld;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickup;              
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0,1)]
    public float sellPercentage;

    


}
    
[System.Serializable]
public struct InventoryItem
 {
     public int itemID;
    public int itemAmount;
    
}




