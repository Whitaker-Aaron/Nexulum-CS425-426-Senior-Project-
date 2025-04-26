
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class PauseMenuTransition : MonoBehaviour
{
    GameObject ReturnToBaseButton;
    GameObject SaveButton;

    [SerializeField] GameObject SkillMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject KnightSkillMenu;
    [SerializeField] GameObject GunnerSkillMenu;
    [SerializeField] GameObject EngineerSkillMenu;
    [SerializeField] GameObject MapMenu;
    [SerializeField] GameObject knightSkillPanelButton;

    [SerializeField] GameObject CheckpointUIRef;

    [SerializeField] WeaponClass knightRef;
    [SerializeField] WeaponClass gunnerRef;
    [SerializeField] WeaponClass engineerRef;

    [SerializeField] GameObject MapButton;
    [SerializeField] bool lockSave = false;
    CharacterBase characterRef;
    List<GameObject> checkpointList = new List<GameObject>(); 
    LifetimeManager lifetimeManager;
    AudioManager audioManager;
    RoomManager roomManager;

    GameObject checkpointContent = null;
    ScrollRect checkpointScrollRect = null;

    GameObject mapContent = null;
    ScrollRect mapScrollRect = null;

    GameObject skillTreeContent = null;
    ScrollRect skillTreeScrollRect = null;

    Vector2 initialMapPos;
    Vector2 initialSkillsPos;
    string curRoom;
    string curEventSystem;
    
    Vector3 curRoomCoordinates;

    


    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        //audioManager.PlaySFX("Pause");
    }

    private void OnEnable()
    {
        Debug.Log("Current room at pause: " + roomManager.currentRoom.roomName);
        curRoom = roomManager.currentRoom.roomName;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MapButton);

    }

    private void OnDisable()
    {
        audioManager.PlaySFX("Pause");
    }




    private void Awake()
    {
        characterRef = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        SkillMenu.SetActive(false);
        KnightSkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(false);

        ReturnToBaseButton = GameObject.Find("ReturnToBaseButton");
        SaveButton = GameObject.Find("SaveButton");

        

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MapButton);



        if (GameObject.Find("RoomManager").GetComponent<RoomManager>().IsCheckpoint())
        {
            //SaveButton.GetComponent<Button>().navigation = 
            SaveButton.SetActive(true);
            //SaveButton.GetComponent<Button>().interactable = true;
            SaveButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(false);
            if (GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().currentScene != "BaseCamp")
            {
                ReturnToBaseButton.SetActive(true);
                //ReturnToBaseButton.GetComponent<Button>().interactable = true;
                ReturnToBaseButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(false);
            }
            else
            {
                ReturnToBaseButton.SetActive(false);
                //ReturnToBaseButton.GetComponent<Button>().interactable = false;
                ReturnToBaseButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(true);
            }
        }
        else
        {
            SaveButton.SetActive(false);
            //SaveButton.GetComponent<Button>().interactable = false;
            SaveButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(true);
            //ReturnToBaseButton.GetComponent<Button>().interactable = false;
            ReturnToBaseButton.SetActive(false);
            ReturnToBaseButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(true);
        }
        if (lockSave)
        {
            SaveButton.SetActive(false);
            SaveButton.transform.parent.Find("DisabledPanel").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }

        if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.name == "Scrollbar Vertical"
            || EventSystem.current.currentSelectedGameObject.name == "Scrollbar Horizontal")
        {
            return;
        }
        else if (EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name != "CheckpointRoom(Clone)")
        {
            if (initialMapPos != Vector2.zero)
            {
                var mapContentPanel = mapContent.GetComponent<RectTransform>();
                mapContentPanel.anchoredPosition = initialMapPos;
            }
            if (initialSkillsPos != Vector2.zero)
            {
                var skillsContentPanel = skillTreeContent.GetComponent<RectTransform>();
                skillsContentPanel.anchoredPosition = initialSkillsPos;
            }
        }
        if(EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name == "CheckpointRoom(Clone)"
            && checkpointContent != null && checkpointScrollRect != null && mapContent != null && mapScrollRect != null)
        {
            var selectedItem = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent;
            RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();

            var contentPanel = checkpointContent.GetComponent<RectTransform>();
            Vector2 newPos = (Vector2)checkpointScrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)checkpointScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
            float newPosY = (float)newPos.y;
            //if(newPosY-100f <= 0)
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newPosY-100f);

            Canvas.ForceUpdateCanvases();
            var mapContentPanel = mapContent.GetComponent<RectTransform>();
            var selectedSprite = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<CheckpointUI>().spriteOnMap;
            if(selectedSprite != null)
            {
                Vector2 newMapPos = (Vector2)mapScrollRect.transform.InverseTransformPoint(mapContentPanel.position)
            - (Vector2)mapScrollRect.transform.InverseTransformPoint(selectedSprite.transform.position);
                mapContentPanel.anchoredPosition = new Vector2(newMapPos.x, newMapPos.y);
            }
            

        }
        else if(EventSystem.current.currentSelectedGameObject.transform.parent.name == "SkillTreePanel" && skillTreeContent != null && skillTreeScrollRect != null)
        {
            Debug.Log("Event system in skill tree panel.");
            var selectedItem = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent;
            RectTransform selectedItemRect = selectedItem.GetComponent<RectTransform>();

            var contentPanel = skillTreeContent.GetComponent<RectTransform>();
            Vector2 newPos = (Vector2)skillTreeScrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)skillTreeScrollRect.transform.InverseTransformPoint(selectedItemRect.position);
            float newPosY = (float)newPos.y;
            //if(newPosY-100f <= 0)
            contentPanel.anchoredPosition = new Vector2(newPos.x+2400f, contentPanel.anchoredPosition.y);
        }
    }

    public void SaveGame()
    {
        audioManager.PlaySFX("UIConfirm");
        GameObject.Find("SaveManager").GetComponent<SaveManager>().SaveGame();
    }

    public void returnToMainSkills()
    {
        audioManager.PlaySFX("UIConfirm");
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(knightSkillPanelButton);
        KnightSkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(false);
    }

    public void returnToMainPause()
    {
        audioManager.PlaySFX("UIConfirm");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MapButton);

        CleanUpCheckpoint();
        SkillMenu.SetActive(false);
        KnightSkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(false);
        MapMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void ReturnToBase()
    {
        //Time.timeScale = 1.0f;
        audioManager.PlaySFX("UIConfirm");
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;


        GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().ReturnToBase();



        //StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().ReturnToScene(1));

    }

    public void ExitGame()
    {
        audioManager.PlaySFX("UIConfirm");
        Application.Quit();
    }

    public void OpenSkills()
    {
        audioManager.PlaySFX("UIConfirm");
        PauseMenu.SetActive(false);
        SkillMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(knightSkillPanelButton);
    }

    public void OpenKnightSkills()
    {
        audioManager.PlaySFX("UIConfirm");
        SkillMenu.SetActive(false);
        KnightSkillMenu.SetActive(true);
        skillTreeContent = GameObject.Find("KnightContent");
        skillTreeScrollRect = GameObject.Find("KnightView").GetComponent<ScrollRect>();
        var skillsContentPanel = skillTreeContent.GetComponent<RectTransform>();
        initialSkillsPos = skillsContentPanel.anchoredPosition;
    }

    public void OpenGunnerSkills()
    {
        audioManager.PlaySFX("UIConfirm");
        SkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(true);
        skillTreeContent = GameObject.Find("GunnerContent");
        skillTreeScrollRect = GameObject.Find("GunnerView").GetComponent<ScrollRect>();
        var skillsContentPanel = skillTreeContent.GetComponent<RectTransform>();
        initialSkillsPos = skillsContentPanel.anchoredPosition;
    }

    public void OpenEngineerSkills()
    {
        audioManager.PlaySFX("UIConfirm");
        SkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(true);
        skillTreeContent = GameObject.Find("EngineerContent");
        skillTreeScrollRect = GameObject.Find("EngineerView").GetComponent<ScrollRect>();
        var skillsContentPanel = skillTreeContent.GetComponent<RectTransform>();
        initialSkillsPos = skillsContentPanel.anchoredPosition;
    }

    public void OpenMapMenu()
    {
        audioManager.PlaySFX("UIConfirm");
        PauseMenu.SetActive(false);
        MapMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("BackButton"));
        PopulateSpawnObjects();

    }

    public void PopulateSpawnObjects()
    {
        var checkpoints = roomManager.GetCheckpoints();
        checkpointContent = GameObject.Find("CheckpointContent");
        mapContent = GameObject.Find("MapContent");
        checkpointScrollRect = GameObject.Find("CheckpointView").GetComponent<ScrollRect>();
        mapScrollRect = GameObject.Find("MapView").GetComponent<ScrollRect>();
        var disabledPanel = GameObject.Find("CheckpointsDisabledPanel");
        if (roomManager.IsCheckpoint() && disabledPanel != null) disabledPanel.SetActive(false);
        else if(disabledPanel != null)  disabledPanel.SetActive(true);

        var mapContentPanel = mapContent.GetComponent<RectTransform>();
        initialMapPos = mapContentPanel.anchoredPosition;

        for (int i =0; i <checkpoints.Count; i++) {
           CheckpointUIRef.GetComponent<CheckpointUI>().spawnObject = checkpoints[i];
           var button = CheckpointUIRef.GetComponent<CheckpointUI>().goButton.GetComponent<Button>();
           if (roomManager.IsCheckpoint()) button.interactable = true;
           else button.interactable = false;
           var reference = Instantiate(CheckpointUIRef);
           reference.transform.SetParent(checkpointContent.transform, false);
           checkpointList.Add(reference);
           
        }
        AttachSpriteToCheckpoint();
        ActivateRoomOnMap();
        
    }
    public void ActivateRoomOnMap()
    {
        var allVisitedRooms = roomManager.allVisitedRoomsData;
        int mapChildrenCount = mapContent.transform.childCount;
        var curRoomPanel = GameObject.Find("CurrentRoomPanel");
        var noMapDataPanel = GameObject.Find("NoMapDataFound");
        if (curRoom == "BaseCamp" && curRoomPanel != null) curRoomPanel.SetActive(false);
        else if(curRoomPanel != null) curRoomPanel.SetActive(true);
        bool roomsFound = false;

        for (int i = 0; i < mapChildrenCount; i++)
        {
            
            bool roomVisited = false;
            if (mapContent.transform.GetChild(i).GetComponent<MapMarker>() != null)
            {
                
                var mapRoomName = mapContent.transform.GetChild(i).GetComponent<MapMarker>().roomToMapTo;
                for (int j = 0; j < allVisitedRooms.Count; j++)
                {
                    if(mapRoomName == allVisitedRooms[j].roomName)
                    {
                        roomsFound = true;
                        roomVisited = true;
                        mapContent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    if (mapRoomName == curRoom)
                    {
                        var roomTransPos = mapContent.transform.GetChild(i).transform.position;
                        curRoomPanel.transform.position = new Vector3(roomTransPos.x - 145, roomTransPos.y + 50, 0);

                        var mapContentPanel = mapContent.GetComponent<RectTransform>();

                        if (roomTransPos != null)
                        {
                            Vector2 newMapPos = (Vector2)mapScrollRect.transform.InverseTransformPoint(mapContentPanel.position)
                        - (Vector2)mapScrollRect.transform.InverseTransformPoint(roomTransPos);
                            mapContentPanel.anchoredPosition = new Vector2(newMapPos.x, newMapPos.y);
                        }
                        initialMapPos = mapContentPanel.anchoredPosition;

                    }
                }
                if (!roomVisited) mapContent.transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }
        if(roomsFound && noMapDataPanel != null) noMapDataPanel.SetActive(false);
        else if(noMapDataPanel != null) noMapDataPanel.SetActive(true);

    }

    public void CleanUpCheckpoint()
    {

        if (checkpointList.Count <= 0) return;
        for (int i=0; i < checkpointList.Count; i++)
        {
            Destroy(checkpointList[i]);
            checkpointList.RemoveAt(i);
            i--;

        }
        checkpointContent = null;
        checkpointScrollRect = null;
        mapContent = null;
        initialMapPos = Vector2.zero;
        mapScrollRect = null;
    }


    public void AttachSpriteToCheckpoint()
    {
        int mapChildrenCount = mapContent.transform.childCount;
        for(int i =0; i < mapChildrenCount; i++)
        {
            if(mapContent.transform.GetChild(i).GetComponent<MapMarker>() != null)
            {
                var mapMarker = mapContent.transform.GetChild(i).GetComponent<MapMarker>();
                for(int j = 0; j < checkpointList.Count; j++)
                {
                    var checkpointUI = checkpointList[j].GetComponent<CheckpointUI>();
                    if (mapMarker.roomToMapTo == checkpointUI.spawnObject.roomName) checkpointUI.spriteOnMap = mapContent.transform.GetChild(i).gameObject;
                    

                }

            }
        }

    }


}
