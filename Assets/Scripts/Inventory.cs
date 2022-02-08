using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    public GameObject inventoryUISlotsParent;
    public GameObject hotbarUISlotsParent;

    private ShipController shipController;

    private List<Item> Items;
    // Start is called before the first frame update
    void Start()
    {
        shipController = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggelInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            inventoryUISlotsParent.SetActive(!inventoryUISlotsParent.activeSelf);
            if (shipController.Mode == ShipController.ShipMode.Build)
            {
                hotbarUISlotsParent.SetActive(true);
            }
            else if (shipController.Mode == ShipController.ShipMode.Flight)
            {
                hotbarUISlotsParent.SetActive(inventoryUISlotsParent.activeSelf);
            }
            else hotbarUISlotsParent.SetActive(!hotbarUISlotsParent.activeSelf);
        }
    }

    public void EnableHotbar()
    {
        hotbarUISlotsParent.SetActive(true);
    }

    internal void DisableHotbar()
    {
        hotbarUISlotsParent.SetActive(false);
    }
}