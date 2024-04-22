using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    public ItemDataObject itemData;

    public string itemName;
    public Sprite icon;
    public GameObject prefab;

    private void Awake()
    {
        itemName = itemData.itemName;
        icon = itemData.icon;
        prefab = itemData.prefab;
    }
}
