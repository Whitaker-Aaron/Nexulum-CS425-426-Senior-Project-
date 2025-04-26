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
    AudioManager audioManager;
    string curEventSystem;
    bool loadingGame = false;
    void Start()
    {

        
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        player.inEvent = true;
        player.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        loadingGame = false;
        disableButtons();
        StartCoroutine(StartAnimations());
        
    }



    public IEnumerator StartAnimations()
    {
        StartCoroutine(awaitTransition());
        StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().AnimateTypewriterCheckpoint(chooseClassText, chooseClassText.text, "|", 0.10f, false));
        yield break;
    }

    public IEnumerator awaitTransition()
    {
        yield return StartCoroutine(lifetimeManager.DeanimateTransitionScreen());
        enableButtons();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(knightButton.gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }

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

    public void disableButtons()
    {
        knightButton.interactable = false;
        gunnerButton.interactable = false;
        engineerButton.interactable = false;
    }
    public void enableButtons()
    {
        knightButton.interactable = true;
        gunnerButton.interactable = true;
        engineerButton.interactable = true;
    }

    public void changeClassKnight()
    {
        disableButtons();
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Knight);
        audioManager.PlaySFX("UIConfirm");
        StartCoroutine(StartGame());



    }

    public void changeClassEngineer()
    {
        disableButtons();
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Engineer);
        audioManager.PlaySFX("UIConfirm");
        StartCoroutine(StartGame());


    }

    public void changeClassGunner()
    {
        disableButtons();
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Gunner);
        audioManager.PlaySFX("UIConfirm");
        StartCoroutine(StartGame());


    }

    public IEnumerator StartGame()
    {
        loadingGame = true;
        //lifetimeManager.GoToScene(1, true, "BaseCamp");
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        yield return StartCoroutine(lifetimeManager.AnimateTransitionScreen());
        player.inEvent = false;
        player.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        lifetimeManager.Load(1);
        GameObject.Find("UIManager").GetComponent<UIManager>().EnableHUD();
        yield break;
    }
}
