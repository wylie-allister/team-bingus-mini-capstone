using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactPopup : MonoBehaviour
{
    public string[] wwfFacts;
    int factIndex = 0;

    public TextMeshProUGUI factText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FactRandomizer()
    {
        factIndex = Random.Range(0, wwfFacts.Length);

        factText.text = wwfFacts[factIndex];
    }
}
