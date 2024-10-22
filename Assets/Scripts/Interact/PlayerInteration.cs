using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 9f;
    private InteractableItem currentItem = null;

    void Update()
    {
        DetectInteractableItem();

        if (currentItem != null && Input.GetKeyDown(KeyCode.F))
        {
            currentItem.Interact();
        }
    }

    private void DetectInteractableItem()
    {
        InteractableItem[] interactableItems = FindObjectsOfType<InteractableItem>();
        currentItem = null;

        foreach (InteractableItem item in interactableItems)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance <= interactionRange)
            {
                currentItem = item;
                currentItem.ShowPopUp();
                return;
            }
        }

        if (currentItem == null)
        {
            foreach (InteractableItem item in interactableItems)
            {
                item.HidePopUp();
            }
        }
    }
}
