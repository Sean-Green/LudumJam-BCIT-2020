﻿#define USE_GIZMOS

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

    //Gizmo drawing stuffs
#if USE_GIZMOS
    private List<Node> lFoundPath;
#endif

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

        //Gizmo stuffs
#if USE_GIZMOS
        lFoundPath = new List<Node>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //if the player has moved, update the end position
        if (goPlayer.transform.position.y != nodeEnd.v2Pos.y || goPlayer.transform.position.x != nodeEnd.v2Pos.x) {
            nodeEnd = arrNodes[((int)goPlayer.transform.position.y - (int)v3GridStart.y) * nBoardWidth + (int)goPlayer.transform.position.x - (int)v3GridStart.x];
            nodeStart = arrNodes[((int)transform.position.y - (int)v3GridStart.y) * nBoardWidth + (int)transform.position.x - (int)v3GridStart.x];
#if USE_GIZMOS
            lFoundPath.Clear();
#endif
            if (nodeEnd != null) {
                Node p = nodeEnd;
                while (p.parent != null) {
#if USE_GIZMOS
                    lFoundPath.Add(p);
#endif
                    p = p.parent;
                }
            }

            SolveAStar();
        }
    }

    void SolveAStar() {
        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                arrNodes[y * nBoardWidth + x].bVisited = false;
                arrNodes[y * nBoardWidth + x].fGlobalGoal = Mathf.Infinity;
                arrNodes[y * nBoardWidth + x].fLocalGoal = Mathf.Infinity;
                arrNodes[y * nBoardWidth + x].parent = null;
            }

        System.Func<Node, Node, float> distance = (Node a, Node b) => {
            return Mathf.Sqrt((a.v2Pos.x - b.v2Pos.x) * (a.v2Pos.x - b.v2Pos.x) + (a.v2Pos.y - b.v2Pos.y) * (a.v2Pos.y - b.v2Pos.y));
        };

        System.Func<Node, Node, float> heuristic = (Node a, Node b) => {
            return distance(a, b);
        };

        //starting conditions
        Node currNode = nodeStart;
        nodeStart.fLocalGoal = 0f;
        nodeStart.fGlobalGoal = heuristic(nodeStart, nodeEnd);

        //add the start node to the list so that it gets visited
        //more nodes will be added to the list as they get visited
        List<Node> lNotTestedNodes = new List<Node> {
            nodeStart
        };

        while (lNotTestedNodes.Count != 0) {
            //want to know if lhs < rhs for sorting order
            lNotTestedNodes.Sort(delegate (Node rhs, Node lhs) {
                if (lhs.fGlobalGoal < rhs.fGlobalGoal) return 1;
                else if (lhs.fGlobalGoal > rhs.fGlobalGoal) return -1;
                return 0;
            });
            //Debug.Log(lNotTestedNodes);

            //if we've visited the node at the front, pop it off
            while (lNotTestedNodes.Count != 0 && lNotTestedNodes[0].bVisited)
                lNotTestedNodes.RemoveAt(0);

            //if the list is now empty, break the loop because we're done
            if (lNotTestedNodes.Count == 0)
                break;

            currNode = lNotTestedNodes[0];
            currNode.bVisited = true;

            foreach (Node neighbour in currNode.lNeigbours) {
                if (!neighbour.bVisited && !neighbour.bObstacle) {
                    lNotTestedNodes.Add(neighbour);
                }

                float fPossiblyLowerGoal = currNode.fLocalGoal + distance(currNode, neighbour);

                //if the path that we've found is lower than the current local goal
                //set it because that's a shorter path to take
                if (fPossiblyLowerGoal < neighbour.fLocalGoal) {
                    neighbour.parent = currNode;
                    neighbour.fLocalGoal = fPossiblyLowerGoal;

                    //now that we know a shorter path has been found,
                    //update the global goal so that we know how close
                    //we are to our goal of getting to the final spot
                    neighbour.fGlobalGoal = neighbour.fLocalGoal + heuristic(neighbour, nodeEnd);
                }
            }
        }
    }

#if USE_GIZMOS
    private void OnDrawGizmos() {
        for (int y = 0; y < nBoardHeight; y++)
            for (int x = 0; x < nBoardWidth; x++) {
                Gizmos.color = arrNodes[y * nBoardWidth + x].bObstacle ? Color.red : Color.blue;
                if (arrNodes[y * nBoardWidth + x].bVisited)
                    Gizmos.color = Color.cyan;
                if (arrNodes[y * nBoardWidth + x] == nodeStart)
                    Gizmos.color = Color.green;
                else if (arrNodes[y * nBoardWidth + x] == nodeEnd)
                    Gizmos.color = Color.grey;

                Gizmos.DrawSphere(arrNodes[y * nBoardWidth + x].v2Pos, 0.2f);
                foreach (Node n in arrNodes[y * nBoardWidth + x].lNeigbours) {
                    Gizmos.color = n.bObstacle || arrNodes[y * nBoardWidth + x].bObstacle ? Color.clear : Color.blue;
                    Gizmos.DrawLine(arrNodes[y * nBoardWidth + x].v2Pos, n.v2Pos);
                }

                if (lFoundPath.Count > 0) {
                    Node n = lFoundPath[0];
                    while (n.parent != null) {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(n.v2Pos, n.parent.v2Pos);
                        n = n.parent;
                    }
                }
            }
    }
#endif

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
