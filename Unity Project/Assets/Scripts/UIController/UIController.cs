using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider throwSlider;

    
    void Start()
    {
        
    }

    void Update()
    {
        throwSlider.value = NormalizeSliderValue(GameManager.Instance.itemController.throwForce, 0, GameManager.Instance.itemController.throwForceMax);
    }
    
    private float NormalizeSliderValue(float value, float min, float max)
    {
        if (max == min)
        {
            return 0.0f;
        }

        return (value - min) / (max - min);
    }
}
