using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Island Type", menuName = "Island Type", order = 0)]
public class IslandType : ScriptableObject
{
    public GameObject groundPrefab;
    public Color32 color;
   
}
