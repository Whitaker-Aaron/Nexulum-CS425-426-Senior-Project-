using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTrigger : MonoBehaviourID, EventTrigger
{
    [SerializeField] public string triggerGuid { get; set; }
    [SerializeField] public string guid;
    [SerializeField] DialogueObject dialogueObject;
    public bool hasTriggered { get; set; }
    public RoomInformation roomInfo { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(dialogueObject));
            hasTriggered = true;
            UpdateTriggerState();
            Destroy(this.gameObject);
        }
    }

    private void Awake()
    {
        hasTriggered = false;
        triggerGuid = guid;
    }

    public void SetRoomInfo(RoomInformation roomInfo_)
    {
        roomInfo = roomInfo_;
    }

    public void DisableTrigger()
    {
        Destroy(this.gameObject);
    }

    public void UpdateTriggerState()
    {
        Debug.Log(triggerGuid);
        roomInfo.UpdateTriggerState(triggerGuid, hasTriggered);
    }
}
