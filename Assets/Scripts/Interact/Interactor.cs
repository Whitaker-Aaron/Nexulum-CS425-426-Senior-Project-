using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 2.0f;
    [SerializeField] private LayerMask interactableMask;

    private readonly Collider[] colliders = new Collider[3];
    private i_Interactable currentInteractable;

    private void Update()
    {
        int num = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, colliders, interactableMask);

        if (num > 0)
        {
            i_Interactable interactable = colliders[0].GetComponent<i_Interactable>();
            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable?.HideUi();
                    currentInteractable = interactable;
                    currentInteractable.ShowUI();
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.HideUi();
                currentInteractable = null;
            }
        }
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractable != null)
        {
            Debug.Log("Input detected");
            currentInteractable.Interact(this);
        }
    }
}
