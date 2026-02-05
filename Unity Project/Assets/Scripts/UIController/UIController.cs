using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Throw Slider")]
    public bool throwSliderEnabled = true;
    public Slider throwSlider;


    void Update()
    {
        UpdateSliderActivity();
        UpdateSliderValues();
    }

    void UpdateSliderActivity()
    {
        // Enable / Disable UI elements as needed
        throwSliderEnabled = GameManager.Instance.itemController.isThrowing;
        throwSlider.gameObject.SetActive(throwSliderEnabled);
    }
    
    void UpdateSliderValues()
    {
        
        if (throwSliderEnabled)
            throwSlider.value = NormalizeSliderValue(GameManager.Instance.itemController.throwForce, 0, GameManager.Instance.itemController.throwForceMax);

        
    }
    
    // Helper function to normalize slider values between 0 and 1
    // Min and max variables are relative to the value that is being normalized
    // e.g. throw is a value between 0 and a given max || NSV(throwval, 0, throwvalmax)
    private float NormalizeSliderValue(float value, float min, float max)
    {
        // Return 0 if applicable
        if (max == min)
        {
            return 0.0f;
        }

        // Else return a value between 0 and 1
        return (value - min) / (max - min);
    }
}
