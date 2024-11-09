using UnityEngine;

public class DoorCollisionDetector : MonoBehaviour , i_Trap
{   
    public int damageAmount = 1;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterBase playerHealth = other.GetComponent<CharacterBase>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(damageAmount, Vector3.zero);
            }
        }
    }
}
