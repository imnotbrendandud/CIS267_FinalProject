using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    bool firstItem = true;
    public WeaponHolster wp;
    public Player p;
    public Hotbar hb;

    private void Start()
    {
        wp = FindObjectOfType<WeaponHolster>();
        hb = FindObjectOfType<Hotbar>();
        p = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.gameObject.GetComponent<ItemObject>().item;
            if (Hotbar.instance.items.Count == 0)
            {
                firstItem = true;
            }
            else
            {
                firstItem = false;
            }
            if (Hotbar.instance.Add(item))
            {
                Destroy(collision.gameObject);
                if (firstItem)
                {
                    Hotbar.instance.HighlightButton(0);
                    hb.iw = 0;
                    p.UseItem(item);
                    p.itemInHolster = 0;
                    wp.SelectedItemIcon.transform.position = new Vector3(-1.05f, -3.75f, 0f);
                }
            }
            else if (Inventory.instance.Add(item))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
