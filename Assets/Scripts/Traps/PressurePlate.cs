using System.Collections;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject spikes;
    [SerializeField] private float delaySpikes = 2.0f;
    [SerializeField] private float delayReturnSpikes = 2.0f;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            Debug.Log("Pressure plate activated. Spikes will appear after delay.");
            StartCoroutine(ActivateAndResetSpikes());
        }
    }

    private IEnumerator ActivateAndResetSpikes()
    {
        yield return new WaitForSeconds(delaySpikes);

        if (spikes != null)
        {
            spikes.SetActive(true);
            Debug.Log("Spikes have appeared!");
        }
        else
        {
            Debug.LogError("Spikes GameObject is not assigned.");
        }

        yield return new WaitForSeconds(delayReturnSpikes);

        if (spikes != null)
        {
            spikes.SetActive(false);
            Debug.Log("Spikes have disappeared!");
        }
        else
        {
            Debug.LogError("Spikes GameObject is not assigned.");
        }

        isTriggered = false;
        Debug.Log("Pressure plate reset. It can now be triggered again.");
    }
}
