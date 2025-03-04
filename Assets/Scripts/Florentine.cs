using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Florentine : MonoBehaviourID, Collectible
{

    [SerializeField] float florentineAmount = 0;
    CharacterBase character;
    [SerializeField] public string collectibleGuid { get; set; }
    [SerializeField] public string guid;
    public RoomInformation roomInfo { get; set; }
    public bool hasCollected { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        collectibleGuid = guid;
        //hasTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            character.AddFlorentine(florentineAmount);
            hasCollected = true;
            UpdateCollectible();
            DisableCollectible();
            //Destroy(this.transform.gameObject);
        }
    }

    public void SetRoomInfo(RoomInformation roomInfo_)
    {
        Debug.Log("Sending room info to collectible from: " + roomInfo_.roomName);
        roomInfo = roomInfo_;
    }

    public void DisableCollectible()
    {
        Destroy(this.gameObject);
    }

    public void UpdateCollectible()
    {
        roomInfo.UpdateCollectibleState(collectibleGuid, hasCollected);
    }
}
