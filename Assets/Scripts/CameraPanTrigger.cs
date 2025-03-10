using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPanTrigger : MonoBehaviourID, EventTrigger
{
    [SerializeField] List<GameObject> objectToPanTo;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] float panSpeed;
    [SerializeField] bool panYAxisLocked = false;
    [SerializeField] bool panLookAtLocked = false;
    [SerializeField] float panDelay = 2f;
    [SerializeField] DialogueObject dialogueObject;
    [SerializeField] TutorialObject tutorialObject;
    [SerializeField] GameObject tutorialRef;
    public RoomInformation roomInfo { get; set; }
    public bool hasTriggered { get; set; }
    [SerializeField] public string triggerGuid { get; set; }
    [SerializeField] public string guid;

    masterInput inputManager;

    CameraFollow camera;
    CharacterBase character;
    GameObject canvas;
    // Start is called before the first frame update

    private void Awake()
    {
        triggerGuid = guid;
        hasTriggered = false;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        canvas = GameObject.Find("Canvas").gameObject;
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();

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
        if (objectToPanTo.Count >= 1)
        {
            camera.panYAxisLocked = panYAxisLocked;
            camera.panLookAtLocked = panLookAtLocked;
        }  
        //yield return new WaitForSeconds(0.25f);
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        character.inEvent = true;
        if (dialogueObject != null) yield return StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(dialogueObject));
        if(objectToPanTo.Count >= 1) yield return StartCoroutine(camera.PanToPosition(objectToPanTo[0].transform.position + offset, panSpeed, panDelay));
        hasTriggered = true;
        //if (tutorialObject != null) StartTutorial();
        UpdateTriggerState();
        character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        character.inEvent = false;
        Destroy(this.gameObject);
    }

    public void StartTutorial()
    {
        GameObject curTutorial;
        //if (dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(dialogueObject));
        var tutorial = tutorialRef.GetComponent<TutorialPage>();
        tutorial.tutorial = tutorialObject;
        tutorial.trigger = this.gameObject;
        if (tutorialRef != null)
        {
            curTutorial = Instantiate(tutorialRef);
            curTutorial.transform.SetParent(canvas.transform, false);
            var uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            var mainTutorial = curTutorial.transform.Find("Tutorial").gameObject;
            uiManager.startTutorialAnimate(mainTutorial);
            inputManager.pausePlayerInput();
        }
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
