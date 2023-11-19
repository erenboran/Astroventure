using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject 
{
  public string itemName;
  public Sprite itemIcon;
  public int maxStack;
  public int itemCount;

  public Enums.ItemTypes itemType;

  public Item ShallowCopy()
  {
    return (Item)this.MemberwiseClone();
  }
}

