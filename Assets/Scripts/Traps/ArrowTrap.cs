using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowTrap : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;
    public int damageAmount = 10;
    public Vector3 direction = Vector3.forward; // Default direction (+z)

    [Header("Trap Settings")]
    public float spawnInterval = 1f;
    public int distance = 1;

    private void Start()
    {
        StartCoroutine(SpawnArrows());
    }

    private IEnumerator SpawnArrows()
    {
        while (true)
        {
            SpawnArrow();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnArrow()
    {
        if (arrowPrefab == null)
        {
            Debug.LogWarning("Arrow prefab is not assigned.");
            return;
        }
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // Set the direction and speed of the arrow
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(direction.normalized, arrowSpeed, damageAmount, distance);
        }
    }

}
