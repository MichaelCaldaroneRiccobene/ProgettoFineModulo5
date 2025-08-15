using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Item_SO item;

    public Item_SO Item_SO { get => item; }
    private void Start() => Instantiate(item.preFab, transform.position, transform.rotation, transform);
}
