using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ItemToolTip : MonoBehaviour
{
    //定義變量
    [SerializeField]private TextMeshProUGUI nametext;
    [SerializeField]private TextMeshProUGUI typetext;
    [SerializeField]private TextMeshProUGUI descriptiontext;
    [SerializeField]private Text costtext;

    [SerializeField]
    private GameObject buttom;

    public void SetUpItemToolTip(ItemDetails itemDetails,SlotType slotType)
    {
        nametext.text = itemDetails.itemName;
        typetext.text = GetItemType(itemDetails.itemType);
        descriptiontext.text = itemDetails.itemDescription;

        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Furniture || itemDetails.itemType==ItemType.Commodity)
        {
            buttom.SetActive(true);
            var price = itemDetails.itemPrice;
            if (slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            costtext.text = price.ToString();
            }
            else
            {
                buttom.SetActive(false);    
            }
        }
        
    }

    private string GetItemType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Seed :      return "種子";
            case ItemType.Commodity : return "商品";
            case ItemType.Furniture : return "家具";
            case ItemType.BreakTool :
            case ItemType.ChopTool :
            case ItemType.CollectTool :
            case ItemType.HoeTool :
            case ItemType.ReapTool :
            case ItemType.WaterTool : return "工具";
            default : return "無";
        }
    }
}
