using UnityEngine;

public class Spikes : MonoBehaviour , i_Trap
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float startDelay = 1f;

    public void OnTriggerEnter(Collider other)
    {
        CharacterBase playerHealth = other.GetComponent<CharacterBase>();

        if (playerHealth != null)
        {
            // Apply damage to the player
            playerHealth.takeDamage(damageAmount, Vector3.zero);
            Debug.Log("Player hit by spikes and took " + damageAmount + " damage.");
        }
    }
}
