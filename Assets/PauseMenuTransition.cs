
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;


public class PauseMenuTransition : MonoBehaviour
{
    GameObject ReturnToBaseButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ReturnToBaseButton = GameObject.Find("ReturnToBaseButton");
        if(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().currentScene != "BaseCamp")
        {
            ReturnToBaseButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            ReturnToBaseButton.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToBase()
    {
        //Time.timeScale = 1.0f;
        var reference = GameObject.Find("TransitionScreen").GetComponent<Image>();
        Color imgColor = reference.color;
        imgColor.a = 1;
        reference.color = imgColor;


        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().IncreaseOpacity(GameObject.Find("TransitionScreen"), 1.00f));
        GameObject.Find("MenuManager").GetComponent<MenuManager>().closePauseMenu();
        GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().Load(1);



        //StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().ReturnToScene(1));

    }

 

}
