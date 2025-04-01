using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTimer : MonoBehaviour
{
    [SerializeField] GameObject activeSpikes;
    [SerializeField] float waitTime;
    [SerializeField] float delay;
    [SerializeField] float restart;

    Animator animator;

    // Start is called before the first frame update

    private void OnEnable()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(LoopSpikes());
    }

    public IEnumerator LoopSpikes()
    {
        while (true)
        {
            activeSpikes.SetActive(true);
            yield return new WaitForSeconds(delay);
            animator.SetTrigger("Activate");
            yield return new WaitForSeconds(waitTime);
            animator.SetTrigger("Deactivate");
            yield return new WaitForSeconds(restart);
            activeSpikes.SetActive(false);

        }
    }

    private void OnDisable()
    {
        StopCoroutine(LoopSpikes());
    }
}
