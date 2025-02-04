
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using UnityEngine.EventSystems;


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

    LifetimeManager lifetimeManager;
    RoomManager roomManager;

    


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
        var checkpointContent = GameObject.Find("CheckpointContent");
        for(int i =0; i <checkpoints.Count; i++) {
           CheckpointUIRef.GetComponent<CheckpointUI>().spawnObject = checkpoints[i];
           var reference = Instantiate(CheckpointUIRef);
           reference.transform.SetParent(checkpointContent.transform, false);
        }
        
    }

}
