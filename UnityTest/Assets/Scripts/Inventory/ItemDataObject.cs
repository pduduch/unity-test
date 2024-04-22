using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
public class ItemDataObject : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
}
