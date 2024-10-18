using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RoomTransition : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] Vector3 changeAmount;
    [SerializeField] GameObject targetRoom;
    [SerializeField] GameObject currentRoom;
    [SerializeField] GameObject targetLoad;
    [SerializeField] TransitionDirection exitDirection;
    [SerializeField] TransitionDirection enterDirection;
    RoomInformation targetInfo;
    RoomInformation currentInfo;
    CameraFollow cameraBehavior;
    UIManager uiManager;
    Coroutine currentMove;
    void Start()
    {
        targetInfo = targetRoom.GetComponent<RoomInformation>();
        currentInfo = currentRoom.GetComponent<RoomInformation>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        cameraBehavior = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetEnemyPositions()
    {
        var enemies = targetInfo.GetEnemies();
        for(int i =0;  i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<EnemyFrame>().resetPosition();
            }           
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            var character = other.GetComponent<CharacterBase>();
            Debug.Log("Player detected");
            if (!character.transitioningRoom)
            {
                cameraBehavior.PauseFollow();
                GameObject.Find("RoomManager").GetComponent<RoomManager>().SetRoom(targetInfo);
                character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
                character.transitioningRoom = true;
                StartCoroutine(MovePlayerForward(other, exitDirection));
                StartCoroutine(Transition(other));
                ResetEnemyPositions();
            }
            else if(character.transitioningRoom)
            {
                character.transitionedRoom = true;
                var directionOffset = new Vector3(0.0f, 0.0f, 0.0f);
                switch (enterDirection)
                {
                    case TransitionDirection.forward:
                        directionOffset = new Vector3(0.0f, 0.0f, 5.0f);
                        break;
                    case TransitionDirection.backward:
                        directionOffset = new Vector3(0.0f, 0.0f, -5.0f);
                        break;
                    case TransitionDirection.right:
                        directionOffset = new Vector3(10.0f, 0.0f, 0.0f);
                        break;
                    case TransitionDirection.left:
                        directionOffset = new Vector3(-10.0f, 0.0f, 0.0f);
                        break;

                }
                cameraBehavior.transform.position = character.transform.position + cameraBehavior.offset + directionOffset;
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var character = other.GetComponent<CharacterBase>();
        if (other.name == "Player")
        {   
            Debug.Log("Player detected");
            if (character.transitionedRoom)
            {
                StartCoroutine(ResumeControl(other));

            }
        }
    }

    public IEnumerator ResumeControl(Collider other)
    {
        var character = other.GetComponent<CharacterBase>();
        yield return new WaitForSeconds(0.3f);
        character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        character.transitionedRoom = false;
        character.transitioningRoom = false;
        cameraBehavior.UnpauseFollow();
        if (currentInfo.isCheckpoint && currentInfo.firstVisit)
        {
            Debug.Log("Need to animate checkpoint");
            StartCoroutine(uiManager.AnimateCheckpointReached());
        }
        
        yield return null;
    }

    public IEnumerator Transition(Collider other)
    {
        var character = other.GetComponent<CharacterBase>();
        yield return new WaitForSeconds(1f);
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().AnimateRoomTransition());
        yield return new WaitForSeconds(0.22f);
        
        character.transform.position = targetLoad.transform.position;
        //roomInfo.OnTransition();
        //var translate = new Vector3(other.transform.position.x + changeAmount.x, other.transform.position.y + changeAmount.y, other.transform.position.z + changeAmount.z);
        //other.transform.position = translate;
    }

    public IEnumerator MovePlayerForward(Collider character, TransitionDirection direction)
    {
        var changeAmount = new Vector3(0.0f, 0.0f, 0.0f);
        var characterBase = character.GetComponent<CharacterBase>();

        switch (direction)
        {
            case TransitionDirection.forward:
                changeAmount = new Vector3(0.0f, 0.0f, 2.5f);
                character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 0, character.transform.rotation.z);  
                break;
            case TransitionDirection.backward:
                changeAmount = new Vector3(0.0f, 0.0f, -2.5f);
                character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 180.0f, character.transform.rotation.z);
                break;
            case TransitionDirection.right:
                changeAmount = new Vector3(2.5f, 0.0f, 0.0f);
                character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 90.0f, character.transform.rotation.z);
                break;
            case TransitionDirection.left:
                changeAmount = new Vector3(-2.5f, 0.0f, 0.0f);
                character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 270.0f, character.transform.rotation.z);
                break;
        }

        while (characterBase.transitioningRoom)
        {
            character.transform.position += changeAmount*Time.deltaTime;
            yield return null;
        }
        yield break;
        
    }

  


}

public enum TransitionDirection
{
    forward,
    backward,
    right,
    left
}


