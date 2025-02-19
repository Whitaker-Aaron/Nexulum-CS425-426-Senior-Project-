using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EquipOptionPrefab;

public class ViewItemPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    public ViewType type;
    public WeaponBase weapon;
    public Rune rune;
    public PlayerItem item;

    [SerializeField] public GameObject viewItemName;
    [SerializeField] public GameObject viewOptionDescription;
    [SerializeField] public GameObject viewOptionEffect;
    [SerializeField] public GameObject viewOptionDamage;
    [SerializeField] public GameObject classTypeUI;
    [SerializeField] public GameObject runeTypeUI;
    [SerializeField] public GameObject backButton;
    [SerializeField] public GameObject viewTexture;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onBack()
    {
        GameObject.Find("UIManager").GetComponent<UIManager>().HideViewItem();
    }

    public enum ViewType
    {
        Weapon,
        Item,
        Rune
    }
}
