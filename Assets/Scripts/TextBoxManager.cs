using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Pefab for managing text interactions. 
    Add it to your scene, have NPC scripts call .EnableTextBox(TextAsset) function
    on the object. The text file you hand it will be parsed into the array, 
    and called everytime you hit spacebar.
    */
public class TextBoxManager : MonoBehaviour
{
    public GameObject textBox;

    public Text theText;

    //If you want text to run on the screen at scene start,
    //Drop a textfile in to this slot in the editor,
    //and activate the active checkbox
    public TextAsset startText; 

    //array that holds out lines of text, 
    // will be parsed line by line from the text file
    public string[] textLines;

    // the line of text being read
    private int currentLine;
    // the final line of text to be read
    private int endAtLine;

    // a reference to the player, so we can stop their movement.
    public PlayerController player;

    //For textbox at scene start check this box in the editor
    public bool active;

    void Start()
    {
        // get the player reference
        player = FindObjectOfType<PlayerController>();

        // if active, creates a text box at scene start
        if (active) {
            EnableTextBox(startText);
        } else {
            DisableTextBox();
        }
    }

    void FixedUpdate()
    {
        //if not active, do nothing
        if(!active){ return; }

        // set the current line of text in the box
        theText.text = textLines[currentLine];

        // spacebar press increments the current line
        if (Input.GetKeyDown("space")) 
        {
            currentLine++;
        }
        
        // close the text box and reset the curent line
        if (currentLine > endAtLine){
               DisableTextBox();
               currentLine = 0;
        }
        
    }

    // does as it says
    public void DisableTextBox()
    {        
            textBox.SetActive(false);
            player.canMove = true;
            active = false;
    }

    // Creates a new textbox on the screen that will diplay whatever textfile was handed to it
    // line by line
    public void EnableTextBox(TextAsset newText)
    {
         if (newText != null)
        {
            textLines = new string[1];
            textLines = (newText.text.Split('\n'));
        } else {
            Debug.Log("No Text File");
            return;
        }
        endAtLine = textLines.Length - 1;
        textBox.SetActive(true);
        player.canMove = false;
        active = true;
    }
}
