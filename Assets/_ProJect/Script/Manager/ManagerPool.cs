using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ManagerPool;

public class ManagerPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolObj
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ManagerPool Instace;

    [SerializeField] private List<PoolObj> poolsList;

    private Dictionary<string, Queue<GameObject>> poolDictionaryObj;

    private void Awake() => Instace = this;

    private void Start()
    {
        poolDictionaryObj = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolObj pool in poolsList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionaryObj.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetGameObjFromPool(string tag)
    {
        foreach (GameObject obj in poolDictionaryObj[tag])
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        } return SpawnForPool(tag);
    }

    public GameObject GetAudioFromPool(string tag)
    {
        GameObject obj = poolDictionaryObj[tag].Dequeue();
        obj.SetActive(true);
        poolDictionaryObj[tag].Enqueue(obj);

        return obj;
    }

    private GameObject SpawnForPool(string tag)
    {
        PoolObj poolList = poolsList.Find(x => x.tag == tag);
        GameObject objToSpawn = Instantiate(poolList.prefab, transform);
        poolDictionaryObj[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
