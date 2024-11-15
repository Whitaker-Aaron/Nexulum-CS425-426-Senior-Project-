using System.Collections;
using UnityEngine;

public class AutonomousSpikes : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float activationInterval = 5f;
    [SerializeField] private float activeDuration = 2f;
    [SerializeField] public GameObject spikes;
    private bool isActive = false;

    private void Start()
    {
        // Start the cycle for spikes activation
        StartCoroutine(SpikesCycle());
    }

    private IEnumerator SpikesCycle()
    {
        while (true)
        {
            // Activate spikes
            ActivateSpikes();
            yield return new WaitForSeconds(activeDuration);

            // Deactivate spikes
            DeactivateSpikes();
            yield return new WaitForSeconds(activationInterval - activeDuration);
        }
    }

    private void ActivateSpikes()
    {
        isActive = true;
        spikes.SetActive(true); // Show spikes visually
        Debug.Log("Spikes activated!");
    }

    private void DeactivateSpikes()
    {
        isActive = false;
        spikes.SetActive(false); // Hide spikes visually
        Debug.Log("Spikes deactivated!");
    }

}
