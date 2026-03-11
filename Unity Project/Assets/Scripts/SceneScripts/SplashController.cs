using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    public Image blackOverlay;

    private float overlayTimer = 0;

    public float stayTime = 2.0f;

    public float introTime = 3.0f;

    public float exitTime = 1.5f;

    private Color blackNoAlpha = new Color(0, 0, 0, 0);
    public float transitionScalar = 0.75f;
    
    void Start()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;
        
        overlayTimer = 0;
        blackOverlay.color = Color.black;
    }

    void Update()
    {
        overlayTimer += Time.deltaTime;
        
        if (overlayTimer < introTime)
        {
            blackOverlay.color = Color.Lerp(blackOverlay.color, blackNoAlpha, Time.deltaTime * transitionScalar);
        }
        else if (overlayTimer < introTime + stayTime)
        {
            blackOverlay.color = blackNoAlpha;
        }
        else if (overlayTimer > introTime + stayTime  && overlayTimer < introTime + stayTime + exitTime)
        {
            blackOverlay.color = Color.Lerp(blackOverlay.color, Color.black, Time.deltaTime * 2f * transitionScalar);
        }

        if (overlayTimer > introTime + stayTime + exitTime)
        {
            blackOverlay.color = Color.black;
            
        }

        if (overlayTimer > introTime + stayTime + exitTime + 1.0f)
        {
            SceneController.Instance.GoToStartMenu();
        }
        
        
    }
}
