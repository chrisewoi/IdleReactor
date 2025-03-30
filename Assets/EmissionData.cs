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

    private int r, g, b, target, value, valueMod;
    private float valueAsFloat;
    public int minSaturation;
    public float colorSpeed;

    public float cooloff;
    private float cooloffCurrent;

    void Start()
    {
        r = minSaturation; g = minSaturation; b = minSaturation;
        target = Random.Range(0, 2); // pick a rand r,g,b to start with
        SetRGB();
        valueMod = 1;
        valueAsFloat = 1f;
        cooloffCurrent = cooloff;
    }

    private void OnEnable()
    {
        r = minSaturation; g = minSaturation; b = minSaturation;
        target = Random.Range(0, 2); // pick a rand r,g,b to start with
        SetRGB();
        valueMod = 1;
        valueAsFloat = 1f;
        cooloffCurrent = cooloff;
    }

    void Update()
    {
        cooloffCurrent -= Time.deltaTime;
        if (cooloffCurrent < 0f)
        {
            Color trailColor = trail.startColor;
            trailColor.a = 0f;
            trail.startColor = trailColor;
            Destroy(trail.gameObject, cooloff);
            GameObject newTrail = Instantiate(trail.gameObject, gameObject.transform);
            
            trail.transform.SetParent(null);
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
}
