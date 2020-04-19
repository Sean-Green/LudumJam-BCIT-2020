using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    private Vector2Int v2ObstacleFinder;

    // Start is called before the first frame update
    void Start()
    {
        if (goPlayer == null)
            goPlayer = GameObject.FindGameObjectWithTag("Player");

        nBoardWidth = tmTiles.size.x;
        nBoardHeight = tmTiles.size.y;
        v2ObstacleFinder = Vector2Int.zero;

        arrNodes = new Node[nBoardHeight * nBoardWidth];
        for (int y = 0; y < nBoardHeight; y++) {
            for (int x = 0; x < nBoardWidth; x++) {
                v2ObstacleFinder.x = x;
                v2ObstacleFinder.y = y;

                //can refactor to the constructor
                arrNodes[y * nBoardWidth + x] = new Node {
                    x = x,
                    y = y,
                    bObstacle = Physics2D.OverlapCircle(v2ObstacleFinder, 0.2f, lmObstacles),
                    parent = null,
                    bVisited = false
                };
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(v2ObstacleFinder.x, v2ObstacleFinder.y, 0), 0.5f);
    }

    class Node {
        public bool bObstacle = false;
        public bool bVisited;
        public float fGlobalGoal;
        public float fLocalGoal;
        public int x;
        public int y;
        public List<Node> lNeigbours;
        public Node parent;
    }
}
