using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class OpeningCutscene : MonoBehaviour
{
    LifetimeManager lifetimeManager;
    UIManager uiManager;
    [SerializeField] TMP_Text headerText;
    [SerializeField] GameObject playerBox;
    [SerializeField] GameObject landscapeBox;
    [SerializeField] GameObject landscapeBorder;
    [SerializeField] GameObject landscapeScanlines;
    [SerializeField] GameObject playerDisplayModel;
    [SerializeField] GameObject playerCover;
    [SerializeField] GameObject playerScanlines;
    [SerializeField] GameObject playerBorder;
    [SerializeField] GameObject pointLight;
    [SerializeField] List<GameObject> imagesToDisplay = new List<GameObject>();
    [SerializeField] public List<DialogueObject> transmissionDialogues;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        lifetimeManager = GameObject.Find("LifetimeManager").GetComponent<LifetimeManager>();
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        player.inEvent = true;
        playerBox.SetActive(false);
        pointLight.SetActive(false);
        landscapeBox.SetActive(false);
        playerDisplayModel.SetActive(false);
        InitializeDisplayImages();
        StartCoroutine(CutsceneFlow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator CutsceneFlow()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(lifetimeManager.DeanimateTransitionScreen());
        StartCoroutine(uiManager.AnimateTypewriterCheckpoint(headerText, "Establishing Connection...", "|", 0.06f, true));
        yield return new WaitForSeconds(2f);

        playerBox.SetActive(true);

        StartCoroutine(uiManager.IncreaseImageOpacity(playerCover, 1.0f, true));
        StartCoroutine(uiManager.IncreaseImageOpacity(playerBox, 1.0f, true));
        StartCoroutine(uiManager.IncreaseImageOpacity(playerScanlines, 1.0f, true));
        StartCoroutine(uiManager.IncreaseImageOpacity(playerBorder, 1.0f, true));
        yield return new WaitForSeconds(0.25f);
        landscapeBox.SetActive(true);
        StartCoroutine(uiManager.IncreaseImageOpacity(landscapeBox, 1.0f, true));
        StartCoroutine(uiManager.IncreaseImageOpacity(landscapeScanlines, 1.0f, true));
        StartCoroutine(uiManager.IncreaseImageOpacity(landscapeBorder, 1.0f, true));
        yield return new WaitForSeconds(1f);

        playerDisplayModel.SetActive(true);
        yield return new WaitForSeconds(2f);
        pointLight.SetActive(true);
        StartCoroutine(uiManager.DecreaseImageOpacity(playerCover, 1.0f));

        yield return new WaitForSeconds(4.5f);
        StartCoroutine(uiManager.AnimateTypewriterCheckpoint(headerText, "Incoming Transmission...", "|", 0.06f, false));
        yield return new WaitForSeconds(1f);
        uiManager.EnableDialogueBox();
        yield return StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().AwaitLoadDialogueBox(transmissionDialogues[0]));
        StartCoroutine(uiManager.IncreaseImageOpacity(imagesToDisplay[0], 1.0f, true));
        yield return StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().AwaitLoadDialogueBox(transmissionDialogues[1]));
        StartCoroutine(uiManager.IncreaseImageOpacity(imagesToDisplay[1], 1.0f, true));
        StartCoroutine(uiManager.DecreaseImageOpacity(imagesToDisplay[0], 1.0f));
        yield return StartCoroutine(GameObject.Find("UIManager").GetComponent<UIManager>().AwaitLoadDialogueBox(transmissionDialogues[2]));
        yield return null;
    }

    public void InitializeDisplayImages()
    {
        
        for (int i =0; i < imagesToDisplay.Count; i++)
        {
            var reference = imagesToDisplay[i].GetComponent<Image>();
            Color imgColor = reference.color;
            imgColor.a = 0;
            reference.color = imgColor;
        }
    }
}
