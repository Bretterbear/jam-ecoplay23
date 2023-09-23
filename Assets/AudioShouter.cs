using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShouter : MonoBehaviour
{

    public void AudioPauseClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Button");
    }

    public void AudioClickQuitClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Button");
    }
}
