using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EmissionData : MonoBehaviour
{
    public float speed;
    public Color color;
    public TrailRenderer trail, trail2;
    public SpriteRenderer spriteRenderer;

    public Vector2 direction;
    public Vector2 position => transform.position;
    public float timeEnabled;

    private int r, g, b, target, value, valueMod;
    private float valueAsFloat;
    public int minSaturation;
    public float colorSpeed;

    public float cooloff;
    private float cooloffCurrent;

    public float emissionSize;

    private void Awake()
    {
        emissionSize = transform.localScale.x;
    }
    void Start()
    {
        r = minSaturation; g = minSaturation; b = minSaturation;
        target = Random.Range(0, 2); // pick a rand r,g,b to start with
        SetRGB();
        valueMod = 1;
        valueAsFloat = 1f;
        cooloffCurrent = cooloff + Random.Range(-cooloff, cooloff)/10f;
    }

    private void OnEnable()
    {
        r = minSaturation; g = minSaturation; b = minSaturation;
        target = Random.Range(0, 2); // pick a rand r,g,b to start with
        SetRGB();
        valueMod = 1;
        valueAsFloat = 1f;
        cooloffCurrent = cooloff + Random.Range(-cooloff, cooloff)/10f;

        trail.startColor = new Color(trail.startColor.r, trail.startColor.g, trail.startColor.b, 1f);
        timeEnabled = Time.time;
    }

    void Update()
    {
        cooloffCurrent -= Time.deltaTime;
        
        trail.time = cooloffCurrent + cooloff/3f;
            
        if (cooloffCurrent < 0f)
        {
            GameObject newTrail = Instantiate(trail.gameObject, gameObject.transform);
            newTrail.name = "Trail";
            Color trailColor = trail.startColor;
            trailColor.a = 0.1f;
            trail.startColor = trailColor;
            
            trail.transform.SetParent(null);
            Destroy(trail.gameObject, cooloff);
            
            trail = newTrail.GetComponent<TrailRenderer>();
            gameObject.SetActive(false);
            return;
        }
        
        valueAsFloat += colorSpeed * valueMod * Time.deltaTime;
        value = (int)(valueAsFloat * 255f); //value = Mathf.RoundToInt(valueAsFloat * 255f);
        transform.position += (Vector3)direction * (speed * Time.deltaTime);
        ColorUpdater();
        spriteRenderer.color = color;
        Color tmp = color;
        tmp = new Color(ColorX(r), ColorX(g), ColorX(b), 100f / 255f);
        trail.startColor = tmp;
    }

    public void ColorUpdater()
    {
        
        switch (valueMod)
        {
            case 1:
                switch (target)
                {
                    case 0:
                        r = UpdateSaturation();
                        if (r >= 255) TargetReached();
                        break;
                    case 1:
                        g = UpdateSaturation();
                        if (g >= 255) TargetReached();
                        break;
                    case 2:
                        b = UpdateSaturation();
                        if (b >= 255) TargetReached();
                        break;
                }
                break;
            case -1:
                switch (target)
                {
                    case 0:
                        r = UpdateSaturation();
                        if (r <= minSaturation) TargetReached();
                        break;
                    case 1:
                        g = UpdateSaturation();
                        if (g <= minSaturation) TargetReached();
                        break;
                    case 2:
                        b = UpdateSaturation();
                        if (b <= minSaturation) TargetReached();
                        break;
                }
                break;
        }
        // update color here
        // i did it already
    }

    void TargetReached()
    {
        // flip the direction of change, move 
        valueMod *= -1;
        target = (target + 1) % 3;
    }

    int UpdateSaturation()
    {
        return (int)(valueAsFloat * 255); //Mathf.RoundToInt(valueAsFloat * 255);
    }

    void SetRGB()
    {
        switch (target)
        {
            case 0:
                r = 255;
                g = 255;
                b = minSaturation;
                break;
            case 1:
                r = minSaturation;
                g = 255;
                b = 255;
                break;
            case 2:
                r = 255;
                g = minSaturation;
                b = 255;
                break;
        }
    }

    float ColorX(int colorValue)
    {
        return colorValue / 255f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bounds"))
        {
            direction = Vector2.Reflect(direction, other.contacts[0].normal);
        }
    }
}
