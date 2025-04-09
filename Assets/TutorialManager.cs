using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialRef;
    UIManager uiManager;
    masterInput inputManager;
    GameObject canvas;
    CharacterBase character;

    [SerializedDictionary("TutorialName", "Tutorial")]
    public SerializedDictionary<string, TutorialObject> tutorialLookup;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        canvas = GameObject.Find("Canvas").gameObject;
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    

    public TutorialObject returnTutorial(string tutorialToReturn)
    {
        return tutorialLookup[tutorialToReturn];
    }

    public void LoadPage(TutorialObject tutorialToLoad)
    {
        GameObject curTutorial;
        var tutorial = tutorialRef.GetComponent<TutorialPage>();
        tutorial.tutorial = tutorialToLoad;
        tutorial.trigger = this.gameObject;
        curTutorial = Instantiate(tutorialRef);
        curTutorial.transform.SetParent(canvas.transform, false);
        var uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        var menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        var mainTutorial = curTutorial.transform.Find("Tutorial").gameObject;
        menuManager.menusPaused = true;
        uiManager.startTutorialAnimate(mainTutorial);
        inputManager.pausePlayerInput();
        
        Time.timeScale = 0.0f;
    }
}
