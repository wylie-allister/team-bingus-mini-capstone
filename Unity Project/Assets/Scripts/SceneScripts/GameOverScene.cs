using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverScene : MonoBehaviour
{
    public InputActionReference jumpAction;
    public InputActionReference roarAction;

    public TextMeshProUGUI treeCountTopText;
    public TextMeshProUGUI treeCountText;
    public TextMeshProUGUI fact1Text;
    public TextMeshProUGUI fact2Text;

    private bool slowText = false;
    
    public string[] wwfFacts;

    private bool isFact1OnScreen = false;

    private bool isFact2OnScreen = false;
    public float factScrollSpeed = 10;
    private float currentTextSpeed;

    private int fact1Index = 0;
    private int fact2Index = 0;

    public Animator canvasAnimator;
    private float canvasAnimationTimer = 0.0f;
    private bool activeAnimationTimer = true;


    public GameObject playerLoseText;
    
    // Start is called before the first frame update
    void Start()
    {
        playerLoseText.SetActive(false);
        
        currentTextSpeed = factScrollSpeed;
        treeCountText.text = PlayerPrefs.GetInt("TreesSaved").ToString();
        
        roarAction.action.started += ctx => slowText = true;
        roarAction.action.canceled += ctx => slowText = false;

        if (DEBUG.Instance.didPlayerLose)
        {
            playerLoseText.SetActive(true);
            treeCountText.gameObject.SetActive(false);
            treeCountTopText.gameObject.SetActive(false);
        }
        else
        {
            playerLoseText.SetActive(false);
            treeCountText.gameObject.SetActive(true);
            treeCountTopText.gameObject.SetActive(true);
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

        if (slowText)
        {
            currentTextSpeed = factScrollSpeed * 0.5f;
        }
        else
        {
            currentTextSpeed = factScrollSpeed;
        }
        
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
            fact1Text.rectTransform.anchoredPosition = new Vector2(fact1Text.rectTransform.anchoredPosition.x,
                -900);
        }
        
        fact1Text.rectTransform.anchoredPosition = new Vector2(fact1Text.rectTransform.anchoredPosition.x,
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
            fact2Text.rectTransform.anchoredPosition = new Vector2(fact2Text.rectTransform.anchoredPosition.x,
                -900);
        }
        
        fact2Text.rectTransform.anchoredPosition = new Vector2(fact2Text.rectTransform.anchoredPosition.x,
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
                    if (fact1Index > wwfFacts.Length)
                    {
                        fact1Index = 0;
                    }
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
                    if (fact2Index > wwfFacts.Length)
                    {
                        fact2Index = 0;
                    }
                }
                else
                {
                    fact2Index = Random.Range(0, wwfFacts.Length);
                }
                break;
        }
    }
}
