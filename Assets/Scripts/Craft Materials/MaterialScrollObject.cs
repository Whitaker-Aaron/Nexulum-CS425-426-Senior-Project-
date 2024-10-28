using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MaterialScrollObject : MonoBehaviour
{
    [SerializeField] public RawImage imageRef;
    [SerializeField] public Image background;
    [SerializeField] public TMP_Text descriptionMain;
    [SerializeField] public TMP_Text descriptionSub;
    [SerializeField] public TMP_Text quantityMain;
    [SerializeField] public TMP_Text quantitySub;

    public int quantityInt; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
