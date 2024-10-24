using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public GameObject popUpUI;
    [SerializeField] string interactableName;
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        //gameObject.SetActive(false);  // For example, deactivate the item after interacting
    }

    // Method to show the pop-up UI
    public void ShowPopUp()
    {
        if (popUpUI != null)
        {
            popUpUI.SetActive(true); 
        }
    }

    // Method to hide the pop-up UI
    public void HidePopUp()
    {
        if (popUpUI != null)
        {
            popUpUI.SetActive(false);
        }
    }
}
