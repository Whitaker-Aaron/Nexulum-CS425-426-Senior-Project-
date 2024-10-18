using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DungeonEntrance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "Player")
        {
            var character = other.GetComponent<CharacterBase>();
            if (!character.transitioningRoom)
            {
                StartCoroutine(AnimateDungeonEntrance(other));

            }
            
        }
    }

    public IEnumerator AnimateDungeonEntrance(Collider other)
    {
        var character = other.GetComponent<CharacterBase>();
        character.transitioningRoom = true;
        character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().PauseFollow();
        character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, 0, character.transform.rotation.z);
        StartCoroutine(character.MoveForward());
        yield return new WaitForSeconds(2);
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().GoToScene(2));
        //character.transform.position += changeAmount * Time.deltaTime;
    }




}
