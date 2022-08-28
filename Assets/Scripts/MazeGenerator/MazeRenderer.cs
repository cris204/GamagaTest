using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [Header("Maze config")]
    [Range(2, 20)] public int width = 10;
    [Range(2, 20)] public int height = 10;
    public int minMazeSize = 2;
    public int maxMazeSize = 20;

    public float spaceSize = 1f;

    [Header("Final")]
    public Transform finalObjectTransform;

    [Header("Wall")]
    public Transform wallsContainer;
    public List<Transform> walls;

    [Header("Node")]
    public Transform nodesContainer;
    public List<Transform> nodes;

    #region Generate Maze
    public void GenerateMaze()
    {
        transform.position = Vector3.zero;
        RemoveMaze();
        var maze = MazeGenerator.Generate(width, height);
        DrawMaze(maze);

        finalObjectTransform.position = nodes[nodes.Count - 1].position;

    }

    private void DrawMaze(WallState[,] maze)
    {
        walls.Clear();
        nodes.Clear();

        Vector3 position;
        Vector3 positionOffset;
        Vector3 wallScale;

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {

                WallState tile = maze[i, j];
                position = new Vector3((transform.position.x + (-width / 2) + i * spaceSize), 0, transform.position.z + (-height / 2) + j * spaceSize);

                Transform newTile = PoolManager.Instance.GetObject(Env.MAZE_NODE_PATH).transform;
                newTile.SetParent(nodesContainer);
                newTile.position = position;
                newTile.name = nodes.Count.ToString();
                nodes.Add(newTile);

                if (tile.HasFlag(WallState.UP)) {
                    Transform topWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                    topWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.z = spaceSize / 2;
                    topWall.position = position + positionOffset;
                    wallScale = topWall.localScale;
                    wallScale.x = spaceSize;
                    topWall.localScale = wallScale;
                    topWall.eulerAngles = Vector3.zero;

                    walls.Add(topWall);
                }

                if (tile.HasFlag(WallState.LEFT)) {
                    Transform leftWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                    leftWall.SetParent(wallsContainer);
                    positionOffset = Vector3.zero;
                    positionOffset.x = -spaceSize / 2;
                    leftWall.position = position + positionOffset;
                    wallScale = leftWall.localScale;
                    wallScale.x = spaceSize;
                    leftWall.localScale = wallScale;
                    leftWall.eulerAngles = Vector3.up * 90;
                    
                    walls.Add(leftWall);
                }

                if (i == (width - 1)) {

                    if (tile.HasFlag(WallState.RIGHT)) {
                        Transform rightWall = PoolManager.Instance.GetObject(Env.MAZE_WALL_PATH).transform;
                        rightWall.SetParent(wallsContainer);
                        positionOffset = Vector3.zero;
                        positionOffset.x = spaceSize / 2;
                        rightWall.position = position + positionOffset;
                        wallScale = rightWall.localScale;
                        wallScale.x = spaceSize;
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
                        positionOffset.z = -spaceSize / 2;
                        downWall.position = position + positionOffset;
                        wallScale = downWall.localScale;
                        wallScale.x = spaceSize;
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
