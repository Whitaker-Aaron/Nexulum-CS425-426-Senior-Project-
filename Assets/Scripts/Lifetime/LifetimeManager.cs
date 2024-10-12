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

    void Start()
    {
       
        //currentInputRef.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GoToScene(int index)
    {
        //Debug.Log("Changing to: " + sceneName);
        
        //yield return StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        SceneManager.LoadSceneAsync(index);

        yield break;

        //SceneManager.UnloadSceneAsync(currentScene);


    }

    public void Load(int index)
    {
        //SceneManager.LoadSceneAsync(index);
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
        //currentInputRef.SetActive(true);
        StartCoroutine(GoToScene(1));
    }

    public IEnumerator StartScene()
    {
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;

        yield return new WaitForSeconds(0.5f);

        currentScene = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().name;
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
            if(Mathf.Abs(reference.color.a - 1.0f) <= 0.5)
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


}
