using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject CheckpointText;
    [SerializeField] GameObject mainHUD;
    [SerializeField] GameObject knightHUD;
    [SerializeField] GameObject engineerHUD;
    [SerializeField] GameObject gunnerHUD;
    GameObject currentCheckpointText;
    CharacterBase character;
    // Start is called before the first frame update
    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        knightHUD.SetActive(false);
        gunnerHUD.SetActive(false);
        engineerHUD.SetActive(false);
    }
    public void Initialize()
    {
        UpdateClass(character.weaponClass.classType, character.weaponClass.currentLvl);
    }
    
    public void UpdateClass(WeaponBase.weaponClassTypes weaponClass, int experienceLVL)
    {
        switch (weaponClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                knightHUD.SetActive(true);
                gunnerHUD.SetActive(false);
                engineerHUD.SetActive(false);
                
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                knightHUD.SetActive(false);
                gunnerHUD.SetActive(true);
                engineerHUD.SetActive(false);
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                knightHUD.SetActive(false);
                gunnerHUD.SetActive(false);
                engineerHUD.SetActive(true);
                break;
        }
        UpdateExperienceLevel(weaponClass, experienceLVL);
    }

    public void UpdateExperienceBar()
    {

    }

    public void UpdateExperienceLevel(WeaponBase.weaponClassTypes weaponClass, int experienceLVL)
    {
        switch (weaponClass)
        {
            case WeaponBase.weaponClassTypes.Knight:
                knightHUD.transform.Find("KnightLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                knightHUD.transform.Find("KnightLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
            case WeaponBase.weaponClassTypes.Gunner:
                gunnerHUD.transform.Find("GunnerLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                gunnerHUD.transform.Find("GunnerLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
            case WeaponBase.weaponClassTypes.Engineer:
                engineerHUD.transform.Find("EngineerLVLTextMain").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                engineerHUD.transform.Find("EngineerLVLTextSub").GetComponent<TMP_Text>().text = experienceLVL.ToString();
                break;
        }
    }

    public IEnumerator AnimateCheckpointReached()
    {
        Debug.Log("Animating Checkpoing text");
        currentCheckpointText = Instantiate(CheckpointText);
        currentCheckpointText.transform.SetParent(mainCanvas.transform, false);
        currentCheckpointText.transform.localScale = new Vector3(6.0f, 6.0f, 6.0f);
        var rate = new Vector3(20f, 20f, 20f);
        while(currentCheckpointText.transform.localScale.x != 1.0f)
        {
           Debug.Log(currentCheckpointText.transform.localScale);
            if((currentCheckpointText.transform.localScale.x - (rate.x * Time.deltaTime)) <= 1.0f)
            {
                currentCheckpointText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                yield return null;
            }
            else
            {
                currentCheckpointText.transform.localScale -= (rate * Time.deltaTime);
                yield return null;
            }

        }
        //yield return new WaitForSeconds(2.8f);
        yield return StartCoroutine(ReduceTextOpacity(currentCheckpointText, 1.00f));
        Destroy(currentCheckpointText);
        yield break;
    }

    private IEnumerator ReduceTextOpacity(GameObject text, float rate)
    {
        var reference = text.GetComponent<Text>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

}
