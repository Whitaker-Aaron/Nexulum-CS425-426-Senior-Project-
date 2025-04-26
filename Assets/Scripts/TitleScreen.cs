using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button loadGameButton;
    [SerializeField] GameObject newPanel;
    [SerializeField] GameObject loadPanel;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject logo;
    SaveManager SaveManager;
    LifetimeManager LifetimeManager;
    UIManager uiManager;
    AudioManager audioManager;
    CharacterBase character;
    MenuManager menuManager;
    bool loadingGame = false;
    string curEventSystem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitThenStart());
    }

    public IEnumerator waitThenStart()
    {
        yield return new WaitForSeconds(0.5f);
        StartLogic();
    }

    public void StartLogic()
    {
        newPanel.SetActive(false);
        loadPanel.SetActive(false);
        SaveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        LifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        menuManager.menusPaused = true;
        uiManager.DisableHUD();
        StartCoroutine(animateTitleScreen());
        //curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        if (!SaveManager.hasData) loadGameButton.interactable = false;
    }

    private void Update()
    {
        if(curEventSystem == null) curEventSystem = EventSystem.current.currentSelectedGameObject.name;
        else if (EventSystem.current.currentSelectedGameObject.name != curEventSystem)
        {
            curEventSystem = EventSystem.current.currentSelectedGameObject.name;
            audioManager.PlaySFX("UIChange");
        }
    }

    public IEnumerator animateLogoScale()
    {
        var image = logo.GetComponent<Image>();
        image.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        var desScale = new Vector3(1, 1, 1);
        while (image.transform.localScale.x >= 1)
        {
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, desScale, 1f * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator animateTitleScreen()
    {
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(LifetimeManager.DeanimateTransitionScreen());
        StartCoroutine(animateLogoScale());
        yield return StartCoroutine(uiManager.IncreaseImageOpacity(logo, 1f, true));
        yield return null;
    }


    public void NavigateToNewLoad()
    {
        audioManager.PlaySFX("UIConfirm");
        newPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        loadPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void OnLoad() 
    {
        //if (loadingGame) return;
        audioManager.PlaySFX("UIConfirm");
        SaveManager.LoadGame();
        LifetimeManager.StartGame();
        loadingGame = true;
    }

    public void OnNew()
    {
        //if (loadingGame) return;
        audioManager.PlaySFX("UIConfirm");
        SaveManager.NewGame();
        StartCoroutine(LifetimeManager.StartNewGame());
        loadingGame = true;
    }
}
