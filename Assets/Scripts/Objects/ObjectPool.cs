using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public PoolType poolType;

    public GameObject prefab;

    public int defaultSize = 20;

    [HideInInspector]
    public Queue<GameObject> objects = new();
}