using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    [SerializeField]
    Item newitem;
    
    void Start()
    {
        //newitem= ScriptableObject.CreateInstance<Item>();
        //AssetDatabase.CreateAsset(newitem, "Assets/NewScriptableObject.asset");
    }
 
}
