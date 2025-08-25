using UnityEngine;

public abstract class Item_SO : ScriptableObject
{
    public GameObject PreFab;
    public string Name;
    public string Description;

    public abstract void OnUse(GameObject user);
    
}
