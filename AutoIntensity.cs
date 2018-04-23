using UnityEngine;
using System.Collections;

public class AutoIntensity : MonoBehaviour
{

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    float skySpeed = 1;


    private Light _sun;
    Skybox sky;

    void Start()
    {
        _sun = GetComponent<Light>();
    }

    void Update()
    {
        float progress = PlayerTime.dayProgress;

        _sun.transform.rotation = Quaternion.Euler(-90.0f + 360.0f * progress, 0.0f, 0.0f);

        float tRange = 1 - minPoint;

        float dot = Mathf.Clamp01((Vector3.Dot(_sun.transform.forward, Vector3.down) - minPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        _sun.intensity = i;

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(_sun.transform.forward, Vector3.down) - minAmbientPoint) / tRange);

        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        _sun.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = _sun.color;
        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

    }
}