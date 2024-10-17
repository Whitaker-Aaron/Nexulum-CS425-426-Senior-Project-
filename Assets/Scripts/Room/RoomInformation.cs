using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomInformation : MonoBehaviour
{
    [SerializeField] bool isCheckpoint;
    [SerializeField] string roomName;

    GameObject character; 
    // Start is called before the first frame update

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnTransition()
    {
        //character.transform.position = startPos.transform.position;
    }
}
