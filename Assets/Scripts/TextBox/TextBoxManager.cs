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
    }
    public void ReloadScript(TextAsset newText)
    {
        if (newText != null)
        {
            textLines = new string[1];
            textLines = textLines = (newText.text.Split('\n'));
        }
    }
}
