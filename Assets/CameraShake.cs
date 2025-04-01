using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraShake : MonoBehaviour
{
    int particleCount => ParticlePool.Ins.ActiveParticleCount();

    public float shakeAmount;
    [Range(0f,1f)] public float shakeFrequency;
    [Range(0f,1f)] public float shakeHeat;
    [Range(0f,1f)] public float shakeHeatTarget;
    [Tooltip("How many seconds it will take for shake amount to match the target amount")] public float shakeChangeSpeed;
    public float shakeTime; // time since current shake began
    public int particleMax;
    public float v;

    private Camera cam;
    public Light2D sceneLight;
    public float lightIntensityMax;

    private Vector2 targetPos;
    private float targetRot;
    private Vector2 camOrigin;

    public AnimationCurve curve;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        camOrigin = cam.transform.position;

        InvokeRepeating("SetShakePos", 0f, shakeFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeat();
        ScreenShake();
        SetLightIntensity();
    }

    void UpdateHeat()
    {
        shakeHeatTarget = (float)particleCount / (float)particleMax;


        shakeHeatTarget = Mathf.Clamp01(shakeHeatTarget);
        shakeFrequency = Mathf.Clamp01(shakeFrequency);
        shakeHeat = Mathf.Clamp01(shakeHeat);

        shakeHeat = Mathf.SmoothDamp(shakeHeat, shakeHeatTarget, ref v, shakeChangeSpeed);

    }

    void ScreenShake()
    {
        if (targetPos != Vector2.zero)
        {
            cam.transform.position = Vector2.LerpUnclamped(camOrigin, targetPos, curve.Evaluate(shakeTime));
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10f);
        } 
        else
        {
            shakeTime = 0f;
        }

        shakeTime += Time.deltaTime / shakeFrequency;
    }
    void SetShakePos()
    {
        targetPos = Random.insideUnitCircle * shakeAmount * shakeHeat;
    }

    void SetLightIntensity()
    {
        sceneLight.intensity = shakeHeat * lightIntensityMax;
    }
}
