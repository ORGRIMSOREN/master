using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine.EventSystems;


namespace Mfram.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("組件獲取")] [SerializeField] private Image slotimage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHightlight;
        [SerializeField] private Button button;
         // 快捷欄槽位
        public InventoryLocation containerType;
        [Header("格子類型")] public SlotType slotType;
        public bool isSelected;
        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;
        private IPointerClickHandler _pointerClickHandlerImplementation;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotimage.sprite = item.itemIcon;
            itemAmount = amount;
            slotimage.enabled = true;
            amountText.text = amount.ToString();
            button.interactable = true;
        }

        /// <summary>
        /// 讲Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotimage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemDetails.itemID == 0)return;
            isSelected =!isSelected;
            slotHightlight.gameObject.SetActive(isSelected);
            
            inventoryUI.UpdateSlotHightlight(slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount!=0)
            {
                inventoryUI.dragitem.enabled = true;
                inventoryUI.dragitem.sprite = slotimage.sprite;
                inventoryUI.dragitem.SetNativeSize();
                
                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragitem.transform.position=Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragitem.enabled = false;
    
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                SlotUI targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                if (targetSlot == null)
                {
                    targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotUI>();
                }
        
                if (targetSlot != null && targetSlot != this)
                {
                    Debug.Log($"嘗試交換: 從 {containerType}[{slotIndex}] 到 {targetSlot.containerType}[{targetSlot.slotIndex}]");
            
                    // 同容器內交換
                    if (containerType == targetSlot.containerType)
                    {
                        InventoryManager.Instance.SwapItem(slotIndex, targetSlot.slotIndex, containerType);
                    }
                    // 跨容器交換
                    else
                    {
                        InventoryManager.Instance.SwapItemBetweenContainers(
                            containerType, slotIndex,
                            targetSlot.containerType, targetSlot.slotIndex);
                    }
                }
        
                isSelected = false;
                inventoryUI.UpdateSlotHightlight(-1);
            }
            else
            {
                // 丟棄物品的邏輯...
                if (itemDetails.canDropped)
                {
                    var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        -Camera.main.transform.position.z));
                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
                }

                // 重置選中狀態
                isSelected = false;
                inventoryUI.UpdateSlotHightlight(-1);
            }
        }
    }
}
    