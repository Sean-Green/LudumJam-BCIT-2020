using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnapper : MonoBehaviour
{
    public float fMoveSpeed;
    public float fGridSize;
    public LayerMask lmCollideWith;

    private Vector2 v2GoToPos;
    private Vector2 v2LazyFollow;

    // Start is called before the first frame update
    void Start()
    {
        v2GoToPos = transform.position;
        v2LazyFollow = v2GoToPos;
    }

    public void MoveObject(int nMoveX, int nMoveY)
    {
        //normalize movement between 0 and 1
        if (nMoveX > 0)
            nMoveX /= nMoveX;
        else if (nMoveX < 0)
            nMoveX /= -nMoveX;

        if (nMoveY > 0)
            nMoveY /= nMoveY;
        else if (nMoveY < 0)
            nMoveY /= -nMoveY;

        //TODO: do not let diagonal movement happen if the edge piece is an obstacle
        if (Vector3.Distance(transform.position, v2GoToPos) == 0f) {
            if (!Physics2D.OverlapCircle(v2GoToPos + new Vector2(nMoveX * fGridSize, 0), 0.2f, lmCollideWith)) {
                if (Mathf.Abs(nMoveX) == 1) {
                    v2GoToPos += new Vector2(nMoveX * fGridSize, 0);
                }
            }
            if (!Physics2D.OverlapCircle(v2GoToPos + new Vector2(0, nMoveY * fGridSize), 0.2f, lmCollideWith)) {
                if (Mathf.Abs(nMoveY) == 1) {
                    v2GoToPos += new Vector2(0, nMoveY * fGridSize);
                }
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, v2GoToPos, fMoveSpeed * Time.deltaTime);

        if (transform.position.x == v2GoToPos.x && transform.position.y == v2GoToPos.y && v2GoToPos != v2LazyFollow)
            v2LazyFollow = v2GoToPos;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(v2GoToPos, transform.position);
        Gizmos.DrawSphere(v2GoToPos, 0.2f);
    }

    void FixedUpdate() {

    }
}
