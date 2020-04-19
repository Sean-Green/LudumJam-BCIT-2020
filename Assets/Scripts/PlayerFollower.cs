using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFollower : MonoBehaviour
{
    public GameObject goPlayer;
    public LayerMask lmObstacles;
    public Tilemap tmTiles;

    private Node[] arrNodes;
    private int nBoardWidth;
    private int nBoardHeight;
    private Grid grGrid;
    private Vector3 v3GridStart;

    private Node nodeStart;
    private Node nodeEnd;

    // Start is called before the first frame update
    void Start() {
        if (goPlayer == null)
            goPlayer = GameObject.FindGameObjectWithTag("Player");

        if (grGrid == null)
            grGrid = tmTiles.GetComponentInParent<Grid>();

        nBoardWidth = tmTiles.size.x;
        nBoardHeight = tmTiles.size.y;
        Vector2Int v2ObstacleFinder = Vector2Int.zero;

        v3GridStart = (grGrid.GetCellCenterWorld(grGrid.WorldToCell(grGrid.transform.position)));
        Debug.Log(nBoardHeight + ", " + nBoardWidth);

        arrNodes = new Node[nBoardHeight * nBoardWidth];
        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                v2ObstacleFinder.x = x + (int)v3GridStart.x;
                v2ObstacleFinder.y = y + (int)v3GridStart.y;

                arrNodes[y * nBoardWidth + x] = new Node {
                    v2Pos = new Vector2(x + (int)v3GridStart.x, y + (int)v3GridStart.y),
                    bObstacle = Physics2D.OverlapCircle(v2ObstacleFinder, 0.2f, lmObstacles),
                    parent = null,
                    bVisited = false
                };
            }

        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                if (y > 0)                  arrNodes[y * nBoardWidth + x].lNeigbours.Add(arrNodes[(y - 1) * nBoardWidth + x]);
                if (x > 0)                  arrNodes[y * nBoardWidth + x].lNeigbours.Add(arrNodes[y * nBoardWidth + (x - 1)]);
                if (x < nBoardWidth - 1)    arrNodes[y * nBoardWidth + x].lNeigbours.Add(arrNodes[y * nBoardWidth + (x + 1)]);
                if (y < nBoardHeight - 1)   arrNodes[y * nBoardWidth + x].lNeigbours.Add(arrNodes[(y + 1) * nBoardWidth + x]);
            }

        nodeStart = arrNodes[((int)transform.position.y - (int)v3GridStart.y) * nBoardWidth + (int)transform.position.x - (int)v3GridStart.x];
        nodeEnd = arrNodes[((int)goPlayer.transform.position.y - (int)v3GridStart.y) * nBoardWidth + (int)goPlayer.transform.position.x - (int)v3GridStart.x];
    }

    // Update is called once per frame
    void Update()
    {
        if (goPlayer.transform.position.y != nodeEnd.v2Pos.y || goPlayer.transform.position.x != nodeEnd.v2Pos.x)
            nodeEnd = arrNodes[((int)goPlayer.transform.position.y - (int)v3GridStart.y) * nBoardWidth + (int)goPlayer.transform.position.x - (int)v3GridStart.x];
    }

    private void OnDrawGizmos() {
        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                Gizmos.color = arrNodes[y * nBoardWidth + x].bObstacle ? Color.red : Color.blue;
                if (arrNodes[y * nBoardWidth + x] == nodeStart)
                    Gizmos.color = Color.green;
                else if (arrNodes[y * nBoardWidth + x] == nodeEnd)
                    Gizmos.color = Color.magenta;

                Gizmos.DrawSphere(arrNodes[y * nBoardWidth + x].v2Pos, 0.2f);
                foreach (Node n in arrNodes[y * nBoardWidth + x].lNeigbours) {
                    Gizmos.color = n.bObstacle || arrNodes[y * nBoardWidth + x].bObstacle ? Color.clear : Color.blue;
                    Gizmos.DrawLine(arrNodes[y * nBoardWidth + x].v2Pos, n.v2Pos);
                }
            }
    }

    class Node {
        public bool bObstacle = false;
        public bool bVisited = false;
        public float fGlobalGoal;
        public float fLocalGoal;
        public Vector2 v2Pos;
        public List<Node> lNeigbours = new List<Node>();
        public Node parent;
    }
}
