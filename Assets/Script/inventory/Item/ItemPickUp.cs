using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mfram.Inventory
{
public class ItemPickUp : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D other)
   {
        Item item = other.GetComponent<Item>();
        if (item !=null)
        {
            if(item.itemDetails.canPickup)
            {
                InventoryManager.Instance.AddItem(item,true);
            }
        }
   }
}

}
