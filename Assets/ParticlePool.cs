using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Ins;
    public List<GameObject> pooledParticles;
    public GameObject objectToPool;
    //public int amountToPool;

    void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        pooledParticles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < pooledParticles.Count; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledParticles.Add(tmp);
        }
    }

    public GameObject GetPooledParticle()
    {
        for (int i = 0; i < pooledParticles.Count; i++)
        {
            if (!pooledParticles[i].activeInHierarchy)
            {
                return pooledParticles[i];
            }
        }
        //if pool all active, add new to pool and return that
        GameObject tmp = Instantiate(objectToPool);
        pooledParticles.Add(tmp);
        return tmp;
    }
}
