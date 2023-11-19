using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Item> itemList = new List<Item>();

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public GameObject tempItemPrefab;
    public GameObject inventoryGrid, inventoryPanel;

    private GameObject tempItem;
    private bool isDragging;

    [SerializeField]
    int slotCount;
    [SerializeField]
    GameObject inventorySlotPrefab;

    [SerializeField]
    Item testItem;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryGrid.transform);
            inventorySlots.Add(newSlot.GetComponent<InventorySlot>());
            //newSlot.transform.parent=transform;
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].slotIndex = i;
        }


    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item.itemType == Enums.ItemTypes.Empty)
            {

                int itemCountToMove = Mathf.Min(item.itemCount, item.maxStack);
                inventorySlots[i].item = item.ShallowCopy();
                inventorySlots[i].item.itemCount = itemCountToMove;
                item.itemCount -= itemCountToMove;
                inventorySlots[i].UpdateSlot();

                if (item.itemCount <= 0)
                {
                    item.itemCount = 0;
                    item.itemType = Enums.ItemTypes.Empty;
                }

                else
                {
                    AddItem(item);
                    break;
                }

                break;
            }


            else if (inventorySlots[i].item.itemName == item.itemName && inventorySlots[i].item.itemCount < item.maxStack)
            {

                int spaceLeft = item.maxStack - inventorySlots[i].item.itemCount;
                int itemCountToMove = Mathf.Min(item.itemCount, spaceLeft);
                inventorySlots[i].item.itemCount += itemCountToMove;
                item.itemCount -= itemCountToMove;
                inventorySlots[i].UpdateSlot();
                if (item.itemCount <= 0)
                {
                    item.itemCount = 0;
                    item.itemType = Enums.ItemTypes.Empty;
                }

                else
                {
                    AddItem(item);
                    break;
                }


                break;
            }

            else if (i == inventorySlots.Count - 1)
            {
                Debug.Log("Envanter dolu");
            }

        }
        //  UpdateInventory();
    }

    public void RemoveItem(Item item)
    {
        //items.Remove(item);
        // UpdateInventory();
    }

    public void UpdateInventory()
    {

    }

    public void CreateTempItem(Item item, Vector2 position)
    {
        tempItem = Instantiate(tempItemPrefab, position, Quaternion.identity, inventoryPanel.transform);
        InventorySlot tempSlot = tempItem.GetComponent<InventorySlot>();
        tempSlot.item = item;
        tempSlot.UpdateSlot();
        isDragging = true;
    }

    public void UpdateTempItemPosition(Vector2 position)
    {
        if (isDragging)
        {
            tempItem.transform.position = position;
        }
    }

    public void DestroyTempItem()
    {
        Destroy(tempItem);
        isDragging = false;
    }

    public bool IsDragging()
    {
        return isDragging;
    }

}



