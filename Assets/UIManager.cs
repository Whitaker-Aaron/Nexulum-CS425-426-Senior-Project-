using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject CheckpointText;
    [SerializeField] GameObject levelUpText;
    [SerializeField] GameObject mainHUD;
    [SerializeField] GameObject knightHUD;
    [SerializeField] GameObject engineerHUD;
    [SerializeField] GameObject gunnerHUD;

    [SerializeField] GameObject ability1;
    [SerializeField] GameObject ability2;
    [SerializeField] GameObject ability3;
    [SerializeField] GameObject dashSmear;
    [SerializeField] GameObject criticalText;
    [SerializeField] GameObject criticalTextBorder;

    [SerializeField] GameObject damageNumPrefab;
    [SerializeField] GameObject chestDepositUI;

    [SerializeField] GameObject talk_portrait;
    [SerializeField] GameObject dialogue_box;
    [SerializeField] GameObject advance_textbox_obj;

    Coroutine currentCriticalOpacity;
    Coroutine currentCriticalBorderOpacity;
    Coroutine currentTransitionTypewriter;
    IEnumerator currentDialogueBox;
    Coroutine currentDialogueBoxAnimation;
    Coroutine currentBorderStretch;

    Slider currentAbilitySlider;

    [SerializeField] Slider ExperienceBar;
    GameObject currentCheckpointText;
    Queue<GameObject> currentSmears = new Queue<GameObject>();
    Queue<GameObject> currentDamageNums = new Queue<GameObject>();
    Queue<string> dialogueText = new Queue<string>();
    CharacterBase character;
    AudioManager audioManager;
    bool advanceTextbox = false;
    bool advanceLeadChar = false;
    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        knightHUD.SetActive(false);
        gunnerHUD.SetActive(false);
        engineerHUD.SetActive(false);
    }

    private void LateUpdate()
    {
        if (currentDamageNums.Count > 0)
        {
            foreach (var num in currentDamageNums)
            {
                //num.transform.position = new Vector3(num.transform.position.x, num.transform.position.y, num.transform.position.z);
            }
        }
    }

    public void ShowCriticalText()
    {
        criticalText.SetActive(true);
        criticalTextBorder.SetActive(true);
        StartCoroutine(AnimateCriticalText());
        StartCoroutine(AnimateCriticalTextBorder());
    }

    public void HideCriticalText()
    {
        StopCoroutine(AnimateCriticalText());
        StopCoroutine(AnimateCriticalTextBorder());
        StopCoroutine(currentCriticalOpacity);
        StopCoroutine(currentCriticalBorderOpacity);
        criticalText.SetActive(false);
        criticalTextBorder.SetActive(false);

    }

    public void DisplayDamageNum(Transform enemyTransform, float damage, float textSize = 40f, float rate = 2f)
    {
        var mainText = damageNumPrefab.GetComponent<TMP_Text>();
        mainText.text = damage.ToString();
        mainText.fontSize = textSize;
        var backdropText = damageNumPrefab.transform.Find("EnemyDamageNumberBackdrop").GetComponent<TMP_Text>();
        backdropText.text = damage.ToString();
        backdropText.fontSize = textSize;
        GameObject numRef = Instantiate(damageNumPrefab);
        var ui = GameObject.Find("Canvas");
        numRef.transform.SetParent(ui.transform, false);
        var screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(enemyTransform.position);
        numRef.transform.position = new Vector3(screenPos.x + Random.Range(-25.0f, 25.0f), screenPos.y + Random.Range(0.0f, 120.0f), screenPos.z);
        StartCoroutine(AnimateDamageNum(numRef, enemyTransform, rate));
        //currentDamageNums.Enqueue(numRef);
    }

    public void DisplayChestDepositUI()
    {

    }

    public void StartAnimateTypewriterScreenTransition(TMP_Text tmp_text, string text_to_animate, string leadingChar, float rate)
    {
        if (currentTransitionTypewriter != null) StopCoroutine(currentTransitionTypewriter);
        currentTransitionTypewriter = StartCoroutine(AnimateTypewriterScreenTransition(tmp_text, text_to_animate, leadingChar, rate));
    }

    public IEnumerator AnimateTypewriterScreenTransition(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f)
    {

        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator LoadDialogueBox(DialogueObject dialogueObject)
    {
        if (currentDialogueBox != null) UnloadDialogue();
        foreach (var text in dialogueObject.dialogueList)
        {
            dialogueText.Enqueue(text);
        }
        currentDialogueBoxAnimation = StartCoroutine(AnimateDialogueBoxMovement("left"));
        currentDialogueBox = AnimateTypewriterDialogue(GameObject.Find("DialogueText").GetComponent<TMP_Text>(), dialogueObject.leadingChar, dialogueObject.textRate, dialogueObject.stopPlayer);
        StartCoroutine(currentDialogueBox);
        //Debug.Log("Returned from dialogue coroutine");
        //dialogueObject.dialogueFinished = true;
        yield break;
    }

    public void UnloadDialogue()
    {
        if(currentDialogueBox != null) StopCoroutine(currentDialogueBox);
        if(currentDialogueBoxAnimation != null) StopCoroutine(currentDialogueBoxAnimation);
        GameObject static_portrait = GameObject.Find("Portrait_Static");
        talk_portrait.SetActive(false);
        static_portrait.SetActive(true);
        dialogueText.Clear();
        GameObject.Find("DialogueText").GetComponent<TMP_Text>().text = "";
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (character.inDialogueBox && context.performed)
        {
            advanceTextbox = true;
        }
    }

    public IEnumerator DialogueBoxTimeout()
    {

        //yield return new WaitForSeconds(0.2f);
        if(dialogueText.Count > 0)
        {
            yield break;
        }
        else
        {
           currentDialogueBoxAnimation = StartCoroutine(AnimateDialogueBoxMovement("right"));
        }
        yield break;
    }

    public IEnumerator AnimateDialogueBoxMovement(string direction)
    {
        Vector3 desiredPos = dialogue_box.transform.localPosition;
        if (direction == "left")
        {
            desiredPos = new Vector3(413, dialogue_box.transform.localPosition.y, dialogue_box.transform.localPosition.z);
            
        }
        else if(direction == "right")
        {
            desiredPos = new Vector3(1510, dialogue_box.transform.localPosition.y, dialogue_box.transform.localPosition.z);
        }
        while (dialogue_box.transform.localPosition != desiredPos)
        {
            if (Mathf.Abs(dialogue_box.transform.localPosition.magnitude - desiredPos.magnitude) <= 0.5f) dialogue_box.transform.localPosition = desiredPos;
            dialogue_box.transform.localPosition = Vector3.Lerp(dialogue_box.transform.localPosition, desiredPos, 2.75f * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    public IEnumerator AwaitDialogueBoxLeadChar()
    {
        yield return new WaitForSeconds(0.5f);
        advanceLeadChar = true;
        yield return null;
    }

    public void startBorderStretch()
    {
        currentBorderStretch = StartCoroutine(StretchBorderUI());
    }

    public void stopBorderStretch()
    {
        if (currentBorderStretch != null) StopCoroutine(currentBorderStretch);
        StartCoroutine(UnstretchBorderUI());
    }

    public IEnumerator StretchBorderUI()
    {
     
        var topGrad = GameObject.Find("TopGradient").GetComponent<RectTransform>();
        var topGrad2 = GameObject.Find("TopGradient2").GetComponent<RectTransform>();
        var botGrad = GameObject.Find("BottomGradient").GetComponent<RectTransform>();
        var botGrad2 = GameObject.Find("BottomGradient2").GetComponent<RectTransform>();
        float desiredAmount = topGrad.sizeDelta.y + 300;
        
        while(topGrad.sizeDelta.y != desiredAmount)
        {
            topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, topGrad.sizeDelta.y + 1250f*Time.deltaTime);
            topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, topGrad2.sizeDelta.y + 1250f * Time.deltaTime);
            botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, botGrad.sizeDelta.y + 1250f * Time.deltaTime);
            botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, botGrad2.sizeDelta.y + 1250f * Time.deltaTime);
            if(Mathf.Abs(topGrad.sizeDelta.y - desiredAmount) <= 25)
            {
                topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, desiredAmount);
                topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, desiredAmount);
                botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, desiredAmount);
                botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, desiredAmount);
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator UnstretchBorderUI()
    {
        var topGrad = GameObject.Find("TopGradient").GetComponent<RectTransform>();
        var topGrad2 = GameObject.Find("TopGradient2").GetComponent<RectTransform>();
        var botGrad = GameObject.Find("BottomGradient").GetComponent<RectTransform>();
        var botGrad2 = GameObject.Find("BottomGradient2").GetComponent<RectTransform>();
        float desiredAmount = 275.9484f;

        while (topGrad.sizeDelta.y != desiredAmount)
        {
            topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, topGrad.sizeDelta.y - 1250f * Time.deltaTime);
            topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, topGrad2.sizeDelta.y - 1250f * Time.deltaTime);
            botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, botGrad.sizeDelta.y - 1250f * Time.deltaTime);
            botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, botGrad2.sizeDelta.y - 1250f * Time.deltaTime);
            if (Mathf.Abs(topGrad.sizeDelta.y - desiredAmount) <= 25)
            {
                topGrad.sizeDelta = new Vector2(topGrad.sizeDelta.x, desiredAmount);
                topGrad2.sizeDelta = new Vector2(topGrad2.sizeDelta.x, desiredAmount);
                botGrad.sizeDelta = new Vector2(botGrad.sizeDelta.x, desiredAmount);
                botGrad2.sizeDelta = new Vector2(botGrad2.sizeDelta.x, desiredAmount);
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator AnimateTypewriterDialogue(TMP_Text tmp_text, string leadingChar = "", float rate = 0.25f, bool freezePlayer = false)
    {
        if (freezePlayer)
        {
            character.inDialogueBox = true;
            character.GetMasterInput().GetComponent<masterInput>().pausePlayerInput();
        }
        tmp_text.text = "";
        GameObject static_portrait = GameObject.Find("Portrait_Static");
        talk_portrait.SetActive(false);
        //advance_textbox_obj.SetActive(false);
        while (dialogueText.Count > 0)
        {
            var text_to_aniamte = dialogueText.Dequeue();
            advance_textbox_obj.SetActive(false);
            foreach (char c in text_to_aniamte)
            {
                if (freezePlayer && advanceTextbox)
                {
                    tmp_text.text = text_to_aniamte;
                    tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                    tmp_text.text += leadingChar;
                    advanceTextbox = false;
                    break;
                }
                if (c == ' ') talk_portrait.SetActive(false);
                else talk_portrait.SetActive(true);
                if (tmp_text.text.Length > 0)
                {
                    tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                }
                tmp_text.text += c;
                tmp_text.text += leadingChar;
                audioManager.PlaySFX("KeyTap");
                yield return new WaitForSeconds(rate);
            }
            int counter = 0;
            int counterLimit = 3;
            talk_portrait.SetActive(false);
            if(freezePlayer) advance_textbox_obj.SetActive(true);
            while (counter < counterLimit && !advanceTextbox)
            {
                advanceLeadChar = false;
                if (advanceTextbox) break;
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
                StartCoroutine(AwaitDialogueBoxLeadChar());
                while (!advanceLeadChar)
                {
                    if (advanceTextbox) break;
                    yield return null;
                }
                advanceLeadChar = false;
                if(!freezePlayer) counter++;
                tmp_text.text += leadingChar;
                if (advanceTextbox) break;
                StartCoroutine(AwaitDialogueBoxLeadChar());
                while (!advanceLeadChar)
                {
                    if (advanceTextbox) break;
                    yield return null;
                }
                yield return null;
            }
            advanceTextbox = false;
            //if (!freezePlayer) yield return new WaitForSeconds(0.75f);

            if(dialogueText.Count > 0) tmp_text.text = "";
        }
        if (freezePlayer)
        {
            character.inDialogueBox = false;
            character.GetMasterInput().GetComponent<masterInput>().resumePlayerInput();
        }
        StartCoroutine(DialogueBoxTimeout());
        Debug.Log("Finished dialogue box");
        yield break;
        //if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator AnimateTypewriterDeathscreen(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f)
    {
        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(0.5f);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(0.5f);
            //yield return null;
        }
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
        yield break;
    }

    public IEnumerator AnimateTypewriterCheckpoint(TMP_Text tmp_text, string text_to_animate, string leadingChar = "", float rate = 0.25f)
    {

        tmp_text.text = "";
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text += c;
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        int counter = 0;
        while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(0.5f);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(0.5f);
            //yield return null;
        }
        foreach (char c in text_to_animate)
        {
            if (tmp_text.text.Length > 0)
            {
                tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            }
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - 1);
            tmp_text.text += leadingChar;
            audioManager.PlaySFX("KeyTap");
            yield return new WaitForSeconds(rate);
        }
        counter = 0;
        /*while (leadingChar != "" && counter < 2)
        {
            tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
            yield return new WaitForSeconds(rate * 2);
            counter++;
            tmp_text.text += leadingChar;
            yield return new WaitForSeconds(rate * 2);
            //yield return null;
        }*/
        if (leadingChar != "") tmp_text.text = tmp_text.text.Substring(0, tmp_text.text.Length - leadingChar.Length);
    }

    public IEnumerator AnimateDamageNum(GameObject num, Transform enemyTrans, float rate)
    {
        var text = num.GetComponent<TMP_Text>();
        var textBackdrop = num.transform.GetChild(0).GetComponent<TMP_Text>();
        //textBackdrop.color 
        float xOffset = Random.Range(-100.0f, 0.0f);
        float yOffset = Random.Range(0.0f, 120.0f);
        Vector3 screenPos = Vector3.zero;
        
        while (text.color.a > 0)
        {
            xOffset += 100f * Time.deltaTime;
            if(enemyTrans != null) screenPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(enemyTrans.position);
            num.transform.position = new Vector3((screenPos.x + xOffset), (screenPos.y + yOffset), screenPos.z);
            Color txtColor = text.color;
            Color txtBackdropColor = textBackdrop.color;
            txtColor.a -= rate * Time.deltaTime;
            txtBackdropColor.a -= rate * Time.deltaTime;
            text.color = txtColor;
            textBackdrop.color = txtBackdropColor;
            yield return null;
        }
        Destroy(num);
        yield break;
    }

    public IEnumerator AnimateCriticalTextBorder()
    {
        while (true)
        {
            yield return currentCriticalBorderOpacity = StartCoroutine(IncreaseTextOpacity(criticalTextBorder, 1.0f));
            yield return currentCriticalBorderOpacity = StartCoroutine(ReduceTextOpacity(criticalTextBorder, 1.0f));
        }
    }

    public IEnumerator AnimateCriticalText()
    {
        
        while (true)
        {
            yield return currentCriticalOpacity = StartCoroutine(IncreaseTextOpacity(criticalText, 1.0f));
            yield return currentCriticalOpacity = StartCoroutine(ReduceTextOpacity(criticalText, 1.0f)); 
        }
        
    }

    public void Initialize()
    {
        ExperienceBar.value = character.weaponClass.totalExp;
        UpdateClass(character.weaponClass.classType, character.weaponClass.currentLvl);
        //UpdateExperienceBar(character.weaponClass.getCurrentLvlExperienceAmount(), character.weaponClass.getNextLvlExperienceAmount(), character.weaponClass.totalExp);
    }

    public void InstantiateSmear(float angle)
    {
        var currentSmear = Instantiate(dashSmear);
        currentSmear.transform.SetParent(GameObject.Find("SplashCanvas").transform, false);
        currentSmear.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 0.15f, character.transform.position.z);
        currentSmear.transform.rotation = Quaternion.Euler(currentSmear.transform.eulerAngles.x, currentSmear.transform.eulerAngles.y, -angle);
        StartCoroutine(IncreaseImageOpacity(currentSmear, 4f));
        currentSmears.Enqueue(currentSmear);
    }

    public void DestroyOldestSmear()
    {
        if(currentSmears.Count > 0)
        {
            StartCoroutine(AnimateDestroyOldestSmear(currentSmears.Dequeue()));
        }   
    }
    public IEnumerator AnimateDestroyOldestSmear(GameObject smear)
    {
        yield return StartCoroutine(DecreaseImageOpacity(smear, 3.0f));
        Destroy(smear);
        
    }



    public void ActivateCooldownOnAbility(int abilityNum)
    {
        GameObject ability = null;
        switch (abilityNum)
        { 
            case 1:
                ability = ability1;
                
                break;
            case 2:
                ability = ability2;
                break;
            case 3:
                ability = ability3;
                break;
        }
        ability.transform.Find("AbilityCooldownFill").gameObject.SetActive(true);
        ability.transform.Find("AbilityBorderCooldown").gameObject.SetActive(true);
        ability.transform.Find("AbilityBorderNormal").gameObject.SetActive(false);
        ability.transform.Find("AbilityCooldownBorder").gameObject.SetActive(false);
        ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.SetActive(true);


        var reducedAlpha = ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color;
        reducedAlpha.a = 0.5f;
        ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color = reducedAlpha;

        reducedAlpha = ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color;
        reducedAlpha.a = 0.5f;
        ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color = reducedAlpha;

        //reducedAlpha = ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.GetComponent<Image>().color;
        //reducedAlpha.a = 0.5f;
        //ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.GetComponent<Image>().color = reducedAlpha;
    }

    public IEnumerator AnimateCooldownSlider(Slider currentAbilitySlider, float rate)
    {
        
        while(currentAbilitySlider != null && currentAbilitySlider.value != 1)
        {
            currentAbilitySlider.value += rate * Time.deltaTime;
            if (Mathf.Abs(currentAbilitySlider.value - 1) <= 0.01) currentAbilitySlider.value = 1;
            yield return null;
        }
        yield break;
    }

    public IEnumerator StartCooldownSlider(int abilityNum, float rate)
    {
        Slider currentAbilitySlider = null;
        switch (abilityNum)
        {
            case 1:
                currentAbilitySlider = ability1.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                break;
            case 2:
                currentAbilitySlider = ability2.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                break;
            case 3:
                currentAbilitySlider = ability3.transform.Find("AbilityCooldownFill").GetComponent<Slider>();
                break;
        }
        yield return StartCoroutine(AnimateCooldownSlider(currentAbilitySlider, rate));
    }

    public void DeactivateCooldownOnAbility(int abilityNum)
    {
        GameObject ability = null;
        switch (abilityNum)
        {
            case 1:
                ability = ability1;
                break;
            case 2:
                ability = ability2;
                break;
            case 3:
                ability = ability3;
                break;
        }
        ability.transform.Find("AbilityCooldownFill").gameObject.GetComponent<Slider>().value = 0.0f;
        ability.transform.Find("AbilityCooldownFill").gameObject.SetActive(false);
        ability.transform.Find("AbilityBorderCooldown").gameObject.SetActive(false);
        ability.transform.Find("AbilityBorderNormal").gameObject.SetActive(true);
        ability.transform.Find("AbilityCooldownBorder").gameObject.SetActive(true);
        ability.transform.Find("AbilityCooldownBorderDeactivated").gameObject.SetActive(false);

        var increasedAlpha = ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color;
        increasedAlpha.a = 1.0f;
        ability.transform.Find("AbilityIcon").gameObject.GetComponent<Image>().color = increasedAlpha;

        increasedAlpha = ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color;
        increasedAlpha.a = 1.0f;
        ability.transform.Find("AbilityNumber").gameObject.GetComponent<TMP_Text>().color = increasedAlpha;

        //increasedAlpha = ability.transform.Find("AbilityCooldownBorder").gameObject.GetComponent<Image>().color;
        //increasedAlpha.a = 1.0f;
        //ability.transform.Find("AbilityCooldownBorder").gameObject.GetComponent<Image>().color = increasedAlpha;
    }
    
    public void UpdateClass(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
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
        UpdateExperienceLevel(weaponClass, experienceLVL, changingClass);
        
    }

    public void UpdateExperienceBar(float current)
    {
        
        StartCoroutine(AnimateExperienceBar(current));
        //ExperienceBar.value = current;
    }

    public IEnumerator AnimateExperienceBar(float current)
    {
        while (ExperienceBar.value < current)
        {
            ExperienceBar.value = Mathf.Lerp(ExperienceBar.value, current, 3.0f * Time.deltaTime);
            if (Mathf.Abs(ExperienceBar.value - current) < 0.5) ExperienceBar.value = current;
            if (Mathf.Abs(ExperienceBar.value - ExperienceBar.maxValue) < 0.5) yield break;
            yield return null;
        }
        Debug.Log("Bar finished in AnimateExperienceBar");
        yield break;
    }

    public void ChangeExperienceLvl(float min, float max)
    {
        ExperienceBar.minValue = min;
        ExperienceBar.maxValue = max;
    }
    

    public IEnumerator AnimateExperienceUpdate(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
    {
        if (!changingClass)
        {
            yield return StartCoroutine(AnimateExperienceBar(character.weaponClass.getCurrentLvlExperienceAmount()));
            Debug.Log("Bar finished in AnimateExperienceUpdate");
        }
        

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
        ChangeExperienceLvl(character.weaponClass.getCurrentLvlExperienceAmount(), character.weaponClass.getNextLvlExperienceAmount());
        if (!changingClass)
        {
            yield return StartCoroutine(AnimateExperienceBar(character.weaponClass.totalExp));
        }
        else
        {
            ExperienceBar.value = character.weaponClass.totalExp;
        }
        yield break;
        //
        
    }

    public void UpdateExperienceLevel(WeaponBase.weaponClassTypes weaponClass, int experienceLVL, bool changingClass = false)
    {
        StartCoroutine(AnimateExperienceUpdate(weaponClass, experienceLVL, changingClass));
       
    }

    public void StartLevelUpText()
    {
        StartCoroutine(AnimateLevelUpText());
    }

    public IEnumerator AnimateLevelUpText()
    {
        Vector3 ogPosition = levelUpText.transform.localPosition;
        levelUpText.transform.localPosition = new Vector3(levelUpText.transform.localPosition.x - 2000.0f, levelUpText.transform.localPosition.y, levelUpText.transform.localPosition.z);
        
        bool increasingOpacity = false;
        while (levelUpText.transform.localPosition.x < ogPosition.x)
        {
             if((levelUpText.transform.localPosition.x > ogPosition.x - 1000.0f) && !increasingOpacity)
            {
                StartCoroutine(IncreaseImageOpacity(levelUpText, 4.0f));
            }
             if(Mathf.Abs(levelUpText.transform.localPosition.x - ogPosition.x) < 0.3f){
                levelUpText.transform.localPosition = ogPosition;
             }
             else
             {
                levelUpText.transform.localPosition = Vector3.Lerp(levelUpText.transform.localPosition, ogPosition, 15.0f*Time.deltaTime);
             }
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(DecreaseImageOpacity(levelUpText, 1.0f));
        yield break;
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
        yield return StartCoroutine(AnimateTypewriterCheckpoint(currentCheckpointText.GetComponent<TMP_Text>(), "Checkpoint Reached", "|", 0.125f));
        Destroy(currentCheckpointText);
        yield break;
    }

    private IEnumerator DecreaseImageOpacity(GameObject image, float rate)
    {
        var reference = image.GetComponent<Image>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator IncreaseImageOpacity(GameObject image, float rate)
    {
        var reference = image.GetComponent<Image>();
        while (reference.color.a < 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator ReduceTextOpacity(GameObject text, float rate)
    {
        var reference = text.GetComponent<TMP_Text>();
        while (reference.color.a >= 0.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a -= rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

    private IEnumerator IncreaseTextOpacity(GameObject text, float rate)
    {
        var reference = text.GetComponent<TMP_Text>();
        while (reference.color.a <= 1.0 && reference != null)
        {
            Color imgColor = reference.color;
            imgColor.a += rate * Time.deltaTime;
            reference.color = imgColor;

            yield return null;
        }
        yield break;
    }

}
