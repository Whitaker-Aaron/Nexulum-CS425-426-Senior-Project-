using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ClassSelectScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button knightButton;
    [SerializeField] Button gunnerButton;
    [SerializeField] Button engineerButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button backButton;
    [SerializeField] TMP_Text chooseClassText;
    [SerializeField] GameObject knightBanner;
    [SerializeField] GameObject gunnerBanner;
    [SerializeField] GameObject engineerBanner;
    [SerializeField] GameObject knightPanel;
    [SerializeField] GameObject gunnerPanel;
    [SerializeField] GameObject engineerPanel;
    [SerializeField] GameObject continuePanel;
    [SerializeField] GameObject backPanel;
    LifetimeManager lifetimeManager;
    AudioManager audioManager;
    string curEventSystem;
    bool classSelected = false;
    bool loadingGame = false;
    void Start()
    {

        
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        player.inEvent = true;
        player.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        continuePanel.SetActive(false);
        backPanel.SetActive(false);
        loadingGame = false;
        //disableButtons();
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
        //enableButtons();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(knightButton.gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }

        if (EventSystem.current.currentSelectedGameObject == knightButton.gameObject)
        {
            //hoverOverKnight();
        }
        else if (EventSystem.current.currentSelectedGameObject == gunnerButton.gameObject)
        {
            //hoverOverGunner();
        }
        else if (EventSystem.current.currentSelectedGameObject == engineerButton.gameObject)
        {
            //hoverOverEngineer();
        }
    }

    public void defaultHover()
    {
        if (EventSystem.current.currentSelectedGameObject == knightButton.gameObject)
        {
            hoverOverKnight();
        }
        else if (EventSystem.current.currentSelectedGameObject == gunnerButton.gameObject)
        {
            hoverOverGunner();
        }
        else if (EventSystem.current.currentSelectedGameObject == engineerButton.gameObject)
        {
            hoverOverEngineer();
        }
    }

    public void hoverOverKnight()
    {
        if (classSelected) return;
        knightBanner.SetActive(true);
        gunnerBanner.SetActive(false);
        engineerBanner.SetActive(false);
    }

    public void hoverOverGunner()
    {
        if (classSelected) return;
        knightBanner.SetActive(false);
        gunnerBanner.SetActive(true);
        engineerBanner.SetActive(false);
    }

    public void hoverOverEngineer()
    {
        if (classSelected) return;
        knightBanner.SetActive(false);
        gunnerBanner.SetActive(false);
        engineerBanner.SetActive(true);
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

    public void returnToSelection()
    {
        enableButtons();
        disableContinueBackButtons();
        classSelected = false;
    }

    public void disableContinueBackButtons()
    {
        continueButton.interactable = false;
        backButton.interactable = false;
        continuePanel.SetActive(false);
        backPanel.SetActive(false);
        disableAllSelectedTexts();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(knightButton.gameObject);
    }

    public void disableAllSelectedTexts()
    {
        deactivateSelectedText(knightPanel);
        deactivateSelectedText(gunnerPanel);
        deactivateSelectedText(engineerPanel);
    }

    public void enableContinueBackButtons()
    {
        continuePanel.SetActive(true);
        backPanel.SetActive(true);
        continueButton.interactable = true;
        backButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton.gameObject);
    }

    public void startGame()
    {
        StartCoroutine(StartGame());
    }

    public void changeClassKnight()
    {
        disableButtons();
        classSelected = true;
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Knight);
        audioManager.PlaySFX("UIConfirm");
        activateSelectedText(knightPanel);
        enableContinueBackButtons();
    }

    public void activateSelectedText(GameObject parent)
    {
        var text = parent.transform.Find("selectedText");
        var text_shadow = parent.transform.Find("selectedTextShadow");
        var color = text.GetComponent<TMP_Text>().color;
        color.a = 1.0f;
        var color_shadow = text_shadow.GetComponent<TMP_Text>().color;
        color_shadow.a = 1.0f;
        text.GetComponent<TMP_Text>().color = color;
        text_shadow.GetComponent<TMP_Text>().color = color_shadow;
    }

    public void deactivateSelectedText(GameObject parent)
    {
        var text = parent.transform.Find("selectedText");
        var text_shadow = parent.transform.Find("selectedTextShadow");
        var color = text.GetComponent<TMP_Text>().color;
        color.a = 0.0f;
        var color_shadow = text_shadow.GetComponent<TMP_Text>().color;
        color_shadow.a = 0.0f;
        text.GetComponent<TMP_Text>().color = color;
        text_shadow.GetComponent<TMP_Text>().color = color_shadow;
    }

    public void changeClassEngineer()
    {
        disableButtons();
        classSelected = true;
        Debug.Log("Changing class to Knight");
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Engineer);
        audioManager.PlaySFX("UIConfirm");
        activateSelectedText(engineerPanel);
        enableContinueBackButtons();

    }

    public void changeClassGunner()
    {
        disableButtons();
        classSelected = true;
        var characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        characterRef.UpdateClass(WeaponBase.weaponClassTypes.Gunner);
        audioManager.PlaySFX("UIConfirm");
        activateSelectedText(gunnerPanel);
        enableContinueBackButtons();
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
