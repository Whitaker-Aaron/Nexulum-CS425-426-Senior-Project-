using System.Collections;
using UnityEngine;

public class SpikeTimer : MonoBehaviour
{
    [SerializeField] private GameObject activeSpikes;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private float delay = 55f;
    [SerializeField] private float restart = 60f;

    private Animator animator;
    private Coroutine spikeCoroutine;

    private void Start()
    {
        activeSpikes.SetActive(false);

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
        }

        if (activeSpikes == null)
        {
            Debug.LogWarning("activeSpikes GameObject is not assigned in " + gameObject.name);
            return;
        }
    }

    private void Update()
    {
        if (spikeCoroutine == null)
        {
            spikeCoroutine = StartCoroutine(LoopSpikes());
        }
    }

    private IEnumerator LoopSpikes()
    {
        Debug.Log("Spike Routine Started");

        activeSpikes.SetActive(true);
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("Activate");
        yield return new WaitForSeconds(waitTime);

        animator.SetTrigger("Deactivate");

        yield return new WaitForSeconds(restart);
        activeSpikes.SetActive(false);

        yield return null;
    }
}
