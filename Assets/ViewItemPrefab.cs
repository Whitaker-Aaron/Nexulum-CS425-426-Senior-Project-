using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipOptionPrefab;

public class ViewItemPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    public EquipTypes type;
    public WeaponBase weapon;
    public WeaponClass weaponClass;
    public Rune rune;

    [SerializeField] public GameObject viewOptionName;
    [SerializeField] public GameObject viewOptionEquipText;
    [SerializeField] public GameObject viewOptionDescription;
    [SerializeField] public GameObject viewOptionButton;
    [SerializeField] public GameObject equipOptionEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
