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

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                Gizmos.color = arrNodes[y * nBoardWidth + x].bObstacle ? Color.red : Color.blue;
                Gizmos.DrawSphere(arrNodes[y * nBoardWidth + x].v2Pos, 0.2f);
            }
    }

    class Node {
        public bool bObstacle = false;
        public bool bVisited;
        public float fGlobalGoal;
        public float fLocalGoal;
        public Vector2 v2Pos;
        public List<Node> lNeigbours;
        public Node parent;
    }
}
