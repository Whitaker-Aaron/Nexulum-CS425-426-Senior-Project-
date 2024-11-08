using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Arrow : MonoBehaviour , i_Trap
{
    private Vector3 moveDirection;
    private float speed;
    private int damage;
    private int distance;

    [Header("Arrow Lifetime")]
    public float arrowLife = 5f;
   
    public void Initialize(Vector3 direction, float speed, int damage, int distance)
    {
        
        this.moveDirection = direction;
        this.speed = speed;
        this.damage = damage;
        this.distance = distance;

        AlignWithDirection();

        Destroy(gameObject, arrowLife);
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterBase playerHealth = other.GetComponent<CharacterBase>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
    private void AlignWithDirection()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
}
