
using UnityEngine;
using UnityEngine.UI;

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
        if(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().currentScene != "Base")
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
        GameObject.Find("MenuManager").GetComponent<MenuManager>().closePauseMenu();
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().GoToScene("BaseCamp"));
    }
}
