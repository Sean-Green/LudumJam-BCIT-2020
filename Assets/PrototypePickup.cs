using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePickup : MonoBehaviour
{
    public TextBoxManager textBox;



    // Start is called before the first frame update
    void Start()
    {
        textBox = FindObjectOfType<TextBoxManager>();    
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OntTriggerStay2D()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("I feel you.");
            textBox.DisplayOneLine("You got it");
            Destroy(this);
        }
    }
}
