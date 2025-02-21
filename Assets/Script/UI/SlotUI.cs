using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mfram.Inventory{
public class SlotUI : MonoBehaviour
{
    [Header("組件獲取")]
    [SerializeField]private Image slotimage;
    [SerializeField]private TextMeshProUGUI amountText;
    [SerializeField]private Image slotHightlight;
    [SerializeField]private Button button;
    [Header("格子類型")]
    public SlotType slotType;
    public bool isSelected;
    public ItemDetails itemDetails;
    public int itemAmount;
    public int slotIndex;

    private void Start() 
    {
        isSelected=false;
        if(itemDetails.itemID==0)
        {
            UpdateEmptySlot();
        }    
    }
    /// <summary>
    /// 更新格子UI和信息
    /// </summary>
    /// <param name="item">ItemDetails</param>
    /// <param name="amount">持有数量</param>
    public void UpdateSlot(ItemDetails item,int amount)
    {
        itemDetails=item;
        slotimage.sprite=item.itemIcon;
        itemAmount=amount;
        slotimage.enabled=true;
        amountText.text=amount.ToString();
        button.interactable=true;
    }
    /// <summary>
    /// 讲Slot更新为空
    /// </summary>
    public void UpdateEmptySlot()
{
    if(isSelected)
    {
        isSelected=false;
    }
    slotimage.enabled=false;
    amountText.text=string.Empty;
    button.interactable=false;
}
}
}