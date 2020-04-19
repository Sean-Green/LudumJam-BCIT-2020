using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public GridSnapper gs;
    public Animator anim;

    Vector2 movement;

    public bool canMove = true;

    private void Start() {
        if (gs == null)
            gs = GetComponent<GridSnapper>();
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        int nMoveX = (int)Input.GetAxisRaw("Horizontal");
        int nMoveY = (int)Input.GetAxisRaw("Vertical");

        gs.MoveObject(nMoveX, nMoveY);
        anim.SetBool("bMoving", nMoveX != 0 || nMoveY != 0);


        if (nMoveX != 0 && nMoveX != transform.localScale.x) {
            Vector3 v3Scale = transform.localScale;
            v3Scale.x = -v3Scale.x;
            transform.localScale = v3Scale;
        }
    }
}
