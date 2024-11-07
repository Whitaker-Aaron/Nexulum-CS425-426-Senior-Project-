using UnityEngine;

public class Spikes : MonoBehaviour , i_Trap
{
    [SerializeField] private int damageAmount = 10;

    public void OnTriggerEnter(Collider other)
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
