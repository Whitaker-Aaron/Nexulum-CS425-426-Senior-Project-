using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] RoomList;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        
    }

    public IEnumerator DeactivateRooms()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < RoomList.Length; i++)
        {
            if (!RoomList[i].GetComponent<RoomInformation>().floorEntrance)
            {
                RoomList[i].SetActive(false);
            }
        }
    }


}
