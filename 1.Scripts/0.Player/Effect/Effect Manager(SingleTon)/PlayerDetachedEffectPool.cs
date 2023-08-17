using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDetachedEffectPool : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public List<Pool> poolList;

    //정렬용
    List<GameObject> spawnedObjectList;

    [Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public Transform transform;
        public int initialSize;
    }

    #region SingleTon
    public static PlayerDetachedEffectPool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    private Vector2 createPosition;

    private void Start()
    {
        spawnedObjectList = new List<GameObject>();
        //풀 딕셔너리를 하나 만들고,
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //리스트를 순회하며 리스트 하나당
        foreach (Pool pool in poolList)
        {
            //큐 하나를 생성하고,
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            //최초 생성 개수만큼 객체를 만들어 큐에 인큐
            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                int index = obj.name.IndexOf("(Clone)");
                if (index > 0) obj.name = obj.name.Substring(0, index);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
                ArrangePool(obj);
            }
            //큐를 하나 만들 때마다, 딕셔너리에 이름과 큐를 저장.
            poolDictionary.Add(pool.name, objectQueue);
        }
    }

    //create with rotation
    public GameObject GetFromPool(string name, Quaternion rotation)
    {
        // 큐에 없으면 새로 추가
        Queue<GameObject> poolQueue = poolDictionary[name];
        if (poolQueue.Count <= 0)
        {
            Pool pool = poolList.Find(x => x.name == name);
            GameObject obj = Instantiate(pool.prefab, transform);
            int index = obj.name.IndexOf("(Clone)");
            if (index > 0) obj.name = obj.name.Substring(0, index);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
            ArrangePool(obj);
        }

        //리스트를 순회하며 이 name을 가진 Pool을 찾고, 그 Pool의 transform을 받아서 위치로 넘겨주기
        foreach (Pool pool in poolList)
        {
            if (pool.name.Equals(name))
            {
                createPosition = pool.transform.position;
            }
        }
        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.transform.position = createPosition;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public GameObject GetFromPool(string name, Vector3 position, Quaternion rotation)
    {
        // 큐에 없으면 새로 추가
        Queue<GameObject> poolQueue = poolDictionary[name];
        if (poolQueue.Count <= 0)
        {
            Pool pool = poolList.Find(x => x.name == name);
            GameObject obj = Instantiate(pool.prefab, transform);
            int index = obj.name.IndexOf("(Clone)");
            if (index > 0) obj.name = obj.name.Substring(0, index);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
            ArrangePool(obj);
        }

        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    private void ArrangePool(GameObject obj)
    {
        // 추가된 오브젝트 묶어서 정렬
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnedObjectList.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnedObjectList.Insert(i, obj);
                break;
            }
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        instance.poolDictionary[obj.name].Enqueue(obj);
    }
}
