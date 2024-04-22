using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public delegate void InventoryToggleDelegate(bool isOpen);

    public static InventoryToggleDelegate OnInventoryToggle;

    [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

    [Header("Inventory Menu Components")]
    public GameObject inventoryMenu;
    public GameObject itemPanel;
    public GameObject itemPanelGrid;
    public int inventorySize = 24;

    public MouseUI mouse;

    private InputManager inputManager;
    private List<ItemPanel> panels = new List<ItemPanel>();

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inventoryMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        PickupItemManager.OnItemPickedUp += HandleOnItemPickedUp;

        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlotInfo(null));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.PlayerToggledInventory())
        {
            if (inventoryMenu.activeSelf)
            {
                inventoryMenu.SetActive(false);
                mouse.EmptySlot();
                Cursor.lockState = CursorLockMode.Locked;

                OnInventoryToggle?.Invoke(false);
            }
            else
            {
                OnInventoryToggle?.Invoke(true);

                inventoryMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                RefreshInventory();
            }
        }

        if (inputManager.PlayerClicked() && mouse.mouseSlotInfo.itemData != null && !EventSystem.current.IsPointerOverGameObject())
        {
            DropItem(mouse.mouseSlotInfo.itemData);
        }
    }

    public void RefreshInventory()
    {
        panels = itemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

        if (panels.Count < inventorySize)
        {
            int panelsLeftToCreate = inventorySize - panels.Count;
            for (int i = 0; i < panelsLeftToCreate; i++)
            {
                GameObject newPanel = Instantiate(itemPanel, itemPanelGrid.transform);
                panels.Add(newPanel.GetComponent<ItemPanel>());
            }
        }

        int index = 0;
        foreach (ItemSlotInfo slot in items)
        {
            ItemPanel currentPanel = panels[index];
            if (currentPanel != null)
            {
                currentPanel.inventory = this;
                currentPanel.itemSlot = slot;
                if (slot.itemData != null)
                {
                    currentPanel.itemImage.gameObject.SetActive(true);
                    currentPanel.itemImage.sprite = slot.itemData.icon;
                    currentPanel.itemImage.CrossFadeAlpha(1, 0.0f, true);
                }
                else
                {
                    currentPanel.itemImage.gameObject.SetActive(false);
                }
            }

            index++;
        }

        mouse.EmptySlot();
    }

    public void AddItem(Item newItem)
    {
        foreach (ItemSlotInfo slot in items)
        {
            if (slot.itemData == null)
            {
                slot.itemData = newItem.itemData;
                if (inventoryMenu.activeSelf)
                {
                    RefreshInventory();
                }
                return;
            }
        }

        if (inventoryMenu.activeSelf)
        {
            RefreshInventory();
        }
    }

    public void ClearSlot(ItemSlotInfo slot)
    {
        slot.itemData = null;
    }

    private void HandleOnItemPickedUp(Item item)
    {
        AddItem(item);
        Destroy(item.gameObject);
    }

    public void DropItem(ItemDataObject droppedItemData)
    {
        Transform cameraTransform = Camera.main.transform;

        GameObject item = Instantiate(droppedItemData.prefab, cameraTransform.position + new Vector3(0, 1.0f, 0) + cameraTransform.forward, Quaternion.Euler(Vector3.zero));
        
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null )
        {
            rb.velocity = cameraTransform.forward * 3;
        }

        Item i = item.GetComponentInChildren<Item>();
        if (i != null)
        {
            i.itemData = droppedItemData;
            i.itemName = droppedItemData.name;
            i.icon = droppedItemData.icon;
            i.prefab = droppedItemData.prefab;
        }

        ClearSlot(mouse.mouseSlotInfo);
        mouse.EmptySlot();
        RefreshInventory();
    }
}
