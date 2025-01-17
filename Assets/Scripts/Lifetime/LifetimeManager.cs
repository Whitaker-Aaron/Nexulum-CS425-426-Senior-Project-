using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;


public class LifetimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentScene;
    GameObject currentInputRef;

    WeaponsManager weaponsManager;
    UIManager uiManager;
    RuneManager runeManager;
    MaterialScrollManager scrollManager;
    MenuManager menuManager;
    CharacterBase characterRef;
    masterInput inputManager;
    SkillTreeManager skillTreeMan;
    projectileManager projMan;
    EffectsManager effectsMan;



    GameObject deathScreen;

    private void Awake()
    {
        weaponsManager = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>();
        runeManager = GameObject.Find("RuneManager").GetComponent<RuneManager>();
        scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
        characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        inputManager = GameObject.Find("InputandAnimationManager").GetComponent<masterInput>();
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        skillTreeMan = GameObject.Find("SkillTreeManager").GetComponent<SkillTreeManager>();
        projMan = GameObject.Find("ProjectileManager").GetComponent<projectileManager>();
        effectsMan = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        deathScreen = GameObject.Find("DeathScreen");
        deathScreen.SetActive(false);
        //currentInputRef.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GoToScene(int index)
    {
        //Debug.Log("Changing to: " + sceneName);
        
        yield return StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        SceneManager.LoadSceneAsync(index);
        yield break;

        //SceneManager.UnloadSceneAsync(currentScene);


    }

    public IEnumerator TeleportPlayer()
    {
        characterRef.teleporting = true;
        menuManager.closePauseMenu();
        menuManager.menusPaused = true;
        characterRef.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        StartCoroutine(GoToScene(characterRef.teleportSpawnObject.sceneNum));
        
        //characterRef.transitionedRoom = false;
        //characterRef.transitioningRoom = false;

        //StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        
        //yield return new WaitForSeconds(2);
        //Load(characterRef.teleportSpawnObject.sceneNum);
        yield return new WaitForSeconds(3);
        inputManager.resumePlayerInput();
        characterRef.teleporting = false;
        yield break;

    }

    public void StartTeleport()
    {
        StartCoroutine(TeleportPlayer());
    }

    public void Load(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }

    public IEnumerator ReturnToScene(int index)
    {
        //Debug.Log("Changing to: " + sceneName);
        //yield return StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        //SceneManager.UnloadSceneAsync(currentScene);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
        yield break;
    }

    public void StartGame()
    {
        InitializeManagers();
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnToBase()
    {
        StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        menuManager.closePauseMenu();
        characterRef.transitioningRoom = true;
        menuManager.menusPaused = true;
        characterRef.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        Load(1);
    }

    public void InitializeManagers()
    {
        projMan.initializePool();
        effectsMan.initialize();
        inputManager.enabled = true;
        weaponsManager.Initialize();
        runeManager.Initialize();
        //skillTreeMan.enabled = true;
        skillTreeMan.Initialize();
        uiManager.Initialize();
        //projMan.initializePool();

        //inputManager.Initialize();
    }

    public void OnDeath()
    {
        
        menuManager.CloseMenu();
        menuManager.menusPaused = true;
        inputManager.pausePlayerInput();
        StartCoroutine(AnimateDeathScreen());

        
    }

    public IEnumerator AnimateDeathScreen()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.5f);
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;
        Time.timeScale = 1.0f;
        var deathScreenObj = deathScreen.GetComponent<DeathScreen>();
        deathScreen.SetActive(true);
        yield return StartCoroutine(deathScreenObj.AnimateDeath());
        yield return new WaitForSeconds(12);
        Load(1);
        characterRef.RecoverFromDeath();
        yield return new WaitForSeconds(3);
        deathScreenObj.ResetObjScales();
        deathScreen.SetActive(false);
        inputManager.resumePlayerInput();
        menuManager.menusPaused = false;
        scrollManager.ClearInventory();
        yield return null;
    }

 

    public IEnumerator StartScene()
    {
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;
        characterRef.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        menuManager.menusPaused = true;

        yield return new WaitForSeconds(0.5f);

        currentScene = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().sceneName;
        var title = GameObject.Find("TransitionTitle");
        uiManager.StartAnimateTypewriterScreenTransition(title.GetComponent<TMP_Text>(), GameObject.Find("SceneInformation").GetComponent<SceneInformation>().transitionTitle, "|", 0.2f);
        //title.GetComponent<Text>().text = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().transitionTitle;
        //yield return StartCoroutine(IncreaseTitleOpacity(title, 1.75f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ReduceOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        if(!characterRef.transitioningRoom && !characterRef.transitionedRoom && !characterRef.teleporting) characterRef.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        yield return new WaitForSeconds(0.2f);
        //yield return StartCoroutine(ReduceTitleOpacity(title, 1.00f));
        menuManager.menusPaused = false;
        //StopAllCoroutines();
        yield break;
        
    }

    private void OnEnable()
    {
        
    }

    private IEnumerator ReduceOpacity(GameObject transition, float rate)
    {
        var reference = transition.GetComponent<Image>();
      

        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        
        yield break;
    }


    public IEnumerator IncreaseOpacity(GameObject transition, float rate)
    {
        var reference = transition.GetComponent<Image>();
        while (reference.color.a < 1.0f)
        {
            if(Mathf.Abs(reference.color.a - 1.0f) <= 0.05)
            {
                Color imgColor = reference.color;
                imgColor.a = 1.0f;
                reference.color = imgColor;
            }
            else
            {
                Color imgColor = reference.color;
                imgColor.a += rate * Time.deltaTime;
                reference.color = imgColor;
            }
            
            yield return null;
        }
        Debug.Log("Finished opacity");
        yield break;
    }

    private IEnumerator IncreaseTitleOpacity(GameObject title, float rate)
    {
        var reference = title.GetComponent<Text>();
        while (reference.color.a <= 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator ReduceTitleOpacity(GameObject title, float rate)
    {
        var reference = title.GetComponent<Text>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    public IEnumerator AnimateRoomTransition()
    {
        var trans = GameObject.Find("RoomTransition");
        Color imgColor = trans.GetComponent<Image>().color;
        imgColor.a = 1.0f;
        trans.GetComponent<Image>().color = imgColor; 

        var reference = trans.GetComponent<RectTransform>();
        reference.transform.localPosition = new Vector3(0.0f, 1200.0f, 0.0f);
        bool hasStopped = false;
        while (reference.transform.localPosition.y >= -1200.0f)
        {
            var val = reference.transform.localPosition;
            reference.transform.localPosition = new Vector3(val.x, val.y -= (3500.0f * Time.deltaTime), val.z);
            if(reference.transform.localPosition.y <= 0.0f && !hasStopped)
            {
                reference.transform.localPosition = new Vector3(val.x, 0.0f, val.z);
                yield return new WaitForSeconds(0.55f);
                hasStopped = true;
            }
            yield return null;
        }
        imgColor = trans.GetComponent<Image>().color;
        imgColor.a = 0.0f;
        trans.GetComponent<Image>().color = imgColor;

        yield break;
    }


}
