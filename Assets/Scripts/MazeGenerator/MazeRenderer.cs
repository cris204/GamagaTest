using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [Header("Maze config")]
    [Range(1, 50)] public int width = 10;
    [Range(1, 50)] public int height = 10;
    public float size = 1f;

    [Header("Wall")]
    public PooleableObject wallPrefab;
    public Transform wallsContainer;
    public List<Transform> walls;

    [Header("Tile")]
    public PooleableObject tilePrefab;
    public Transform tilesContainer;
    public List<Transform> tiles;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            RemoveMaze();
            var maze = MazeGenerator.Generate(width, height);
            Draw(maze);
        }
    }

    private void Draw(WallState[,] maze)
    {
        walls.Clear();
        tiles.Clear();

        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {

                WallState tile = maze[i, j];
                Vector3 position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                Transform newTile = PoolManager.Instance.GetObject(tilePrefab.path).transform;
                newTile.SetParent(tilesContainer);
                newTile.position = position;
                tiles.Add(newTile);

                if (tile.HasFlag(WallState.UP)) {
                    Transform topWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                    topWall.SetParent(wallsContainer);
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                    topWall.eulerAngles = Vector3.zero;

                    walls.Add(topWall);
                }

                if (tile.HasFlag(WallState.LEFT)) {
                    Transform leftWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                    leftWall.SetParent(wallsContainer);
                    leftWall.position = position + new Vector3(-size/2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                    
                    walls.Add(leftWall);
                }

                if (i == (width - 1)) {

                    if (tile.HasFlag(WallState.RIGHT)) {
                        Transform rightWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                        rightWall.SetParent(wallsContainer);
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);

                        walls.Add(rightWall);
                    }

                }

                if (j == 0) {

                    if (tile.HasFlag(WallState.DOWN)) {
                        Transform downWall = PoolManager.Instance.GetObject(wallPrefab.path).transform;
                        downWall.SetParent(wallsContainer);
                        downWall.position = position + new Vector3(0, 0, -size / 2);
                        downWall.localScale = new Vector3(size, downWall.localScale.y, downWall.localScale.z);
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
        for (int i = 0; i < tiles.Count; i++) {
            PoolManager.Instance.ReleaseObject(tiles[i].gameObject);
        }
    }

}
