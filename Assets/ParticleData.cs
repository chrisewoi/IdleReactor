using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleData : MonoBehaviour
{
    public float power;
    public float emissionMult;
    private float emissionTime;
    public float heatUncapped;
    public float coolingRate;
    [Range(0f,1f)]public float heat;
    public Color cold, warm, hot, current;

    private SpriteRenderer spriteRenderer;
    public GameObject particleEmission;
    
    
    
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        heat = heatUncapped;
        UpdateColor();
        EmissionCheck();

        heatUncapped -= coolingRate * Time.deltaTime;
    }

    void UpdateColor()
    {
        if (heat < 0.5f) current = Color.Lerp(cold, warm, heat * 2f);
        else current = Color.Lerp(warm, hot, (heat - 0.5f) * 2f);
        
        spriteRenderer.color = current;
    }

    void EmissionCheck()
    {
        emissionTime += Time.deltaTime;

        if (heatUncapped <= 0.01f) heatUncapped = 0.01f;
        if (emissionTime >= 1/emissionMult * 1/heatUncapped)
        {
            emissionTime = 0f;
            
            //Emit particle
            particleEmission = ParticlePool.Ins.GetPooledParticle();
            particleEmission.SetActive(true);
            particleEmission.transform.position = transform.position;
            EmissionData emissionData = particleEmission.GetComponent<EmissionData>();
            emissionData.direction = Random.insideUnitCircle.normalized; // Random direction
            emissionData.speed = 1f;
        }
    }
}
