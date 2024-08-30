using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject currentInHand;
    public Vector3 offset;
    bool open = false;
    public GameObject Menu;

    public Item itemToRemove;
    public int amountToRemove;
    public GameObject itemToAdd;

    private void Start()
    {
        //Loading all the items
        //SetList();
    }

    private void Update()
    {
        OpenClose();
    }

    public void SetList()
    {
        //This is where we can manage the sorting of weapons based off of class and everything else
    }
    void OpenClose()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            open = !open;
            Menu.SetActive(open);
        }
    }
}
