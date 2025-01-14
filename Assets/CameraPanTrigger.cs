using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPanTrigger : MonoBehaviourID
{
    [SerializeField] List<GameObject> objectToPanTo;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] float panSpeed;
    [SerializeField] bool panYAxisLocked = false;
    [SerializeField] bool panLookAtLocked = false;
    [SerializeField] float panDelay = 2f;
    [SerializeField] DialogueObject dialogueObject;
    RoomInformation roomInfo;
    public bool hasTriggered = false;
    public string triggerGuid;
    CameraFollow camera;
    CharacterBase character;
    // Start is called before the first frame update

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(StartCameraPanThenDestroy());
        }
    }

    public IEnumerator StartCameraPanThenDestroy()
    {
        camera.panYAxisLocked = panYAxisLocked;
        camera.panLookAtLocked = panLookAtLocked;
        //yield return new WaitForSeconds(0.25f);
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        character.inEvent = true;
        if (dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(dialogueObject));
        yield return StartCoroutine(camera.PanToPosition(objectToPanTo[0].transform.position + offset, panSpeed, panDelay));
        hasTriggered = true;
        UpdateTriggerState();
        character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        character.inEvent = false;
        Destroy(this.gameObject);
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
        roomInfo.UpdateTriggerState(ID, hasTriggered);
    }
}
