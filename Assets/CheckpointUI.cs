using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointUI : MonoBehaviour
{
    [SerializeField] public GameObject goButton;
    [SerializeField] GameObject roomName;
    [SerializeField] GameObject roomNameShadow;
    [SerializeField] public RoomSpawnObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        roomName.GetComponent<TMP_Text>().text = spawnObject.stylizedRoomName;
        roomNameShadow.GetComponent<TMP_Text>().text = spawnObject.stylizedRoomName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGoButton()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().teleportSpawnObject = spawnObject;
        GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().StartTeleport();
    }
}
