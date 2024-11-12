using UnityEngine;

public interface i_Interactable
{
    bool Interact(Interactor interactor);
    void ShowUI();
    void HideUI();
}
