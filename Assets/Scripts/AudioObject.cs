using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioObject : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audio;
    [SerializeField] string trackName;
    void Start()
    {
        //audio = this.GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        audio.PlayOneShot(audio.clip);
    }

}
