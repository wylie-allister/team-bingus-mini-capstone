using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverScene : MonoBehaviour
{
    public InputActionReference jumpAction;
    public InputActionReference roarAction;

    [Header("Win Screen")]
    public GameObject winScreen;
    public TextMeshProUGUI winHeaderText;
    public TextMeshProUGUI treeCountTopText;
    public TextMeshProUGUI treeCountText;

    [Header("Lose Screen")]
    public GameObject loseScreen;
    public TextMeshProUGUI loseHeaderText;
    public TextMeshProUGUI loseReasonText;

    [Header("Shared - Scrolling Facts")]
    public TextMeshProUGUI fact1Text;
    public TextMeshProUGUI fact2Text;
    public string[] wwfFacts;
    public float factScrollSpeed = 10;

    public Animator canvasAnimator;

    private bool slowText = false;
    private float currentTextSpeed;

    private bool isFact1OnScreen = false;
    private bool isFact2OnScreen = false;

    private int fact1Index = 0;
    private int fact2Index = 0;

    private float canvasAnimationTimer = 0.0f;
    private bool activeAnimationTimer = true;

    void Start()
    {
        currentTextSpeed = factScrollSpeed;

        // Hold roar to slow the scrolling facts
        roarAction.action.started += ctx => slowText = true;
        roarAction.action.canceled += ctx => slowText = false;

        bool didLose = GameState.Instance.didPlayerLose;
        string reason = GameState.Instance.endReason;

        if (didLose)
        {
            // Show lose screen, hide win screen
            loseScreen.SetActive(true);
            winScreen.SetActive(false);

            loseHeaderText.text = "CAUGHT!";
            loseReasonText.text = reason != "" ? reason : "You were spotted too many times!";
        }
        else
        {
            // Show win screen, hide lose screen
            winScreen.SetActive(true);
            loseScreen.SetActive(false);

            winHeaderText.text = "THE CREW FLED!";
            treeCountText.text = PlayerPrefs.GetInt("TreesSaved").ToString();
        }
    }

    void Update()
    {
        // Press Jump/Confirm to return to splash
        if (jumpAction.action.WasPressedThisFrame())
        {
            SceneController.Instance.GoToSplash();
        }

        if (activeAnimationTimer)
        {
            canvasAnimationTimer += Time.deltaTime;
        }

        currentTextSpeed = slowText ? factScrollSpeed * 0.5f : factScrollSpeed;

        if (canvasAnimationTimer >= 1.0f)
        {
            canvasAnimator.SetBool("StartAnimation", true);
            activeAnimationTimer = false;
            canvasAnimationTimer = 0.0f;
        }

        HandleFact1();
        HandleFact2();
    }

    private void HandleFact1()
    {
        if (!isFact1OnScreen)
        {
            GetNewFactIndex(1);
            fact1Text.text = wwfFacts[fact1Index];
            isFact1OnScreen = true;
        }

        if (fact1Text.rectTransform.anchoredPosition.y > 1000)
        {
            isFact1OnScreen = false;
            fact1Text.rectTransform.anchoredPosition = new Vector2(fact1Text.rectTransform.anchoredPosition.x, -900);
        }

        fact1Text.rectTransform.anchoredPosition = new Vector2(
            fact1Text.rectTransform.anchoredPosition.x,
            fact1Text.rectTransform.anchoredPosition.y + (currentTextSpeed * Time.deltaTime));
    }

    private void HandleFact2()
    {
        if (!isFact2OnScreen)
        {
            GetNewFactIndex(2);
            fact2Text.text = wwfFacts[fact2Index];
            isFact2OnScreen = true;
        }

        if (fact2Text.rectTransform.anchoredPosition.y > 1000)
        {
            isFact2OnScreen = false;
            fact2Text.rectTransform.anchoredPosition = new Vector2(fact2Text.rectTransform.anchoredPosition.x, -900);
        }

        fact2Text.rectTransform.anchoredPosition = new Vector2(
            fact2Text.rectTransform.anchoredPosition.x,
            fact2Text.rectTransform.anchoredPosition.y + (currentTextSpeed * Time.deltaTime));
    }

    private void GetNewFactIndex(int factNumber)
    {
        switch (factNumber)
        {
            case 1:
                if (fact1Index == fact2Index)
                {
                    fact1Index++;
                    if (fact1Index >= wwfFacts.Length)
                        fact1Index = 0;
                }
                else
                {
                    fact1Index = Random.Range(0, wwfFacts.Length);
                }
                break;
            case 2:
                if (fact2Index == fact1Index)
                {
                    fact2Index++;
                    if (fact2Index >= wwfFacts.Length)
                        fact2Index = 0;
                }
                else
                {
                    fact2Index = Random.Range(0, wwfFacts.Length);
                }
                break;
        }
    }
}
