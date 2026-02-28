using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Throw Element")]
    public bool throwSliderEnabled = true;
    public Slider throwSlider;
    public GameObject canIconObject;
    
    [Header("Game Time Element")]
    public Slider gameTimeSlider;

    [Header("Tree Count Element")] 
    public TextMeshProUGUI treeCountText;
    private int treeCount = 0;

    [Header("Roar Element")] 
    public Slider roarSlider;

    [Header("Stamina Element")] 
    public Slider staminaSlider;
    

    void Update()
    {
        UpdateSliderActivity();
        UpdateSliderValues();
        HandleTreeCount();
        HandleItemIcon();
    }


    void HandleItemIcon()
    {
        if (GameManager.Instance.itemController.currentThrowable == null)
        {
            canIconObject.SetActive(false);
        }
        else
        {
            canIconObject.SetActive(true);
        }
        
    }
    
    void UpdateSliderActivity()
    {
        // Enable / Disable UI elements as needed
        throwSliderEnabled = GameManager.Instance.itemController.isThrowing && GameManager.Instance.itemController.currentThrowable != null;
        throwSlider.gameObject.SetActive(throwSliderEnabled);
    }
    
    void UpdateSliderValues()
    {
        // Set throw slider to player throw force
        if (throwSliderEnabled)
            throwSlider.value = NormalizeSliderValue(GameManager.Instance.itemController.throwForce, 0, GameManager.Instance.itemController.throwForceMax);

        // Set game timer slider to proper value
        gameTimeSlider.value =
            NormalizeSliderValue(GameManager.Instance.timeRemaining, 0, GameManager.Instance.maxGameTime);

        roarSlider.value = NormalizeSliderValue(GameManager.Instance.roarController.roarCharge, 0, GameManager.Instance.roarController.roarMaxCharge);
        
        staminaSlider.value = NormalizeSliderValue(GameManager.Instance.player.GetComponent<PlayerMovement>().currentStamina, 0, GameManager.Instance.player.GetComponent<PlayerMovement>().maxStamina);
    }

    private void HandleTreeCount()
    {
        treeCountText.text = treeCount.ToString();
    }
    
    // Helper function to normalize slider values between 0 and 1
    // Min and max variables are relative to the value that is being normalized
    // e.g. throw is a value between 0 and a given max || NSV(throwval, 0, throwvalmax)
    private float NormalizeSliderValue(float value, float min, float max)
    {
        // Return 0 if applicable
        if (max == min)
            return 0.0f;

        // Else return the normalized value between 0 and 1
        return (value - min) / (max - min);
    }

    public void IncreaseTreeCount()
    {
        treeCount++;
    }
}
