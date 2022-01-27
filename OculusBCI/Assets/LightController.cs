using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float _lightLevel = 0;
    [SerializeField] private float _lightFadeSpeed = 1.0f;

    private List<Light> _lights = new List<Light>();
    private readonly List<float> _lightIntensities = new List<float>();
    

    
    private void Awake()
    {
        _lights = GameObject.FindObjectsOfType<Light>().ToList();
        foreach (Light light in _lights)
        {
            _lightIntensities.Add(light.intensity);
            light.intensity = 0;
        }
    }

    
    
    
    
    public void StartFadeLightsIn()
    {
        SetLightBrightness(0);
        StartCoroutine(FadeLightsIn());
    }
    private IEnumerator FadeLightsIn()
    {
        while (_lightLevel < 1)
        {
            float incrementValue = _lightFadeSpeed * Time.deltaTime;
            _lightLevel = (_lightLevel + incrementValue > 1) ? 1 : _lightLevel + incrementValue;
            SetLightBrightness(_lightLevel);
            
            yield return null;
        }
        
        GameController.Instance.GameState = GameState_Enum.LIGHTS_ON;
        yield return null;
    }

    public void StartFadeLightsOut()
    {
        SetLightBrightness(1);
        StartCoroutine(FadeLightsOut());
    }
    private IEnumerator FadeLightsOut()
    {
        while (_lightLevel > 0)
        {
            float incrementValue = _lightFadeSpeed * Time.deltaTime;
            _lightLevel = (_lightLevel - incrementValue < 0) ? 0 : _lightLevel - incrementValue;
            SetLightBrightness(_lightLevel);
            
            yield return null;
        }
        
        GameController.Instance.GameState = GameState_Enum.LIGHTS_OFF;
        yield return null;
    }

    private void SetLightBrightness(float value)
    {
        Debug.Log($"<color=#BBBB00>Light</color> Level: {_lightLevel}");
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].intensity = _lightIntensities[i] * value;
        }
    }
}
