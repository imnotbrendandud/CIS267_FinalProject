using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    public new string name = "New Item";
    public Sprite sprite = null;
    public GameObject prefab;
}
