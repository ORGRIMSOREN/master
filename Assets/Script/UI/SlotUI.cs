using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Mfram.Inventory
{
    public class SlotUI : MonoBehaviour , IPointerClickHandler , IBeginDragHandler , IDragHandler , IEndDragHandler
    {
        [Header("組件獲取")]
        [SerializeField]
        private Image slotimage;

        [SerializeField]
        private TextMeshProUGUI amountText;

        public Image slotHightlight;

        [SerializeField]
        private Button button;

        [Header("格子類型")]
        public SlotType slotType;

        public  bool                 isSelected;
        public  ItemDetails          itemDetails;
        public  int                  itemAmount;
        public  int                  slotIndex;
        private IPointerClickHandler _pointerClickHandlerImplementation;
        private InventoryUI          inventoryUI => GetComponentInParent<InventoryUI>();

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
        public void UpdateSlot(ItemDetails item , int amount)
        {
            itemDetails         = item;
            slotimage.sprite    = item.itemIcon;
            itemAmount          = amount;
            slotimage.enabled   = true;
            amountText.text     = amount.ToString();
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

            slotimage.enabled   = false;
            amountText.text     = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount== 0) return;
            isSelected = !isSelected;
            
            

            inventoryUI.UpdateSlotHightlight(slotIndex);
            if (slotType==SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails , isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragitem.enabled       = true;
                inventoryUI.dragitem.sprite        = slotimage.sprite;
                inventoryUI.dragitem.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateSlotHightlight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragitem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragitem.enabled = false;
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotUI>()==null)return;
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<SlotUI>();
                
                if (targetSlot == null) return;
                var targetIndex = targetSlot.slotIndex;
        
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex , targetIndex);
                }
        
                inventoryUI.UpdateSlotHightlight(-1);
        
            }
        
        
            // else
            // {
            //     if (itemDetails.canDropped)
            //     {
            //
            //     var pos=Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            //     
            //     EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos );
            //
            //     }
            // }
        }
       
        

    }
}

    