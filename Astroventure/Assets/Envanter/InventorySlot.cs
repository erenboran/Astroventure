using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image itemIcon;
    public Text itemCountText;
    public int slotIndex;

    [SerializeField]
    GameObject visualPrefab;

    private void OnEnable()
    {
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (item.itemType != Enums.ItemTypes.Empty)
        {
            itemIcon.enabled = true;
            
            itemIcon.sprite = item.itemIcon;
            
            itemCountText.text = item.itemCount.ToString();
        }

        else
        {
            itemIcon.enabled = false;

            itemCountText.text = "";
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (item == null)
            return;
        // Sürükleme işlemi başladığında envanterin üzerine geçici bir görüntü oluşturun.
        visualPrefab.SetActive(false);
        InventoryManager.instance.CreateTempItem(item, eventData.position);
        // İlk slotun eşyasını taşınabilir hale getirin.
        //itemIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null)
            return;
        // Sürükleme işlemi sırasında geçici görüntüyü takip edin.
        InventoryManager.instance.UpdateTempItemPosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Sürükleme işlemi bittiğinde geçici görüntüyü yok edin.
        InventoryManager.instance.DestroyTempItem();
        // Tüm envanter slotlarındaki taşınabilirliği sıfırlayın.
        // foreach (InventorySlot slot in InventoryManager.instance.inventorySlots)
        // {
        //     slot.itemIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
        // }
        // Hedef slotu belirleyin.
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        if (targetObject != null)
        {
            InventorySlot targetSlot = targetObject.TryGetComponent<InventorySlot>(out targetSlot) ? targetSlot : null;

            if (targetSlot != null)
            {
                // Eğer hedef slot boşsa, eşyayı hareket ettirin.
                if (targetSlot.item == null)
                {
                    targetSlot.item = item;
                    item = null;
                }
                // Eğer aynı türdeki eşyaların sayısı item.maxStack'dan azsa, eşyaları birleştirin.
                else if (item.itemName == targetSlot.item.itemName && targetSlot.item.itemCount < item.maxStack)
                {
                    int spaceLeft = item.maxStack - targetSlot.item.itemCount;
                    int itemCountToMove = Mathf.Min(item.itemCount, spaceLeft);
                    targetSlot.item.itemCount += itemCountToMove;
                    item.itemCount -= itemCountToMove;
                    if (item.itemCount <= 0)
                    {
                        item = null;
                    }
                }
                // Aksi takdirde, eşyaları değiştirin.
                else
                {
                    Item temp = targetSlot.item;
                    targetSlot.item = item;
                    item = temp;
                }
                // Eşya hareketi yapıldıktan sonra envanteri güncelleyin.
                //  InventoryManager.instance.UpdateInventory();
                UpdateSlot();
                targetSlot.UpdateSlot();

            }




        }
        visualPrefab.SetActive(true);
    }
}
