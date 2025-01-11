using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClassEquipDetails : MonoBehaviour
{
    public TMP_Text spCount;
    public TMP_Text spCountBackdrop;
    public TMP_Text lvlCount;
    public TMP_Text lvlCountBackdrop;
    public TMP_Text tillNextLvlCount;
    public TMP_Text tillNextLvlCountBackdrop;
    [SerializeField] WeaponClass weaponClass;
    // Start is called before the first frame update
    void Start()
    {
        spCount.text = weaponClass.numSkillPoints.ToString();
        lvlCount.text = weaponClass.currentLvl.ToString();
        tillNextLvlCount.text = (weaponClass.getNextLvlExperienceAmount() - weaponClass.totalExp).ToString();
        spCountBackdrop.text = weaponClass.numSkillPoints.ToString();
        lvlCountBackdrop.text = weaponClass.currentLvl.ToString();
        tillNextLvlCountBackdrop.text = (weaponClass.getNextLvlExperienceAmount() - weaponClass.totalExp).ToString();
    }
}
