using UnityEngine;
using System.Collections;

public class TimedSpikes : MonoBehaviour, i_Trap
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float activationDelay = 1f; 
    [SerializeField] private float activeDuration = 5f;
    [SerializeField] private float deactivateDelay = 0.5f; 
    private Collider spikeCollider;
    public GameObject spikes;

    private void Start()
    {
        spikeCollider = GetComponent<Collider>();
        if (spikeCollider == null)
        {
            Debug.LogError("TimedSpikes: No collider attached to the spikes.");
        }

        spikes.SetActive(true);
        StartCoroutine(ActivateSpikes());
    }

    private IEnumerator ActivateSpikes()
    {
        yield return new WaitForSeconds(activationDelay);

        if (spikes != null)
        {
            spikes.SetActive(true);
        }
        yield return new WaitForSeconds(activeDuration);

        if (spikes != null)
        {
            spikes.SetActive(false);
        }

        yield return new WaitForSeconds(deactivateDelay);
    }

    public void OnTriggerEnter(Collider other)
    {
        CharacterBase playerHealth = other.GetComponent<CharacterBase>();

        if (playerHealth != null)
        {
            playerHealth.takeDamage(damageAmount, Vector3.zero);
            Debug.Log("Player hit by spikes and took " + damageAmount + " damage.");
        }
    }
}
