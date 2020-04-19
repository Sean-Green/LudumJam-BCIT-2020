using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public GridSnapper gs;

    Vector2 movement;

    public bool canMove = true;

    private void Start() {
        if (gs == null)
            gs = GetComponent<GridSnapper>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        gs.MoveObject((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
    }
}
