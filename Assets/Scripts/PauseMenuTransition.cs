
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

    [SerializeField] GameObject CheckpointUIRef;

    [SerializeField] WeaponClass knightRef;
    [SerializeField] WeaponClass gunnerRef;
    [SerializeField] WeaponClass engineerRef;

    [SerializeField] GameObject MapButton;
    CharacterBase characterRef;
    List<GameObject> checkpointList = new List<GameObject>(); 
    LifetimeManager lifetimeManager;
    RoomManager roomManager;

    GameObject checkpointContent = null;
    ScrollRect checkpointScrollRect = null;

    GameObject mapContent = null;
    ScrollRect mapScrollRect = null;

    Vector2 initialMapPos;

    


    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name != "CheckpointRoom(Clone)")
        {
            if (initialMapPos != Vector2.zero)
            {
                var mapContentPanel = mapContent.GetComponent<RectTransform>();
                mapContentPanel.anchoredPosition = initialMapPos;
            }
            return;
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
            if(newPosY-100f <= 0) contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newPosY - 100f);


            var mapContentPanel = mapContent.GetComponent<RectTransform>();
            var selectedSprite = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<CheckpointUI>().spriteOnMap;
            if(selectedSprite != null)
            {
                Vector2 newMapPos = (Vector2)mapScrollRect.transform.InverseTransformPoint(mapContentPanel.position)
            - (Vector2)mapScrollRect.transform.InverseTransformPoint(selectedSprite.transform.position);
                mapContentPanel.anchoredPosition = new Vector2(newMapPos.x, newMapPos.y);
            }
            

        }
    }

    public void SaveGame()
    {
        GameObject.Find("SaveManager").GetComponent<SaveManager>().SaveGame();
    }

    public void returnToMainSkills()
    {
        SkillMenu.SetActive(true);
        KnightSkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(false);
    }

    public void returnToMainPause()
    {
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
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;


        GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().ReturnToBase();



        //StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().ReturnToScene(1));

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSkills()
    {
        PauseMenu.SetActive(false);
        SkillMenu.SetActive(true);
    }

    public void OpenKnightSkills()
    {
        SkillMenu.SetActive(false);
        KnightSkillMenu.SetActive(true);
    }

    public void OpenGunnerSkills()
    {
        SkillMenu.SetActive(false);
        GunnerSkillMenu.SetActive(true);
    }

    public void OpenEngineerSkills()
    {
        SkillMenu.SetActive(false);
        EngineerSkillMenu.SetActive(true);
    }

    public void OpenMapMenu()
    {
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

        var mapContentPanel = mapContent.GetComponent<RectTransform>();
        initialMapPos = mapContentPanel.anchoredPosition;

        for (int i =0; i <checkpoints.Count; i++) {
           CheckpointUIRef.GetComponent<CheckpointUI>().spawnObject = checkpoints[i];
           var reference = Instantiate(CheckpointUIRef);
           reference.transform.SetParent(checkpointContent.transform, false);
           checkpointList.Add(reference);
        }
        AttachSpriteToCheckpoint();
        
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
