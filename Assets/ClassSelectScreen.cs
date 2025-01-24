using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ClassSelectScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button knightButton;
    [SerializeField] Button gunnerButton;
    [SerializeField] Button engineerButton;
    [SerializeField] TMP_Text chooseClassText;
    [SerializeField] GameObject knightBanner;
    [SerializeField] GameObject gunnerBanner;
    [SerializeField] GameObject engineerBanner;
    LifetimeManager lifetimeManager;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(knightButton.gameObject);
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        player.inEvent = true;
        player.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().AnimateTypewriterCheckpoint(chooseClassText, chooseClassText.text, "|", 0.06f, false));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (EventSystem.current.currentSelectedGameObject == knightButton.gameObject)
        {
            knightBanner.SetActive(true);
            gunnerBanner.SetActive(false);
            engineerBanner.SetActive(false);
        }
        else if (EventSystem.current.currentSelectedGameObject == gunnerButton.gameObject)
        {
            knightBanner.SetActive(false);
            gunnerBanner.SetActive(true);
            engineerBanner.SetActive(false);
        }
        else if (EventSystem.current.currentSelectedGameObject == engineerButton.gameObject)
        {
            knightBanner.SetActive(false);
            gunnerBanner.SetActive(false);
            engineerBanner.SetActive(true);
        }
    }

    public void changeClassKnight()
    {
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Knight);
        StartGame();


    }

    public void changeClassEngineer()
    {
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Engineer);
        StartGame();

    }

    public void changeClassGunner()
    {
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Gunner);
        StartGame();
    }

    public void StartGame()
    {
        //lifetimeManager.GoToScene(1, true, "BaseCamp");
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        player.inEvent = false;
        player.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        lifetimeManager.Load(1);
    }
}
