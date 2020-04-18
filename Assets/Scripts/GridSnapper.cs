﻿#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapper : MonoBehaviour
{

    public float fMoveSpeed;
    public int nGridSize;
    public Rigidbody2D rb;

    private Vector2 v2Pos;
    private Vector2 v2GoToPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        v2Pos = transform.position;
        v2GoToPos = v2Pos;
    }

    // Update is called once per frame
    void Update()
    {
        //set the position of everything for this frame
        int nMoveX = (int)Input.GetAxisRaw("Horizontal");
        int nMoveY = (int)Input.GetAxisRaw("Vertical");

        if (Vector3.Distance(transform.position, v2GoToPos) <= 0.1f) {
            if (Mathf.Abs(nMoveX) == 1) {
                v2GoToPos += new Vector2(nMoveX * nGridSize, 0);
            }
            if (Mathf.Abs(nMoveY) == 1) {
                v2GoToPos += new Vector2(0, nMoveY * nGridSize);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, v2GoToPos, fMoveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(v2GoToPos, transform.position);
    }

    void FixedUpdate() {

    }
}