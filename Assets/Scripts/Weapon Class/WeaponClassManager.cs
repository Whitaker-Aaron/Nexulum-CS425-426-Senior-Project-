using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClassManager : MonoBehaviour
{
    CharacterBase characterReference;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        characterReference = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWeaponClass(WeaponClass classToChangeTo)
    {
        characterReference.GetMasterInput().GetComponent<masterInput>().currentClass = classToChangeTo.classType;
    }
}
