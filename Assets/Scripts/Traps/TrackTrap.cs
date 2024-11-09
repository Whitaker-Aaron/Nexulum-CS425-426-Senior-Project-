using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTrap : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.left;
    public float speed = 1f;
    private float length;
    public GameObject centerWeapon;
    private BoxCollider collide;
    private Vector3 initialPosition;
    private float buffer = 0.01f;

    void Start()
    {
        collide = GetComponent<BoxCollider>();
        GetLength();
        initialPosition = centerWeapon.transform.position;
    }

    private void Update()
    {
        MoveWeapon();
    }

    public void GetLength()
    {
        length = collide.bounds.size.x - 1f;
        Debug.Log("The length is: " + length);
    }

    public void MoveWeapon()
    {
        float leftBoundary = initialPosition.x - length / 2;
        float rightBoundary = initialPosition.x + length / 2;

        if (centerWeapon.transform.position.x <= leftBoundary + buffer)
        {
            moveDirection = Vector3.right;
        }
        else if (centerWeapon.transform.position.x >= rightBoundary - buffer)
        {
            moveDirection = Vector3.left;
        }

        centerWeapon.transform.position += moveDirection * speed * Time.deltaTime;
    }
}
