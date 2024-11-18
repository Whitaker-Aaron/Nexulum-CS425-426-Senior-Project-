using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomInformation : MonoBehaviour
{
    [SerializeField] public bool isCheckpoint;
    [SerializeField] public string roomName;
    [SerializeField] GameObject enemies;
    [SerializeField] bool lockYAxis = false;
    List<GameObject> allEnemies = new List<GameObject>();
    public bool firstVisit = true;
    public bool floorEntrance = false;

    GameObject character; 
    // Start is called before the first frame update

    private void Awake()
    {
        
        character = GameObject.FindGameObjectWithTag("Player");
        

        //allEnemies = new GameObject[enemies.transform.childCount];

        for(int i =0; i < enemies.transform.childCount; i++)
        {
            allEnemies.Add(enemies.transform.GetChild(i).gameObject);
        }
    }

    private void OnEnable()
    {
        if (lockYAxis)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yAxisLocked = true;
            Debug.Log("Locking camera y-axis");
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yAxisLocked = false;
            Debug.Log("Unlocking camera y-axis");
        }
        ActivateEnemyHealthBars();
    }

    private void OnDisable()
    {
        DeactivateEnemyHealthBars();
    }

    private void Update()
    {
        for(int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] == null)
            {
                Debug.Log("Removing enemy from list");
                allEnemies.RemoveAt(i);
                i--;
            }
            
        }
    }

    private void Start()
    {
        if (floorEntrance)
        {
            
            StartCoroutine(WaitThenStartCharacterMove());
        }
    }

    public IEnumerator WaitThenStartCharacterMove()
    {
        
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(character.GetComponent<CharacterBase>().MoveForward());
        yield break;
    }

    public void OnTransition()
    {
        //character.transform.position = startPos.transform.position;
    }

    public List<GameObject> GetEnemies()
    {
        return allEnemies;
    }

    public void DeactivateEnemyHealthBars()
    {
       for(int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                allEnemies[i].GetComponent<EnemyFrame>().DeactivateHealthBar();
            }
            else
            {
                break;
            }
        }
    }

    public void ActivateEnemyHealthBars()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                allEnemies[i].GetComponent<EnemyFrame>().ActivateHealthBar();
            }
            else
            {
                break;
            }
        }
    }
}
