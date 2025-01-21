using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    CameraFollow camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player has fallen through death plane");
            var character = other.gameObject.GetComponent<CharacterBase>();
            //camera.PauseFollow();
            //camera.PauseLookAt();
            camera.positionTarget = character.lastGroundLocation;
            character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
            GameObject.Find("MenuManager").GetComponent<MenuManager>().menusPaused = true;
            StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().AnimateRoomTransition());
            StartCoroutine(WaitThenTakeDamage(character));
        }
    }

    public IEnumerator WaitThenTakeDamage(CharacterBase character)
    {
        yield return new WaitForSeconds(0.3f);
        camera.SetCameraMode(CameraFollow.FollowMode.PositionLerp);
        yield return new WaitForSeconds(0.1f);
        camera.SetCameraMode(CameraFollow.FollowMode.Lerp);
        character.ResetToGround();
        yield return new WaitForSeconds(0.75f);

        if (!character.transitionedRoom && !character.transitioningRoom)
        {
            character.takeDamage(20, Vector3.zero);
            //camera.UnpauseFollow();
            //camera.UnpauseLookAt();
        }
        character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        GameObject.Find("MenuManager").GetComponent<MenuManager>().menusPaused = false;
    }
}
