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

    public string LeftNode;
    public string DownNode;
    public string RightNode;
    public string UpNode;
    
    public NodeData cameFromNode;
    public void UpdateNeighbors() {
        LeftNode = neighboursDictionary[WallState.LEFT] != null ? neighboursDictionary[WallState.LEFT].Name : "";
        DownNode = neighboursDictionary[WallState.DOWN] != null ? neighboursDictionary[WallState.DOWN].Name : "";
        RightNode = neighboursDictionary[WallState.RIGHT] != null ? neighboursDictionary[WallState.RIGHT].Name : "";
        UpNode = neighboursDictionary[WallState.UP] != null ? neighboursDictionary[WallState.UP].Name : "";
        GetNeighbours();
    }

    public List<NodeData> GetNeighbours() {
         
        List<NodeData> neighboursList = new List<NodeData>();

        foreach (NodeData node in neighboursDictionary.Values) {

            if(node != null) {
                neighboursList.Add(node);
            }
        }
        return neighboursList;
    }

    public void AddNeighbour(NodeData node, WallState side) {
        neighboursDictionary[side] = node;
        node.neighboursDictionary[MazeGenerator.GetOppositeWall(side)] = this;

        UpdateNeighbors();
        node.UpdateNeighbors();
    }

    public void RemoveNeighbour(WallState side) {
        NodeData node = neighboursDictionary[side];
        if (node == null)
            return;
        node.neighboursDictionary[MazeGenerator.GetOppositeWall(side)] = null;
        node.UpdateNeighbors();
        neighboursDictionary[side] = null;
        UpdateNeighbors();
    }
}
