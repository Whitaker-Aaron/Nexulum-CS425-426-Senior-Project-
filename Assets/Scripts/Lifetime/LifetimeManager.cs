using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifetimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GoToScene(string sceneName)
    {
        
        yield return StartCoroutine(IncreaseOpacity(GameObject.Find("TransitionScreen")));
        SceneManager.LoadScene(sceneName);
        
    }

    public IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ReduceOpacity(GameObject.Find("TransitionScreen")));
    }

    private void OnEnable()
    {
        
    }

    private IEnumerator ReduceOpacity(GameObject transition)
    {
        var reference = transition.GetComponent<Image>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= 1.00f * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator IncreaseOpacity(GameObject transition)
    {
        var reference = transition.GetComponent<Image>();
        while (reference.color.a <= 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += 1.00f * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }


}
