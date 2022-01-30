using System.Collections;
using Kino;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AnalogGlitch), typeof(DigitalGlitch))]
public class CameraEffects : MonoBehaviour
{
    private AnalogGlitch analog;
    private DigitalGlitch digital;

    private bool shaking;
    private float shakeSpeed;

    private void OnEnable()
    {
        analog = GetComponent<AnalogGlitch>();
        digital = GetComponent<DigitalGlitch>();
    }

    private void Update()
    {
        // Random glitches
        analog.colorDrift = Mathf.Lerp(analog.colorDrift, Random.value > 0.99f ? Random.value * 0.3f + 0.2f : 0f, Time.deltaTime * 10f);
        
        // Shake effect
        if (shaking)
        {
            analog.horizontalShake += Time.deltaTime * shakeSpeed;
            analog.scanLineJitter += Time.deltaTime * shakeSpeed;
            return;
        }
        
        if (analog.horizontalShake > 0)
        {
            analog.horizontalShake = Mathf.Lerp(analog.horizontalShake, 0f, Time.deltaTime * shakeSpeed * 100f);
        }
        if (analog.scanLineJitter > 0)
        {
            analog.scanLineJitter = Mathf.Lerp(analog.scanLineJitter, 0f, Time.deltaTime * shakeSpeed * 100f);
        }
    }

    // 0.0 - 1.0
    public void Shake(float intensity)
    {
        if (shaking) return;

        StartCoroutine(Shaker(Mathf.Clamp01(intensity)));
    }

    private IEnumerator Shaker(float intensity)
    {
        shaking = true;
        shakeSpeed = intensity;

        yield return new WaitForSeconds(intensity);
        shaking = false;
    }
}
