using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_SO : ScriptableObject
{
    public GameObject preFab;
    public string Name;
    public string Description;

    public abstract void OnUse(GameObject user);
    
}
