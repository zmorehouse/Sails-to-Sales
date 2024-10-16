using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{

public AudioClip ClickOnBeat;
public AudioClip Click; 
public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
    }

public void ClickHigh(){
GetComponent<AudioSource> ().PlayOneShot (ClickOnBeat, 1);
}

public void ClickLow(){
GetComponent<AudioSource> ().PlayOneShot (Click, 1);

}

}
