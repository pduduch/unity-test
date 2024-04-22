using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    public GameObject mouseUI;
    public Image mouseCursor;
    public ItemSlotInfo mouseSlotInfo;
    public Image itemImage;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            mouseCursor.enabled = false;
            mouseUI.SetActive(false);
        }
        else
        {
            mouseCursor.enabled = true;

            if (mouseSlotInfo.itemData != null)
            {
                mouseUI.SetActive(true);
            }
            else
            {
                mouseUI.SetActive(false);
            }
        }
    }

    public void SetUI()
    {
        itemImage.sprite = mouseSlotInfo.itemData.icon;
    }

    public void EmptySlot()
    {
        mouseSlotInfo = new ItemSlotInfo(null);
    }
}
