using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialEventTrigger : MonoBehaviourID, EventTrigger
{
    [SerializeField] public string triggerGuid { get; set; }
    [SerializeField] public string guid;
    [SerializeField] GameObject tutorialRef;
    [SerializeField] public bool isClassDependent = false;
    public bool hasTriggered { get; set; }
    public RoomInformation roomInfo { get; set; }
    [SerializeField] TutorialObject tutorialObject;
    [SerializeField] TutorialObject[] classTutorialObjects;
    masterInput inputManager;
    GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        canvas = GameObject.Find("Canvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject curTutorial;
            //if (dialogueObject != null) StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().LoadDialogueBox(dialogueObject));
            
            if (tutorialRef != null && !isClassDependent)
            {
                var tutorial = tutorialRef.GetComponent<TutorialPage>();
                tutorial.tutorial = tutorialObject;
                tutorial.trigger = this.gameObject;
                curTutorial = Instantiate(tutorialRef);
                curTutorial.transform.SetParent(canvas.transform, false);
                var uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
                var mainTutorial = curTutorial.transform.Find("Tutorial").gameObject;
                uiManager.startTutorialAnimate(mainTutorial);
                inputManager.pausePlayerInput();
            }
            else if(classTutorialObjects != null && isClassDependent)
            {
                var curClass = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().weaponClass.classType;
                TutorialObject tutorialToLoad = null;
                switch (curClass)
                {
                    case WeaponBase.weaponClassTypes.Knight:
                        tutorialToLoad = classTutorialObjects[0];
                        break;
                    case WeaponBase.weaponClassTypes.Gunner:
                        tutorialToLoad = classTutorialObjects[1];
                        break;
                    case WeaponBase.weaponClassTypes.Engineer:
                        tutorialToLoad = classTutorialObjects[2];
                        break;


                }
                if (tutorialToLoad != null)
                {
                    var tutorial = tutorialRef.GetComponent<TutorialPage>();
                    tutorial.tutorial = tutorialToLoad;
                    tutorial.trigger = this.gameObject;
                    curTutorial = Instantiate(tutorialRef);
                    curTutorial.transform.SetParent(canvas.transform, false);
                    var uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
                    var mainTutorial = curTutorial.transform.Find("Tutorial").gameObject;
                    uiManager.startTutorialAnimate(mainTutorial);
                    inputManager.pausePlayerInput();
                }
                
            }
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
        if (roomInfo != null) roomInfo.UpdateTriggerState(triggerGuid, hasTriggered);
    }

}
