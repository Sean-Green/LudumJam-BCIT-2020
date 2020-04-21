using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public GameObject panel;
    public Text textBoxInPanel;
    public TextAsset textFile;
    public string[] textLines;
    // Start is called before the first frame update
    public int currentLine;
    public int endAtLine;
    public bool isActive;
    public bool stopPlayerMovement;

    public PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (textFile != null){
            textLines = (textFile.text.Split('\n'));
        }
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
        if (isActive)
        {
            EnableTextBox();
        } else {
            DisableTextBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        textBoxInPanel.text = textLines[currentLine];

        if(Input.GetKeyDown(KeyCode.X))
        {
            currentLine += 1;
        }
        if(currentLine > endAtLine)
        {
            DisableTextBox();
        }
    }
    public void EnableTextBox()
    {
        panel.SetActive(true);
        isActive = true;
        if(stopPlayerMovement)
        {
            player.canMove = false;
        }
    }
    public void DisableTextBox()
    {
        isActive = false;
        panel.SetActive(false);
        player.canMove = true;
        currentLine = 0;
    }
    public void ReloadScript(TextAsset newText)
    {
        if (newText != null)
        {
            textLines = new string[1];
            textLines = textLines = (newText.text.Split('\n'));
        }
    }
    // pass in a .txt, the first and last line you want read from it, and the manager will display a textBox with that info.
    public void Display(TextAsset newText, int start, int finish)
    {
        ReloadScript(newText);
        currentLine = start;
        endAtLine = finish;
        EnableTextBox();
    }
    /* Pass in a .txt and we will display all of it in a text box.*/
    public void Display(TextAsset newText)
    {
        Display(newText, 0, 0);
    }
    public void DisplayOneLine(string line)
    {
        textLines = new string[1];
        textLines[1] = line;
        currentLine = 0;
        endAtLine = 1;
        EnableTextBox();
    }
}
