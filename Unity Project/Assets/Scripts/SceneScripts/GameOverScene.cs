using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverScene : MonoBehaviour
{
    public InputActionReference jumpAction;
    public InputActionReference roarAction;

    [Header("Lose Panel")]
    // The existing caught log object already in the scene
    public GameObject playerLoseText;

    [Header("Win Panel")]
    // Duplicate of the lose panel reskinned for win - assign in editor
    public GameObject playerWinObject;
    // Optional: TMP inside the win object for the header text
    public TextMeshProUGUI winHeaderText;
    // Optional: TMP inside the win object for tree count
    public TextMeshProUGUI treeCountWinText;

    [Header("Tree Count (fallback Win display)")]
    // Original tree count texts - still shown if no winObject is assigned
    public TextMeshProUGUI treeCountTopText;
    public TextMeshProUGUI treeCountText;

    [Header("Scrolling Facts")]
    public TextMeshProUGUI fact1Text;
    public TextMeshProUGUI fact2Text;
    public string[] wwfFacts;
    public float factScrollSpeed = 10;

    public Animator canvasAnimator;

    // Optional: TMP inside playerLoseText to show the explicit reason
    [Header("Optional Explicit Reason (Lose)")]
    public TextMeshProUGUI loseReasonText;

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

        roarAction.action.started += ctx => slowText = true;
        roarAction.action.canceled += ctx => slowText = false;

        // Read lose state - guard in case GameState hasn't initialised yet
        bool didLose = GameState.Instance != null && GameState.Instance.didPlayerLose;

        if (didLose)
        {
            // Show caught panel, hide everything win-related
            playerLoseText.SetActive(true);

            if (playerWinObject != null) playerWinObject.SetActive(false);
            if (treeCountText != null)    treeCountText.gameObject.SetActive(false);
            if (treeCountTopText != null) treeCountTopText.gameObject.SetActive(false);

            // Fill in explicit reason if the text field is wired up
            if (loseReasonText != null)
            {
                string reason = GameState.Instance.endReason;
                loseReasonText.text = reason != "" ? reason : "You were spotted too many times!";
            }
        }
        else
        {
            // Hide caught panel
            playerLoseText.SetActive(false);

            if (playerWinObject != null)
            {
                // New win panel is set up - use it
                playerWinObject.SetActive(true);

                if (treeCountText != null)    treeCountText.gameObject.SetActive(false);
                if (treeCountTopText != null) treeCountTopText.gameObject.SetActive(false);

                if (winHeaderText != null)
                    winHeaderText.text = "THE CREW FLED!";

                // Show tree count inside the win panel if wired up
                if (treeCountWinText != null)
                    treeCountWinText.text = "Trees Saved: " + PlayerPrefs.GetInt("TreesSaved").ToString();
            }
            else
            {
                // Fallback: no win panel built yet, show original tree count texts
                if (treeCountText != null)
                {
                    treeCountText.text = PlayerPrefs.GetInt("TreesSaved").ToString();
                    treeCountText.gameObject.SetActive(true);
                }
                if (treeCountTopText != null) treeCountTopText.gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
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
