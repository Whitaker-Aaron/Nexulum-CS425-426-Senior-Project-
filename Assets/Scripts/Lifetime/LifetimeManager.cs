using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifetimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string currentScene;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GoToScene(string sceneName)
    {
        
        yield return StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        SceneManager.LoadScene(sceneName);
        
    }

    public IEnumerator StartScene()
    {
        currentScene = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().name;
        var title = GameObject.Find("TransitionTitle");
        title.GetComponent<Text>().text = GameObject.Find("SceneInformation").GetComponent<SceneInformation>().transitionTitle;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(IncreaseTitleOpacity(title, 1.75f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ReduceOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(ReduceTitleOpacity(title, 1.00f));

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

    private IEnumerator IncreaseOpacity(GameObject transition, float rate)
    {
        var reference = transition.GetComponent<Image>();
        while (reference.color.a <= 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
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
