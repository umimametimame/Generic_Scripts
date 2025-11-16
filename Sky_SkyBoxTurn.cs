using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Sky_SkyBoxTurn : MonoBehaviour
{
    private Volume volume;
    private VolumeProfile profile;
    private HDRISky hdriSky;

    public float speedPerSec;

    private void Start()
    {
        volume = GetComponent<Volume>();
        profile = volume.profile;
        for(int i = 0; i < profile.components.Count; i++)
        {
            if (profile.components[i] is HDRISky)
            {
                hdriSky = (HDRISky)profile.components[i];
            }
        }
    }

    private void Update()
    {
        hdriSky.rotation.value += Time.deltaTime * speedPerSec;
        while(hdriSky.rotation.value > 360.0f)
        {
            hdriSky.rotation.value -= 360.0f;
        }
    }
}
