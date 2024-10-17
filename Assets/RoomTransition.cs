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
    [SerializeField] GameObject targetLoad;
    [SerializeField] TransitionDirection direction;
    RoomInformation roomInfo;
    Coroutine currentMove;
    void Start()
    {
        roomInfo = targetRoom.GetComponent<RoomInformation>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            var character = other.GetComponent<CharacterBase>();
            Debug.Log("Player detected");
            if (!character.transitioningRoom)
            {
                character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
                character.transitioningRoom = true;
                StartCoroutine(MovePlayerForward(other, direction));
                StartCoroutine(Transition(other));
            }
            else if(character.transitioningRoom)
            {
                character.transitionedRoom = true;
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
                this.StopAllCoroutines();
                character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
                character.transitionedRoom = false;
                character.transitioningRoom= false;
                

                //StartCoroutine(Transition(other));
            }
        }
    }

    public IEnumerator Transition(Collider other)
    {
        var character = other.GetComponent<CharacterBase>();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().AnimateRoomTransition());
        yield return new WaitForSeconds(0.5f);
        
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


