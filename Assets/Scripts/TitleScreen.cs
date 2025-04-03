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
    SaveManager SaveManager;
    LifetimeManager LifetimeManager;
    UIManager uiManager;
    CharacterBase character;
    MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        newPanel.SetActive(false);
        loadPanel.SetActive(false);
        SaveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        LifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>(); 
        menuManager.menusPaused = true;
        uiManager.DisableHUD();
        if (!SaveManager.hasData) loadGameButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateToNewLoad()
    {
        newPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        loadPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void OnLoad() 
    {
        SaveManager.LoadGame();
        LifetimeManager.StartGame();
    }

    public void OnNew()
    {
        SaveManager.NewGame();
        StartCoroutine(LifetimeManager.StartNewGame());
    }
}
