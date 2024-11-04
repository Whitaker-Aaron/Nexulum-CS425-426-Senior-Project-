using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    private CharacterBase character;

    private void OnTriggerEnter(Collider other)
    {
        CharacterBase playerHealth = other.GetComponent<CharacterBase>();

        if (playerHealth != null)
        {
            // Apply damage to the player
            playerHealth.takeDamage(damageAmount);
            Debug.Log("Player hit by spikes and took " + damageAmount + " damage.");
        }
    }
}
