using System.Collections.Generic;
using UnityEngine;
public class ManagerPool : MonoBehaviour
{
    public static ManagerPool Instace {  get; private set; }

    [Header("Setting")]
    [SerializeField] private List<PoolObj_SO> poolObjs;

    private Dictionary<string, Queue<GameObject>> poolDictionaryObj;

    private void Awake()
    {
        if (Instace != null && Instace != this) { Destroy(gameObject); return; }
        else Instace = this;
    }

    private void Start() => GeneratePool();

    private void GeneratePool()
    {
        poolDictionaryObj = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolObj_SO pool in poolObjs)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            if(pool == null) continue;
            for (int i = 0; i < pool.StartPool;i++)
            {
                GameObject obj = Instantiate(pool.PreFab,transform);
                objectPool.Enqueue(obj);
                obj.gameObject.SetActive(false);
            }
            poolDictionaryObj.Add(pool.ID, objectPool);
        }
    }

    public GameObject GetGameObjFromPool(string tag)
    {
        if (!poolDictionaryObj.ContainsKey(tag)) return null;

        if (poolDictionaryObj[tag].Count > 0)
        {
            GameObject objToSpawn = poolDictionaryObj[tag].Dequeue();
            objToSpawn.SetActive(true);
            return objToSpawn;
        }
        else return SpawnForPool(tag);
    }

    public void ReturnToPool(string tag,GameObject obj)
    {
        if (!poolDictionaryObj.ContainsKey(tag)) return;

        obj.gameObject.SetActive(false);
        poolDictionaryObj[tag].Enqueue(obj);
    }

    private GameObject SpawnForPool(string tag)
    {
        PoolObj_SO poolList = poolObjs.Find(x => x.ID == tag);
        GameObject objToSpawn = Instantiate(poolList.PreFab, transform);

        return objToSpawn;
    }
}
