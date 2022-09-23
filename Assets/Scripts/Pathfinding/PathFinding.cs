using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<NodeData> openList = new List<NodeData>();
    private List<NodeData> closeList = new List<NodeData>();

    private MazeRenderer mazeRender;

    public List<NodeData> FindPath(int startX, int startY, int endX, int endY)
    {
        openList.Clear();
        closeList.Clear();

        if (mazeRender == null) {
            mazeRender = GameController.Instance.mazeRender;
        }

        NodeData startNode = mazeRender.GetNodeInPosition(startX, startY);
        NodeData endNode = mazeRender.GetNodeInPosition(endX, endY);

        openList.Add(startNode);

        for (int x = 0; x < mazeRender.width; x++) {
            for (int y = 0; y < mazeRender.height; y++) {
                NodeData nodeData = mazeRender.GetNodeInPosition(x, y);
                nodeData.gCost = int.MaxValue;
                nodeData.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);

        while(openList.Count > 0) {
            NodeData currentNode = GetLowestFcostNode(openList);
            if(currentNode == endNode) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            List<NodeData> nodeNeighbours = currentNode.GetNeighbours();

            foreach (NodeData node in nodeNeighbours) {

                if (closeList.Contains(node)) continue;

                int tentativeGcost = currentNode.gCost + CalculateDistance(currentNode, node);
                if(tentativeGcost < node.gCost) {
                    node.cameFromNode = currentNode;
                    Debug.DrawLine(currentNode.nodePosition.position, currentNode.nodePosition.up * 50, Color.red,60);
                    node.IndexFrom = currentNode.Index;
                    node.gCost = tentativeGcost;
                    node.hCost = CalculateDistance(node, endNode);

                    if (!openList.Contains(node)) {
                        openList.Add(node);
                    }

                }
            }

        }

        return null;
    }

    private List<NodeData> CalculatePath(NodeData endNode)
    {
        List<NodeData> path = new List<NodeData>();
        path.Add(endNode);
        NodeData currentNode = endNode;

        while(currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistance(NodeData nodeA, NodeData nodeB)
    {
        int xDistance = (int)Mathf.Abs(nodeA.Index.x - nodeB.Index.x);
        int yDistance = (int)Mathf.Abs(nodeA.Index.y - nodeB.Index.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private NodeData GetLowestFcostNode(List<NodeData> nodes)
    {
        NodeData lowestFCostNode = nodes[0];

        for (int i = 0; i < nodes.Count; i++) {
            if(nodes[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = nodes[i];
            }
        }
        return lowestFCostNode;
    }

}
