using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    public static ControlsPanel Instance;

    public bool isOpen = false;

    public GameObject controlsPanelObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        controlsPanelObject.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        controlsPanelObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        controlsPanelObject.SetActive(false);
    }

    public void Toggle()
    {
        isOpen = !isOpen;
        controlsPanelObject.SetActive(isOpen);
        
    }
}
