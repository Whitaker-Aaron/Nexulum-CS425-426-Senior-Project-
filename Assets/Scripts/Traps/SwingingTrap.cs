using UnityEngine;

public class SwingingAxeTrap : MonoBehaviour , i_Trap
{
    [Header("Swing Settings")]
    public float swingSpeed = 1f;
    public float swingAngle = 45f;
    public int damageAmount = 20;

    private float initialRotationZ;

    void Start()
    {
        initialRotationZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        transform.localRotation = Quaternion.Euler(0, 0, initialRotationZ + angle);
    }

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
