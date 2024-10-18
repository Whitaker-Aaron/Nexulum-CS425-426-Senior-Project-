using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomInformation : MonoBehaviour
{
    [SerializeField] public bool isCheckpoint;
    [SerializeField] public string roomName;
    [SerializeField] GameObject enemies;
    GameObject[] allEnemies;

    GameObject character; 
    // Start is called before the first frame update

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player");

        allEnemies = new GameObject[enemies.transform.childCount];

        for(int i =0; i <  allEnemies.Length; i++)
        {
            allEnemies[i] = enemies.transform.GetChild(i).gameObject;
        }
    }

    public void OnTransition()
    {
        //character.transform.position = startPos.transform.position;
    }

    public GameObject[] GetEnemies()
    {
        return allEnemies;
    }
}
