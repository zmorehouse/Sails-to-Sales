using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Light_Fader : MonoBehaviour
{
    private float minIntensity = 0.0f;
    private float maxIntensity = 5.0f;

    void Start()
    {
    }

    void Update()
    {
     
    }

    public void Beat()
    {
        GetComponent<Light>().intensity = maxIntensity;
    }

    public void offBeat()
    {
        GetComponent<Light>().intensity = minIntensity;
    }

}
