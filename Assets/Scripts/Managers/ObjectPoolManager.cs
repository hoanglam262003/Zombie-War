using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [Header("Pools")]
    [SerializeField]
    private ObjectPool[] pools;

    private readonly Dictionary<PoolType, ObjectPool> poolLookup = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (ObjectPool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogWarning($"Pool {pool.poolType} has no prefab.");
                continue;
            }

            if (poolLookup.ContainsKey(pool.poolType))
            {
                Debug.LogWarning($"Duplicate PoolType: {pool.poolType}");
                continue;
            }

            poolLookup.Add(pool.poolType, pool);

            for (int i = 0; i < pool.defaultSize; i++)
            {
                CreateNewObject(pool);
            }
        }
    }

    private GameObject CreateNewObject(ObjectPool pool)
    {
        GameObject obj = Instantiate(pool.prefab);

        obj.SetActive(false);

        PoolObject poolObject = obj.GetComponent<PoolObject>();

        if (poolObject == null)
            poolObject = obj.AddComponent<PoolObject>();

        poolObject.PoolType = pool.poolType;

        pool.objects.Enqueue(obj);

        return obj;
    }

    public GameObject Get(PoolType poolType)
    {
        if (!poolLookup.TryGetValue(poolType, out ObjectPool pool))
        {
            Debug.LogError($"Pool not found: {poolType}");
            return null;
        }

        if (pool.objects.Count == 0)
        {
            CreateNewObject(pool);
        }

        GameObject obj = pool.objects.Dequeue();

        obj.SetActive(true);

        return obj;
    }

    public GameObject Get(
        PoolType poolType,
        Vector3 position,
        Quaternion rotation)
    {
        GameObject obj = Get(poolType);

        if (obj == null)
            return null;
        obj.SetActive(false);
        obj.transform.SetPositionAndRotation(
            position,
            rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null)
            return;

        PoolObject poolObject =
            obj.GetComponent<PoolObject>();

        if (poolObject == null)
        {
            Destroy(obj);
            return;
        }

        if (!poolLookup.TryGetValue(
            poolObject.PoolType,
            out ObjectPool pool))
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);

        pool.objects.Enqueue(obj);
    }

    public void Return(PoolObject poolObject)
    {
        Return(poolObject.gameObject);
    }
}