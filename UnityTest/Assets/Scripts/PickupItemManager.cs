using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemManager : MonoBehaviour
{
    public delegate void PickupItemDelegate(Item item);

    public static PickupItemDelegate OnItemPickedUp;

    [SerializeField]
    private float maxPickupDistance = 10f;

    [SerializeField]
    private Material highlightMaterial;

    private InputManager inputManager;
    private Transform temporarySelectedItem;
    private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (temporarySelectedItem != null)
        {
            Renderer itemRenderer = temporarySelectedItem.GetComponent<Renderer>();
            itemRenderer.material = originalMaterial;

            temporarySelectedItem = null;
            originalMaterial = null;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxPickupDistance))
        {
            Item item = hit.transform.gameObject.GetComponentInChildren<Item>();
            if (item != null)
            {
                Renderer itemRenderer = hit.transform.GetComponent<Renderer>();
                if (itemRenderer != null )
                {
                    temporarySelectedItem = hit.transform;
                    originalMaterial = itemRenderer.material;

                    itemRenderer.material = highlightMaterial;
                }

                if (inputManager.PlayerClicked())
                {
                    OnItemPickedUp?.Invoke(item);
                }
            }
        }
        
    }
}
