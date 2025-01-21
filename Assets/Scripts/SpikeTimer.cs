using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTimer : MonoBehaviour
{
    [SerializeField] GameObject activeSpikes;
    [SerializeField] float waitTime;
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(LoopSpikes());
    }

    public IEnumerator LoopSpikes()
    {
        while (true)
        {
            activeSpikes.SetActive(false);
            yield return new WaitForSeconds(waitTime);
            activeSpikes.SetActive(true);
            yield return new WaitForSeconds(waitTime);
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(LoopSpikes());
    }
}
