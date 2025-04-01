using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Ins;
    public List<GameObject> pooledParticles;
    public List<EmissionData> emissionDatas;
    public GameObject objectToPool;
    public int poolCountOnStart;
    public float timeNoCollision;
    //public int amountToPool;

    void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        pooledParticles = new List<GameObject>();
        emissionDatas = new List<EmissionData>();
        GameObject tmp;
        for (int i = 0; i < poolCountOnStart; i++)
        {
            tmp = Instantiate(objectToPool);
            emissionDatas.Add(tmp.GetComponent<EmissionData>());
            tmp.SetActive(false);
            pooledParticles.Add(tmp);
        }
    }

    private void Update()
    {
        foreach (EmissionData obj in emissionDatas)
        {
            foreach(EmissionData obj2 in emissionDatas)
            {
                if (obj.timeEnabled > Time.time - timeNoCollision && obj.position == obj2.position) return;
                if (Vector2.Distance(obj.position, obj2.position) < obj.transform.localScale.x)
                {
                    obj.direction = Vector2.Reflect(obj.direction, obj2.position - obj.position);
                }
            }
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
        emissionDatas.Add(tmp.GetComponent<EmissionData>());
        return tmp;
    }

    public int ParticleCount()
    {
        return pooledParticles.Count;
    }
    public int ActiveParticleCount()
    {
        int count = 0;
        foreach(GameObject obj in pooledParticles)
        {
            if (obj.activeInHierarchy) count++;
        }
        return count;
    }
}
