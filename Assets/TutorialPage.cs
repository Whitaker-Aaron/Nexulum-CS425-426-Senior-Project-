using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TutorialPage : MonoBehaviour
{
    public TutorialObject tutorial;
    public GameObject trigger;
    [SerializeField] GameObject tutorialName;
    [SerializeField] GameObject tutorialShadow;
    [SerializeField] GameObject tutorialDescription;
    [SerializeField] GameObject tutorialImage;
    [SerializeField] GameObject tutorialAbilityName;
    [SerializeField] GameObject tutorialInputSprite;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject nextPanel;
    [SerializeField] GameObject exitPanel;
    [SerializeField] GameObject nextDisablePanel;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject abilityNameBackdrop;
    [SerializeField] GameObject backDisablePanel;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject pageCount;
    bool usingController = false;
    bool canProgress;
    public int curPage = 0;

    masterInput inputManager;
    AudioManager audioManager;

    // Start is called before the first frame update
    public void Awake()
    {
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void Start()
    {
        Debug.Log("Gamepad active?: " + inputManager.getGamepadActive());
        if (inputManager.getGamepadActive()) usingController = true;
        audioManager.PauseFootsteps("TestWalk");
        audioManager.PlaySFX("TutorialStart");
        canProgress = false;
        StartCoroutine(InputDelay());
        LoadPage();
        //EventSystem.
        
    }

    private void Update()
    {
        //Debug.Log(canProgress);
    }

    public void LoadPage()
    {
        if (tutorial != null)
        {
            Debug.Log(tutorial.name);
            switch (tutorial.tutorialPageTypes[curPage])
            {
                case TutorialObject.pageType.first:
                    
                    exitPanel.SetActive(false);
                    nextPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(nextButton);
                    nextButton.GetComponent<Button>().interactable = true;
                    backButton.GetComponent<Button>().interactable = false;
                    backDisablePanel.SetActive(true);
                    nextDisablePanel.SetActive(false);
                    break;
                case TutorialObject.pageType.middle:
                    
                    exitPanel.SetActive(false);
                    nextPanel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(nextButton);
                    nextButton.GetComponent<Button>().interactable = true;
                    backButton.GetComponent<Button>().interactable = true;
                    backDisablePanel.SetActive(false);
                    nextDisablePanel.SetActive(false);
                    break;
                case TutorialObject.pageType.last:
  
                    exitPanel.SetActive(true);
                    nextPanel.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(exitButton);
                    backButton.GetComponent<Button>().interactable = true;
                    backDisablePanel.SetActive(false);
                    nextDisablePanel.SetActive(false);
                    break;
                case TutorialObject.pageType.only:
        
                    exitPanel.SetActive(true);
                    nextPanel.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(exitButton);
                    backButton.GetComponent<Button>().interactable = false;
                    backDisablePanel.SetActive(true);
                    nextDisablePanel.SetActive(false);
                    break;
            }

            tutorialName.GetComponent<TMP_Text>().text = tutorial.tutorialName;

            pageCount.GetComponent<TMP_Text>().text =
                (curPage + 1).ToString() + "/" + (tutorial.tutorialDialogueList.Count).ToString();

            tutorialShadow.GetComponent<TMP_Text>().text = tutorial.tutorialName;
            tutorialDescription.GetComponent<TMP_Text>().text = tutorial.tutorialDialogueList[curPage];
            tutorialImage.GetComponent<Image>().sprite = tutorial.tutorialDialogueImages[curPage];
            tutorialImage.GetComponent<Image>().preserveAspect = true;

            if (tutorial.tutorialAbilitySpriteKeyboard[curPage] != null && !usingController)
            {
                tutorialInputSprite.SetActive(true);
                tutorialInputSprite.
                    GetComponent<Image>().sprite = tutorial.tutorialAbilitySpriteKeyboard[curPage];
                tutorialInputSprite.
                    GetComponent<Image>().preserveAspect = true;
            }
            else if (tutorial.tutorialAbilitySpriteGamepad[curPage] != null && usingController)
            {
                tutorialInputSprite.SetActive(true);
                tutorialInputSprite.
                    GetComponent<Image>().sprite = tutorial.tutorialAbilitySpriteGamepad[curPage];
                tutorialInputSprite.
                    GetComponent<Image>().preserveAspect = true;
            }
            else
            {
                tutorialInputSprite.SetActive(false);
            }

            if (tutorial.tutorialAbilityName[curPage] != "")
            {
                tutorialAbilityName.SetActive(true);
                abilityNameBackdrop.SetActive(true);
                tutorialAbilityName.GetComponent<TMP_Text>().text = tutorial.tutorialAbilityName[curPage];
            }
            else
            {
                abilityNameBackdrop.SetActive(false);
                tutorialAbilityName.SetActive(false);
            }
        }
    }

    public IEnumerator InputDelay()
    {
        Debug.Log("Inside input delay");
        yield return new WaitForSecondsRealtime(0.25f);
        canProgress = true;
        Debug.Log("Inside input delay2");
        yield return new WaitForSecondsRealtime(0.25f);
        Debug.Log("Inside input delay3");
    }

    public void OnBack()
    {
        if (!canProgress) return;
        if(curPage -1 >= 0) curPage--;
        LoadPage();
        audioManager.PlaySFX("UIConfirm");
        
    }

    public void OnExit()
    {
        if (!canProgress) return;
        GameObject.Find("InputandAnimationManager").GetComponent<masterInput>().resumePlayerInput();
        var menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        menuManager.menusPaused = false;
        audioManager.PlaySFX("UIConfirm");
        Destroy(trigger);
        Destroy(this.gameObject);
        Time.timeScale = 1.0f;
    }


    public void OnNext()
    {
        if (!canProgress) return;
        curPage++;
        LoadPage();
        audioManager.PlaySFX("UIConfirm");
    }

}
