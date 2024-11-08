using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SFX", fileName = "newSceneAudio")]
public class SFX : ScriptableObject
{
    [SerializeField] public string sfxName;
    [SerializeField] public AudioClip clip;
    public AudioSource source;

    public void PlaySFX()
    {
        source.loop = false;
        source.PlayOneShot(source.clip);
    }
}
