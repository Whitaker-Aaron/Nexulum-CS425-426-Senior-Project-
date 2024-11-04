using UnityEngine;

public interface i_Interactable
{
    string interactionPrompt { get; } 
    bool Interact(Interactor interactor);
    void ShowUI();
    void HideUi();
}
