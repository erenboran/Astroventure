using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHelper : MonoBehaviour
{
    [SerializeField]
    Dropdown dropdownMenu;
    [SerializeField] InputField inputField;
    int itemCount=1;
    InventoryManager InventoryManager;

    private void Start()
    {
        InventoryManager = InventoryManager.instance; ;
        foreach (ScriptableObject obj in InventoryManager.itemList)
        {
            dropdownMenu.options.Add(new Dropdown.OptionData(obj.name));
        }

    }

    public void InputFieldChanged()
    {
        itemCount = int.Parse(inputField.text);
    }


    public void AddItem()
    {
        Item newItem = InventoryManager.itemList[dropdownMenu.value-1].ShallowCopy();
        newItem.itemCount = itemCount;
        InventoryManager.AddItem(newItem);
    }


}
