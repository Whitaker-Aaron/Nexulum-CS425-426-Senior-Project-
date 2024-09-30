/*-----------------------------------------------------
 * classAbilities Script
 * Author: Spencer Garcia
 * Start Date: 9/26/2024
 * 
 * Description:
 * 
 * class ability manager, functions activated from masterInput.
 * base stats contained in this file but could be changed
 * when we implement the skill trees.
 * 
 * --------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class classAbilties : MonoBehaviour
{

    //----------------Variables------------------

    GameObject player;

    //private WeaponBase.weaponClassTypes currentClass;

    //Knight
    bool bubble = false;
    public float bubbleTime = 5f;

    //Gunner

    //Engineer
    public GameObject turretPrefab;
    public GameObject turretTransparentPrefab;
    GameObject currentTurret;
    public float turretPlacementRadius = 3f;
    bool placing = false;
    Vector3 mousePos = Vector3.zero;
    public float turretSpawnHeight;
    bool instant = false;

    //-------------------------------------------

    //----------------Functions------------------

    public void activateAbilityOne(WeaponBase.weaponClassTypes currentClass)
    {
        if(currentClass == WeaponBase.weaponClassTypes.Knight)
        {
            StartCoroutine(bubbleShield());
        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {
            placing = true;
            instant = true;
            gameObject.GetComponent<masterInput>().placing = true;
            //currentTurret = turretTransparentPrefab;
            
        }
    }

    public void activateAbilityTwo(WeaponBase.weaponClassTypes currentClass)
    {
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {

        }
    }

    public void activateAbilityThree(WeaponBase.weaponClassTypes currentClass)
    {
        if (currentClass == WeaponBase.weaponClassTypes.Knight)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Gunner)
        {

        }
        if (currentClass == WeaponBase.weaponClassTypes.Engineer)
        {

        }
    }

    //Knight

    IEnumerator bubbleShield()
    {
        bubble = true;
        player.GetComponent<CharacterBase>().bubbleShield = true;
        yield return new WaitForSeconds(bubbleTime);
        bubble = false;
        player.GetComponent<CharacterBase>().bubbleShield = false;
        yield break;
    }

    //Gunner

    //Engineer

   

    void activateTurret()
    {
        print("activating turret");
        //player look at cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, 100))
        {
            mousePos = hit.point;
        }

        if (instant)
        {
            instant = false;
            currentTurret = GameObject.Instantiate(turretTransparentPrefab, player.transform.position, Quaternion.LookRotation(player.transform.forward));
        }

        float distance = Vector3.Distance(player.transform.position, mousePos);
        
        if(distance <= turretPlacementRadius)
        {
            currentTurret.gameObject.transform.position = mousePos;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else
        {
            Vector3 direction = (mousePos - player.transform.position).normalized;
            currentTurret.gameObject.transform.position = player.transform.position + direction * turretPlacementRadius;
            currentTurret.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            if (currentTurret != null)
            {
                placing = false;
                gameObject.GetComponent<masterInput>().placing = false;
                Quaternion rot = currentTurret.transform.rotation;
                Destroy(currentTurret);
                currentTurret = Instantiate(turretPrefab, mousePos + new Vector3 (0,turretSpawnHeight,0), rot);
            }
        }
    }

    //-------------------------------------------


    //--------------Main Functions---------------

    private void Awake()
    {
        //currentClass = gameObject.GetComponent<masterInput>().currentClass;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(placing)
        {
            activateTurret();
        }
    }
}
