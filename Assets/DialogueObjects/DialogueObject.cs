using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue", fileName = "newDialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] public List<string> dialogueList = new List<string>();
    [SerializeField] public float textRate = 0.25f;
    [SerializeField] public bool dialogueFinished = false;
    [SerializeField] public bool stopPlayer = false;
    [SerializeField] public string leadingChar = "";
}
