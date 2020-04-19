using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour
{
    public TextAsset theText;

    public int startLine;
    public int endLine;
    public TextBoxManager textBox;
    // Start is called before the first frame update
    public bool destroyWhenActivated;
    public bool requireButtonPress;
    private bool waitForPress;
    private bool readyToEnable;
    void Start()
    {
        textBox = FindObjectOfType<TextBoxManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if(waitForPress && Input.GetKeyDown(KeyCode.X))
        {
            textBox.ReloadScript(theText);
            textBox.currentLine = startLine;
            textBox.endAtLine = endLine;
            waitForPress = false;
            readyToEnable = true;


            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }

        if (readyToEnable && Input.GetKeyUp(KeyCode.X))
        {
            textBox.EnableTextBox();
            readyToEnable = false;
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "SimplerPlayer")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }
            textBox.ReloadScript(theText);
            textBox.currentLine = startLine;
            textBox.endAtLine = endLine;
            textBox.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }
    void OntriggerExit2D(Collider2D other)
    {
        if (other.name == "SimplerPlayer")
        {
            waitForPress = false;
        }
    }
}
