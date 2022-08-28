using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [Header("Maze config")]
    [Range(1, 20)] public int width = 10;
    [Range(1, 20)] public int height = 10;
    public float size = 1f;

    public float minMazeSize = 1;
    public float maxMazeSize = 20;

    [Header("Final")]
    public Transform finalObjectTransform;

    [Header("Wall")]
    public Transform wallsContainer;
    public List<Transform> walls;

    [Header("Node")]
    public Transform nodesContainer;
    public List<Transform> nodesTranform;
    public List<NodeData> nodesData = new List<NodeData>();

    #region Generate Maze
    public void GenerateMaze() {
        transform.position = Vector3.zero;
        RemoveMaze();
        var maze = MazeGenerator.Generate(width, height);
        DrawMaze(maze);

        finalObjectTransform.position = nodesTranform[nodesTranform.Count - 1].position;

    }

    private void DrawMaze(WallState[,] maze) {
        nodesData.Clear();
        walls.Clear();
        nodesTranform.Clear();

        Vector3 position;
        Vector3 positionOffset;
        Vector3 wallScale;

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {
                nodesData.Add(MazeGenerator.nodesData[i, j]);
                WallState tile = maze[i, j];
                position = new Vector3((transform.position.x + (-width / 2) + i * size), 0, transform.position.z + (-height / 2) + j * size);

                Transform newTile = PoolManager.Instance.GetObject(Env.MAZE_NODE_PATH).transform;
                newTile.SetParent(nodesContainer);
                newTile.position = position;
                newTile.name = nodesTranform.Count.ToString();

                nodesTranform.Add(newTile);
                if (tile.HasFlag(WallState.UP)) {
                    Transform topWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                    topWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.z = size / 2;
                    topWall.position = position + positionOffset;
                    wallScale = topWall.localScale;
                    wallScale.x = size;
                    topWall.localScale = wallScale;
                    topWall.eulerAngles = Vector3.zero;

                    walls.Add(topWall);
                    MazeGenerator.nodesData[i, j].RemoveNeighbour(WallState.UP);
                }

                if (tile.HasFlag(WallState.LEFT)) {
                    Transform leftWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                    leftWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.x = -size / 2;
                    leftWall.position = position + positionOffset;
                    wallScale = leftWall.localScale;
                    wallScale.x = size;
                    leftWall.localScale = wallScale;
                    leftWall.eulerAngles = Vector3.up * 90;

                    walls.Add(leftWall);
                    MazeGenerator.nodesData[i, j].RemoveNeighbour(WallState.LEFT);
                }

                if (i == (width - 1)) {

                    if (tile.HasFlag(WallState.RIGHT)) {
                        Transform rightWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                        rightWall.SetParent(wallsContainer);
                        positionOffset = Vector3.zero;
                        positionOffset.x = size / 2;
                        rightWall.position = position + positionOffset;
                        wallScale = rightWall.localScale;
                        wallScale.x = size;
                        rightWall.localScale = wallScale;
                        rightWall.eulerAngles = Vector3.up * 90;

                        walls.Add(rightWall);
                    }

                }

                if (j == 0) {

                    if (tile.HasFlag(WallState.DOWN)) {
                        Transform downWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                        downWall.SetParent(wallsContainer);
                        positionOffset = Vector3.zero;
                        positionOffset.z = -size / 2;
                        downWall.position = position + positionOffset;
                        wallScale = downWall.localScale;
                        wallScale.x = size;
                        downWall.localScale = wallScale;
                        downWall.eulerAngles = Vector3.zero;

                        walls.Add(downWall);
                    }

                }
                MazeGenerator.nodesData[i, j].Name = (nodesTranform.Count-1).ToString();
                MazeGenerator.nodesData[i, j].nodePosition = nodesTranform[nodesTranform.Count - 1];
            }

        }

    }

    private void RemoveMaze() {
        for (int i = 0; i < walls.Count; i++) {
            PoolManager.Instance.ReleaseObject(walls[i].gameObject);
        }

        //We doesnt need to remove the tiles because the position will be the same
        for (int i = 0; i < nodesTranform.Count; i++) {
            PoolManager.Instance.ReleaseObject(nodesTranform[i].gameObject);
        }
    }

    #endregion


    public Vector3 GetStartPosition() {
        return nodesTranform[0].transform.position;
    }

    public NodeData GetNodeInPosition(int x, int y)
    {
        for (int i = 0; i < nodesData.Count; i++) {
            if(nodesData[i].Index.x == x && nodesData[i].Index.y == y) {
                return nodesData[i];
            }
        }
        return null;
    }

    public NodeData GetNearNodeByDistance(Vector3 position)
    {
        float minDistance = Vector3.Distance(position, nodesData[0].nodePosition.position);
        NodeData nearNode = nodesData[0];

        for (int i = 1; i < nodesData.Count; i++) {

            float distance = Vector3.Distance(position, nodesData[i].nodePosition.position);
            if (minDistance > distance) {
                minDistance = distance;
                nearNode = nodesData[i];
            }
        }

        return nearNode;
    }

    public NodeData GetNearNodeByDistance(Vector3 position, List<NodeData> nodes)
    {
        float minDistance = Vector3.Distance(position, nodes[0].nodePosition.position);
        NodeData nearNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++) {

            float distance = Vector3.Distance(position, nodes[i].nodePosition.position);
            if (minDistance > distance) {
                minDistance = distance;
                nearNode = nodes[i];
            }
        }

        return nearNode;
    }
}
