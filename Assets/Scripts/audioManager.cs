using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSource player;
    private string currentLoop = "";
    public bool playingFootsteps;
    [Range(0f, 1f)] public float loopAudio;
    [SerializedDictionary("SFXName", "SFX")]
    public SerializedDictionary<string, SFX> sfxSources;

    [SerializedDictionary("LoopName", "Loop")]
    public SerializedDictionary<string, MusicLoop> loopSources;

    [SerializedDictionary("FootstepName", "Footsteps")]
    public SerializedDictionary<string, Footsteps> footstepSources;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        LoadSFX();
        LoadLoops();
        LoadFootsteps();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSFX()
    {
        foreach (var item in sfxSources)
        {
            item.Value.source = gameObject.AddComponent<AudioSource>();
            item.Value.source.clip = item.Value.clip;
            item.Value.source.volume = item.Value.volume;
        }
    }

    public void LoadLoops()
    {
        foreach (var item in loopSources)
        {
            item.Value.source = gameObject.AddComponent<AudioSource>();
            item.Value.source.clip = item.Value.clip;
            item.Value.source.volume = item.Value.volume;
            item.Value.PlayBackground();
        }
    }

    public void LoadFootsteps()
    {
        foreach (var item in footstepSources)
        {
            item.Value.source = gameObject.AddComponent<AudioSource>();
            item.Value.source.clip = item.Value.clip;
            item.Value.source.volume = item.Value.volume;
            item.Value.PlayBackground();
        }
    }

    public void PlayFootsteps(string footstepName)
    {
        playingFootsteps = true;
        footstepSources[footstepName].PlayLoop();
    }

    public void PauseFootsteps(string footstepName)
    {
        playingFootsteps = false;
        footstepSources[footstepName].StopLoop();
    }

    public void PlaySFX(string sfx)
    {
        sfxSources[sfx].PlaySFX();
    }

    public void StopSFX(string sfx)
    {
        sfxSources[sfx].StopSFX();
    }

    public void StopLoop()
    {
        /*if (loopSources[currentLoop] != null)
        {
            loopSources[currentLoop].StopLoop();
        }*/
        StartCoroutine(FadeStopLoop());
    }

    public IEnumerator FadeStopLoop()
    {
        if (currentLoop != null && currentLoop != "")
        {
            var startLoop = ReduceVolOnLoop(0.050f);
            StartCoroutine(startLoop);
            yield return new WaitForSeconds(2f);
            loopSources[currentLoop].StopLoop();
        }
    }

    public void PlayLoop(AudioClip newTrack)
    {
        StopLoop();
        audioSource.clip = newTrack;
        audioSource.Play();
    }

    public void ChangeTrack(string newTrack)
    {
        if (newTrack == currentLoop) return;
        if (loopSources[newTrack] != null)
        {
            StartCoroutine(FadeTracks(newTrack));
        }
    }

    public IEnumerator FadeTracks(string newTrack)
    {

        
        if (currentLoop != null && currentLoop != "")
        {
            var startLoop = ReduceVolOnLoop(0.050f);
            StartCoroutine(startLoop);
            yield return new WaitForSeconds(2f);
            loopSources[currentLoop].StopLoop();
        }
        currentLoop = newTrack;
        loopSources[currentLoop].PlayLoop();
        StartCoroutine(IncreaseVolOnLoop(0.050f));
        yield break;
    }

    public IEnumerator ReduceVolOnLoop(float rate)
    {
        while(loopSources[currentLoop].source.volume > 0)
        {
            loopSources[currentLoop].source.volume -= rate * Time.deltaTime;
            if (Mathf.Abs(loopSources[currentLoop].source.volume - 0.0f) < 0.001)
            {
                loopSources[currentLoop].source.volume = 0.0f;
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator IncreaseVolOnLoop(float rate)
    {
        while (loopSources[currentLoop].source.volume < loopSources[currentLoop].volume)
        {
            loopSources[currentLoop].source.volume += rate * Time.deltaTime;
            if (Mathf.Abs(loopSources[currentLoop].source.volume - loopSources[currentLoop].volume) <= 0.001)
            {
                loopSources[currentLoop].source.volume = loopSources[currentLoop].volume;
            }
            yield return null;
        }
        yield break;
    }
}
