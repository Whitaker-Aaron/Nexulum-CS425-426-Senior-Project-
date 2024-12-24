using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassEquipDetails : MonoBehaviour
{
    public TMP_Text spCount;
    public TMP_Text lvlCount;
    public TMP_Text tillNextLvlCount;
    [SerializeField] WeaponClass weaponClass;
    // Start is called before the first frame update
    void Start()
    {
        spCount.text = weaponClass.numSkillPoints.ToString();
        lvlCount.text = weaponClass.currentLvl.ToString();
        tillNextLvlCount.text = (weaponClass.getNextLvlExperienceAmount() - weaponClass.totalExp).ToString();
    }
}
