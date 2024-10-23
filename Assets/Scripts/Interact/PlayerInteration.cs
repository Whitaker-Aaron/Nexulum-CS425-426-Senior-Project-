using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 9f;
    private InteractableItem currentItem = null;
    InteractableItem[] interactableItems;
    CharacterBase character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }

    void Update()
    {
        DetectInteractableItem();

        if (currentItem != null)
        {
            character.inRangeOfTerminal = true;
            //currentItem.Interact();
        }
        else
        {
            character.inRangeOfTerminal = false;
        }
    }

    private void DetectInteractableItem()
    {
        interactableItems = FindObjectsOfType<InteractableItem>();
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
