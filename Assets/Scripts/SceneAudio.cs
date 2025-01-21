using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene Audio", fileName = "newSceneAudio")]
public class SceneAudio : ScriptableObject
{
    // Start is called before the first frame update
    [SerializedDictionary("Track Name", "AudioObject")]
    public SerializedDictionary<string, AudioClip> audioSources;
    AudioManager audioManager;

    
    public void PlayTrack(string trackName)
    {
        //GameObject.Find("AudioManager").GetComponent<AudioManager>().ChangeTrack(audioSources[trackName]);
    }

}
