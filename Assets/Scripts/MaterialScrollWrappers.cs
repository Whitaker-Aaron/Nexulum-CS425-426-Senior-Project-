using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialScrollWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public MaterialScrollObject scrollObject;
    [SerializeField] public RawImage imageRef;
    [SerializeField] public Image background;
    [SerializeField] public TMP_Text descriptionMain;
    [SerializeField] public TMP_Text descriptionSub;
    [SerializeField] public TMP_Text quantityMain;
    [SerializeField] public TMP_Text quantitySub;
    [SerializeField] public Image dropShadowRef;
    public int quantityInt;
}
