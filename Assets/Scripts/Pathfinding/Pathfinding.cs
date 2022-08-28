using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    private GameController gameController;
    private Transform playerPoint;
    private Transform finalPoint;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        gameController = GameController.Instance;
        playerPoint = gameController.player.transform;
        finalPoint = gameController.mazeRender.finalObjectTransform;
    }

    private void Update()
    {
        if (gameController.NeedToShowPath()) {
            FindPath(playerPoint.position, finalPoint.position);
        }
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeFromWorldPoint(startPosition);
        Node finalNode = grid.GetNodeFromWorldPoint(targetPosition);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closeSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {

            Node currentNode = openSet.RemoveFirstItem();

            closeSet.Add(currentNode);

            if(currentNode == finalNode) {
                RetracePath(startNode, finalNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(currentNode)) {
                if(!neighbour.walkable || closeSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, finalNode);
                    neighbour.parent = currentNode;
                    
                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }

            }
        }
    }

    private void RetracePath(Node startNode, Node finalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = finalNode;

        while(currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;

    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distanceX > distanceY) {
            return 14 * distanceY + 10*(distanceX - distanceY);
        }
        return 14 * distanceX + 10*(distanceY - distanceX);

    }
}
