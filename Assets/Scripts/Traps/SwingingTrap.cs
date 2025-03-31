using UnityEngine;

public enum SwingDirection { X, Y, Z};

public class SwingingAxeTrap : MonoBehaviour , i_Trap
{
    [Header("Swing Settings")]
    public float swingSpeed = 1f;
    public float swingAngle = 45f;
    public int damageAmount = 20;
    public SwingDirection swingDirection;

    private float initialRotation;

    void Start()
    {
        if (swingDirection == SwingDirection.X)
        {
            initialRotation = transform.localEulerAngles.x;
        }
        else if (swingDirection == SwingDirection.Y)
        {
            initialRotation = transform.localEulerAngles.z;
        }
        else if (swingDirection == SwingDirection.Z)
        {
            initialRotation = transform.localEulerAngles.z;
        }
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        transform.localRotation = Quaternion.Euler(0, 0, initialRotation + angle);
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
