using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MusicLoop", fileName = "newSceneLoop")]
public class MusicLoop : ScriptableObject
{
    [SerializeField] public string loopName;
    [SerializeField] public AudioClip clip;
    [SerializeField] public float volume;
    public AudioSource source;

    public void PlayBackground()
    {
        source.volume = 0.0f;
        source.Play();
        source.Pause();
    }

    public void PlayLoop()
    {
        source.Stop();
        source.loop = true;
        source.Play();
    }

    public void StopLoop()
    {
        source.Stop();
    }
}
