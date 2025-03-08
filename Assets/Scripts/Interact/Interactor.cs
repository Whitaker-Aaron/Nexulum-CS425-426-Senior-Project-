using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] public Transform interactionPoint;
    [SerializeField] public float interactionRadius = 2.0f;
    [SerializeField] public LayerMask interactableMask;

    private readonly Collider[] colliders = new Collider[3];
    private i_Interactable currentInteractable;

    private void Update()
    {
        int num = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, colliders, interactableMask);
        //Debug.Log(transform.gameObject);
        //Debug.Log(currentInteractable);
        //Debug.Log(num);
        if (num > 0)
        {
            i_Interactable interactable = colliders[0].GetComponent<i_Interactable>();
            //Debug.Log(interactable);
            
            if (interactable != null)
            {
                if (interactable != currentInteractable)
                {
                    currentInteractable?.HideUI();
                    currentInteractable = interactable;
                    currentInteractable.ShowUI();
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.HideUI();
                currentInteractable = null;
            }
        }
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().
            GetMasterInput().GetComponent<masterInput>().inputPaused == true) return;
        Debug.Log("Input detected");
        Debug.Log(currentInteractable);
        if (context.performed && currentInteractable != null)
        {
            
            currentInteractable.Interact(this);
        }
    }
}
