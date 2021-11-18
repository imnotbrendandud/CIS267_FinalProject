using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public static Hotbar instance;
    public static int maxItems = 3;
    public List<Item> items = new List<Item>(maxItems);
    [SerializeField] private HotbarUI hotbarUI;
    [SerializeField] private List<Button> slotButtons;

    public delegate void OnChange();
    public OnChange onChangeCallback;
    public ItemSwitch itemSwitch;
    public int i;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        itemSwitch = GameObject.Find("GameManager").GetComponent<ItemSwitch>();
    }

    private void Update()
    {
        if (GameObject.Find("Inventory")) return;
        WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Player player = FindObjectOfType<Player>();
            i = 0;
            if (InBounds(i))
            {
                player.itemInHolster = 0;
                player.UseItem(this.items[i]);
                weaponHolster.SelectedItemIcon.transform.position = new Vector3(-1.05f, -3.75f, 0f);
                ResetButtons(true);
                HighlightButton(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Player player = FindObjectOfType<Player>();
            i = 1;
            if (InBounds(i))
            {
                player.itemInHolster = 1;
                player.UseItem(this.items[i]);
                weaponHolster.SelectedItemIcon.transform.position = new Vector3(0f, -3.75f, 0f);
                ResetButtons(true);
                HighlightButton(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Player player = FindObjectOfType<Player>();
            i = 2;
            if (InBounds(i))
            {
                player.itemInHolster = 2;
                player.UseItem(this.items[i]);
                weaponHolster.SelectedItemIcon.transform.position = new Vector3(1.075f, -3.75f, 0f);
                ResetButtons(true);
                HighlightButton(i);
            }
        }
    }

    public void ResetButtons(bool setActive)
    {
        foreach (Button button in slotButtons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = new Color32(255, 255, 255, 0);
            colors.highlightedColor = new Color32(255, 255, 255, 0);
            button.colors = colors;
            itemSwitch.setHotbarNumbers(false);
            WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
            weaponHolster.SelectedItemIcon.SetActive(setActive);
        }
    }

    public void HighlightButton(int i)
    {
        ColorBlock colors = slotButtons[i].colors;
        colors.normalColor = new Color32(255, 255, 255, 40);
        colors.highlightedColor = new Color32(255, 255, 255, 40);
        slotButtons[i].colors = colors;
        WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
        weaponHolster.SelectedItemIcon.SetActive(true);
    }

    public bool InBounds(int i)
    {
        return i >= 0 && i < items.Capacity;//FIX THIS
    }

    public bool IsFull()
    {
        return items.Count >= items.Capacity;
    }

    public bool Add(Item item)
    {
        if (IsFull()) return false;
        items.Add(item);
        hotbarUI.updateUI();
        if (onChangeCallback != null) onChangeCallback();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        hotbarUI.updateUI();
        if (onChangeCallback != null) onChangeCallback();
    }

    public void RemoveIndex(int i)
    {
        items.RemoveAt(i);
        hotbarUI.updateUI();
        if (onChangeCallback != null) onChangeCallback();
    }

    public void DropItem(int i)
    {
        float drag = 4.5f;
        float force = 100f;
        float itemDropOffset = 1.5f;
        Player player = FindObjectOfType<Player>();
        float horizontal = player.animator.GetFloat("lastMoveHorizontal");
        float vertical = player.animator.GetFloat("lastMoveVertical");

        Vector3 offset = new Vector3(horizontal, vertical, 0) * itemDropOffset;
        offset.z = player.transform.position.z;
        Vector2 push = new Vector2(horizontal, vertical) * force;

        GameObject instance = Instantiate(this.items[i].prefab, player.transform.position + offset, transform.rotation);
        instance.GetComponent<Rigidbody2D>().AddForce(push);

        instance.GetComponent<Rigidbody2D>().drag = drag;
        this.RemoveIndex(i);
        FindObjectOfType<WeaponHolster>().ResetWeapon();
        ResetButtons(false);
    }
}
