using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [Header("Maze config")]
    [Range(1, 50)] public int width = 10;
    [Range(1, 50)] public int height = 10;
    public float size = 1f;

    [Header("Floor")]
    public Transform floorTransform;

    [Header("Final")]
    public Transform finalObjectTransform;

    [Header("Wall")]
    public PooleableObject wallPrefab;
    public Transform wallsContainer;
    public List<Transform> walls;

    [Header("Node")]
    public PooleableObject nodePrefab;
    public Transform nodesContainer;
    public List<Transform> nodes;

    #region Generate Maze
    public void GenerateMaze()
    {
        transform.position = Vector3.zero;
        RemoveMaze();
        var maze = MazeGenerator.Generate(width, height);
        DrawMaze(maze);

        transform.position = new Vector3(size / 2, 0, size / 2); 

        finalObjectTransform.position = nodes[nodes.Count - 1].position;

    }

    private void DrawMaze(WallState[,] maze)
    {
        walls.Clear();
        nodes.Clear();

        floorTransform.localScale = new Vector3(width/2 , 1, height/2);

        Vector3 position;
        Vector3 positionOffset;
        Vector3 wallScale;

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {

                WallState tile = maze[i, j];
                position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                Transform newTile = PoolManager.Instance.GetObject(nodePrefab.path).transform;
                newTile.SetParent(nodesContainer);
                newTile.position = position;
                nodes.Add(newTile);

                if (tile.HasFlag(WallState.UP)) {
                    Transform topWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                    topWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.z = size / 2;
                    topWall.position = position + positionOffset;
                    wallScale = topWall.localScale;
                    wallScale.x = size;
                    topWall.localScale = wallScale;
                    topWall.eulerAngles = Vector3.zero;

                    walls.Add(topWall);
                }

                if (tile.HasFlag(WallState.LEFT)) {
                    Transform leftWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                    leftWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.x = -size / 2;
                    leftWall.position = position + positionOffset;
                    wallScale = leftWall.localScale;
                    wallScale.x = size;
                    leftWall.localScale = wallScale;
                    leftWall.eulerAngles = Vector3.up * 90;
                    
                    walls.Add(leftWall);
                }

                if (i == (width - 1)) {

                    if (tile.HasFlag(WallState.RIGHT)) {
                        Transform rightWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
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
                        Transform downWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
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
            }

        }

    }

    private void RemoveMaze()
    {
        for (int i = 0; i < walls.Count; i++) {
            PoolManager.Instance.ReleaseObject(walls[i].gameObject);
        }

        //We doesnt need to remove the tiles because the position will be the same
        for (int i = 0; i < nodes.Count; i++) {
            PoolManager.Instance.ReleaseObject(nodes[i].gameObject);
        }
    }

    #endregion


    public Vector3 GetStartPosition()
    {
        return nodes[0].transform.position;
    }


}
