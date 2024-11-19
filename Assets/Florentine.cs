using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Florentine : MonoBehaviour
{

    [SerializeField] float florentineAmount = 0;
    CharacterBase character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            character.AddFlorentine(florentineAmount);
            Destroy(this.transform.gameObject);
        }
    }
}
