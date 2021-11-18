using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSwitch : MonoBehaviour
{
    public Hotbar hotbar;
    public HotbarUI hotbarUI;
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public GameObject HotbarNumbers;
    public Button SelectedInventoryButton;
    InventorySlot[] slots;
    InventorySlot slot;
    Button slotButton;
    public ColorBlock colors;
    public Player p;

    private int hotbarIndex = -1;
    private int inventoryIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        p = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Inventory"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                setHotBarItem(0);
                SwitchItems();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                setHotBarItem(1);
                SwitchItems();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                setHotBarItem(2);
                SwitchItems();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {//mouse was clicked over inventory or hotbar element
                SetButtonToSelectedColor();
            }
            else
            {//mouse was not clicked on any buttons, therefore clear selections
                ResetItems();
                HotbarNumbers.SetActive(false);
                inventoryUI.ResetButtonColor();
            }
        }
    }

    public void setHotbarNumbers(bool visible)
    {
        HotbarNumbers.SetActive(visible);
    }

    public int getInventoryIndex()
    {
        return inventoryIndex;
    }

    public int getHotbarIndex()
    {
        return hotbarIndex;
    }

    public void ResetItems()
    {
        hotbarIndex = -1;
        inventoryIndex = -1;
    }

    public void ResetHotbarItem()
    {
        hotbarIndex = -1;
    }

    public void setHotBarItem(int i)
    {
        if (Hotbar.instance.InBounds(i))
        {
            hotbarIndex = i;
        }
    }


    public void setInventoryItem(int i)
    {
        if (Inventory.instance.InBounds(i))
        {
            inventoryIndex = i;
            inventoryUI.ResetButtonColor();
            SetButtonToSelectedColor();
        }
    }
    public void SetButtonToSelectedColor()
    {
        if (Inventory.instance.InBounds(inventoryIndex))
        {
            inventoryUI.ResetButtonColor();
            slots = inventoryUI.getSlots();
            slot = slots[inventoryIndex];
            slotButton = slot.GetButton();
            colors.normalColor = new Color32(0, 255, 83, 40);
            colors.highlightedColor = new Color32(0, 255, 83, 40);
            colors.pressedColor = new Color32(0, 0, 0, 40);
            colors.selectedColor = new Color32(0, 255, 83, 40);
            colors.disabledColor = new Color32(0, 255, 83, 40);
            colors.colorMultiplier = 1;
            colors.fadeDuration = .1f;
            slotButton.colors = colors;
        }
    }

    public void SwitchItems()
    {
        if (Hotbar.instance.InBounds(hotbarIndex) && Inventory.instance.InBounds(inventoryIndex))
        {
            Item tempHotBarItem = Hotbar.instance.items[hotbarIndex];
            Item tempInventoryItem = Inventory.instance.items[inventoryIndex];
            //copy inventory item and hotbar item
            Inventory.instance.items[inventoryIndex] = tempHotBarItem;
            Hotbar.instance.items[hotbarIndex] = tempInventoryItem;

            // replace inventory and hotbar item in lists
            inventoryUI.updateUI();
            hotbarUI.updateUI();

            //change item in holster if you swapped that item
            if (hotbarIndex == p.itemInHolster)
            {
                p.UseItem(hotbar.items[hotbar.i]);
            }
        }
    }
}
