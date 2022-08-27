using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool isWalkable, Vector3 worldPos, int xPos, int yPos)
    {
        walkable = isWalkable;
        worldPosition = worldPos;
        gridX = xPos;
        gridY = yPos;
    }

    public int fCost
    {
        get{
            return gCost + hCost;
        }

    }

}
