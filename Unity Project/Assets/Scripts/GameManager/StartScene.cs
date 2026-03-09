using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    // Here we get the move action for vertical selection - horzontal is not needed for alpha
    // JumpAction input reference is just a neat selection key
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    
    private int selectionIndex = 0;

    public float selectedFontSize;
    public float deselectedFontSize;
    
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private TextMeshProUGUI optionsText;
    [SerializeField] private TextMeshProUGUI quitText;
    private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();

    private bool canNavigate = true;
    
    void Start()
    {
        texts.Add(startText);
        texts.Add(optionsText);
        texts.Add(quitText);
    }

    void Update()
    {
        // Get input, handle selection and text
        float vertInput = moveAction.action.ReadValue<Vector2>().y;
        HandleNavigation(vertInput);
        HandleSelection();
        HandleText();
    }
    
    // We use a bool (canNavigate) to ensure we dont move every frame
    // Input is a float and this is the simplest I can think of doing this rn -BC
    private void HandleNavigation(float input)
    {
        // If theres no input, set navigate to true and break logic
        if (input == 0)
        {
            canNavigate = true;
            return;
        }

        // If we receive an up input, decrement selection index and bool-break from logic
        if (input > 0 && selectionIndex > 0 && canNavigate)
        {
            selectionIndex--;
            canNavigate = false;
        }
        // If we receive a down input, increment selection index and bool-break from logic
        else if (input < 0 && selectionIndex < texts.Count - 1 && canNavigate)
        {
            selectionIndex++;
            canNavigate = false;
        }
    }

    private void HandleSelection()
    {
        // If selection has not been made, break from logic
        if (!jumpAction.action.WasPressedThisFrame())
            return;
        
        // If selection is made, proceed with current selection variable
        switch (selectionIndex)
        {
            // Start selection
            case 0:
                //Debug.Log("GOTO GAMEPLAY SCENE");
                SceneController.Instance.GoToGameplay();
                break;
            // Options selection
            case 1:
                //Debug.Log("GOTO OPTIONS");
                break;
            // Quit selection
            case 2:
                //Debug.Log("QUIT");
                Application.Quit();
                break;
        }
    }

    private void HandleText()
    {
        // Loop through our texts
        for (int currentTextIndex = 0; currentTextIndex < texts.Count; currentTextIndex++)
        {
            // If we are on the currently selected text, increase fontsize
            if (currentTextIndex == selectionIndex)
            {
                texts[currentTextIndex].fontSize = selectedFontSize;
                continue;
            }

            // If we are not on the currently selected text, decrease fontsize
            if (texts[currentTextIndex].fontSize != deselectedFontSize)
            {
                texts[currentTextIndex].fontSize = deselectedFontSize;
            }
        }
    }
}
