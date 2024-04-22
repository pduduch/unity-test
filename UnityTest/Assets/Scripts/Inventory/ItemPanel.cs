using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler, IDropHandler, IDragHandler
{
    public Inventory inventory;
    public ItemSlotInfo itemSlot;
    public Image itemImage;

    private MouseUI mouse;
    private bool hasClicked;

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerPress = this.gameObject;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hasClicked)
        {
            OnClick();
            hasClicked = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (hasClicked)
        {
            OnClick();
            hasClicked = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnClick();
        hasClicked = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hasClicked = true;
    }

    public void SelectItem()
    {
        mouse.mouseSlotInfo = itemSlot;
        mouse.SetUI();
    }

    public void FadePanel()
    {
        itemImage.CrossFadeAlpha(0.3f, 0.05f, true);
    }

    public void OnClick()
    {
        if (inventory != null)
        {
            mouse = inventory.mouse;

            if (mouse.mouseSlotInfo.itemData == null)
            {
                if (itemSlot.itemData != null)
                {
                    SelectItem();
                    FadePanel();
                }

            }
            else
            {
                if (itemSlot == mouse.mouseSlotInfo)
                {
                    inventory.RefreshInventory();
                }
            }
        }
    }
}
