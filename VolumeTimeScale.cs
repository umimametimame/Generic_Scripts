using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeTimeScale : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField, NonEditable] private VisualEnvironment volumeVisual;
    private float volumeInitialSpeed;

    [SerializeField] private WaterSurface water;
    private float waterInitialSpeed;

    private void Start()
    {
        volume.profile.TryGet(out volumeVisual);
        volumeInitialSpeed = volumeVisual.windSpeed.value;
        waterInitialSpeed = water.timeMultiplier;
    }
    private void Update()
    {
        volumeVisual.windSpeed.value = Time.timeScale * volumeInitialSpeed;
        water.timeMultiplier = Time.timeScale * waterInitialSpeed;
    }
}
