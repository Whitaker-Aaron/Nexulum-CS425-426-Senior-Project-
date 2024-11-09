using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Footsteps", fileName = "newFootsteps")]
public class Footsteps : ScriptableObject
{
    [SerializeField] public string footstepName;
    [SerializeField] public float volume;
    [SerializeField] public AudioClip clip;
    public AudioSource source;

    public void PlayBackground()
    {
        //source.volume = 0.0f;
        source.loop = true;
        source.Play();
        source.Pause();
        
    }

    public void PlayLoop()
    {
        source.Stop();
        source.loop = true;
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Resume()
    {
        source.UnPause();
    }

    public void StopLoop()
    {
        source.Stop();
    }
}
