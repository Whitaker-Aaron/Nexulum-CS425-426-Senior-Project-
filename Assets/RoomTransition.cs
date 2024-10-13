using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 changeAmount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Player")
        {
            Debug.Log("Player detected");
            StartCoroutine(Transition(other));
        }
    }

    public IEnumerator Transition(Collider other)
    {
        
        StartCoroutine(GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>().AnimateRoomTransition());
        yield return new WaitForSeconds(0.5f);
        var translate = new Vector3(other.transform.position.x + changeAmount.x, other.transform.position.y + changeAmount.y, other.transform.position.z + changeAmount.z);
        other.transform.position = translate;
    }

    

}
