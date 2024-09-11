using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateToMaterialMenu()
    {
        Debug.Log("Button pressed");
        GameObject.Find("MenuManager").GetComponent<MenuManager>().navigateToMaterialMenu();
    }




}
