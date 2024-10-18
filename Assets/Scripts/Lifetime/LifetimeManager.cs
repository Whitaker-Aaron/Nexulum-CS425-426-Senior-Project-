using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LifetimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentScene;
    GameObject currentInputRef;

    WeaponsManager weaponsManager;
    RuneManager runeManager;
    MaterialScrollManager scrollManager;
    CharacterBase characterRef;


    GameObject deathScreen;
    
    

    void Awake()
    {
       weaponsManager = GameObject.Find("WeaponManager").GetComponent<WeaponsManager>();
       runeManager = GameObject.Find("RuneManager").GetComponent<RuneManager>();
       scrollManager = GameObject.Find("ScrollManager").GetComponent<MaterialScrollManager>();
       characterRef = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();    
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

    public void InitializeManagers()
    {
        GameObject.Find("InputandAnimationManager").GetComponent<masterInput>().enabled = true;
        weaponsManager.Initialize();
        runeManager.Initialize();
    }

    public void OnDeath()
    {
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;
        StartCoroutine(AnimateDeathScreen());

        
    }

    public IEnumerator AnimateDeathScreen()
    {
        var deathScreenObj = deathScreen.GetComponent<DeathScreen>();
        characterRef.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        deathScreen.SetActive(true);
        StartCoroutine(deathScreenObj.AnimateDeath());
        yield return new WaitForSeconds(12);
        Load(1);
        yield return new WaitForSeconds(3);
        deathScreenObj.ResetObjScales();
        deathScreen.SetActive(false);
        characterRef.RecoverFromDeath();
        scrollManager.ClearInventory();
        characterRef.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        yield return null;
    }

 

    public IEnumerator StartScene()
    {
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;

        yield return new WaitForSeconds(0.5f);

        currentScene = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().sceneName;
        var title = GameObject.Find("TransitionTitle");
        title.GetComponent<Text>().text = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().transitionTitle;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(IncreaseTitleOpacity(title, 1.75f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ReduceOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(ReduceTitleOpacity(title, 1.00f));
        StopAllCoroutines();
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
            Debug.Log(reference.color.a);
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
            reference.transform.localPosition = new Vector3(val.x, val.y -= (4500.0f * Time.deltaTime), val.z);
            if(reference.transform.localPosition.y <= 0.0f && !hasStopped)
            {
                yield return new WaitForSeconds(1);
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
