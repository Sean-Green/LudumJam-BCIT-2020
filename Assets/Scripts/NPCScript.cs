using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCScript : MonoBehaviour
{
    // the class the manages the text box
    public TextBoxManager textBox;

    // .txt with the first interaction
    public TextAsset intro;

    // .txt with the next interaction
    public TextAsset repeated;

    public bool playerNear, introduced;


    private void FixedUpdate()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown("space"))
        {
            if (!introduced){
                textBox.EnableTextBox(intro);
                introduced = true;
            } else {
                textBox.EnableTextBox(repeated);
            }
        }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {        
        if(other.gameObject.layer == 10)
        {
            playerNear = true;
        }
    }
}
