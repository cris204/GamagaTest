using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeData
{
    public string Name;
    public Vector2 Index;
    public Transform nodePosition;

    public Dictionary<WallState, NodeData> neighboursDictionary = new Dictionary<WallState, NodeData> {
        { WallState.LEFT, null},
        { WallState.DOWN, null},
        { WallState.RIGHT, null},
        { WallState.UP, null},
    };
  

    public int gCost;
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }

    }
    
    public NodeData cameFromNode;

    public List<NodeData> GetNeighbours(bool includeMe = false) {
         
        List<NodeData> neighboursList = new List<NodeData>();

        foreach (NodeData node in neighboursDictionary.Values) {

            if(node != null) {
                neighboursList.Add(node);
            }
        }
        if (includeMe) {
            neighboursList.Add(this);
        }
        return neighboursList;
    }

    public void AddNeighbour(NodeData node, WallState side) {
        neighboursDictionary[side] = node;
        node.neighboursDictionary[MazeGenerator.GetOppositeWall(side)] = this;
    }

    public void RemoveNeighbour(WallState side) {
        NodeData node = neighboursDictionary[side];
        if (node == null)
            return;
        node.neighboursDictionary[MazeGenerator.GetOppositeWall(side)] = null;
        neighboursDictionary[side] = null;
    }
}
