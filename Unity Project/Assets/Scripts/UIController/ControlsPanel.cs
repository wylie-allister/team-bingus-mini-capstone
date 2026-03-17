using UnityEngine;

// Attach to the Controls Panel GameObject in the StartScene canvas.
// The panel should contain an Image with the controllerControls sprite.
// StartScene.cs calls Open() / Close() on this.
public class ControlsPanel : MonoBehaviour
{
    public static ControlsPanel Instance;

    public bool isOpen { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Start hidden
        gameObject.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }
}
