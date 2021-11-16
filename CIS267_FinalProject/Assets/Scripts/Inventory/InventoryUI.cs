using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsContainer;
    public InventorySlot[] slots;
    public GameObject inventoryUI;
    public Hotbar hb;
    ItemSwitch itemSwitch;
    // Start is called before the first frame update
    void Start()
    {
        slots = slotsContainer.GetComponentsInChildren<InventorySlot>();
        itemSwitch = FindObjectOfType<ItemSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeInHierarchy);
            hb.ResetButtons();
            itemSwitch.inventoryItem = null;
            itemSwitch.hotbarItem = null;
        }
    }

    public void OnItemClick(int i)
    {
        if (i <= Inventory.instance.items.Count)
        {
            itemSwitch.inventoryIndex = i;
            itemSwitch.setInventoryItem(i);
        }
        else
        {
            itemSwitch.inventoryItem = null;
            itemSwitch.hotbarItem = null;
        }
    }

    public void updateUI()
    {
        for (int i = 0; i < Inventory.maxItems; i++)
        {
            if (i < Inventory.instance.items.Count)
            {
                slots[i].addItem(Inventory.instance.items[i]);
            }
            else
            {
                slots[i].clearSlot();
            }
        }
    }
}
