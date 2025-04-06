using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickManager : MonoBehaviour
{
    GameObject lastFocusedItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFocusedItem == null && EventSystem.current.currentSelectedGameObject != null)
        {
            lastFocusedItem = EventSystem.current.currentSelectedGameObject;
        }
        else if(EventSystem.current.currentSelectedGameObject != null && lastFocusedItem.name != EventSystem.current.currentSelectedGameObject.name)
        {
            lastFocusedItem = EventSystem.current.currentSelectedGameObject;
        }

        if (EventSystem.current.currentSelectedGameObject == null && lastFocusedItem != null)
        {
            if(lastFocusedItem.activeSelf) EventSystem.current.SetSelectedGameObject(lastFocusedItem);
        }
    }

}
