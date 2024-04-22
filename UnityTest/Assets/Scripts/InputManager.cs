using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    private InputMappings inputMappings;
    private bool isInventoryOpen = false;

    public static InputManager Instance {  get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }

        inputMappings = new InputMappings();
        Cursor.visible = false;
    }

    private void Start()
    {
        Inventory.OnInventoryToggle += HandleInventoryToggle; 
    }

    private void OnEnable()
    {
        inputMappings.Enable();
    }

    private void OnDisable()
    {
        inputMappings.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return isInventoryOpen ? Vector2.zero : inputMappings.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return isInventoryOpen ? Vector2.zero : inputMappings.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerToggledInventory()
    {
        return inputMappings.Player.Inventory.triggered;
    }

    public bool PlayerClicked()
    {
        return inputMappings.Player.LeftClick.triggered;
    }

    private void HandleInventoryToggle(bool isOpen)
    {
        isInventoryOpen = isOpen;
    }
}
