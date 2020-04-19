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

    public bool introduced;

    // Really rough way to have the player interact with NPC's by colliding with them and hit space
    // will need to be changed if we move to tile based movement.
    // other issues include having to walk away from the NPC and collide again to trigger the text box more than once
    private void OnCollisionStay2D(Collision2D other){
        
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
