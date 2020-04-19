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

    }
    public void OnTriggerEnter2D(Collider2D other)
    {        
        Debug.Log("entered trigger");
        if(other.gameObject.layer == 10)
        {
            playerNear = true;
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Within trigger");
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit trigger");
    }
}
