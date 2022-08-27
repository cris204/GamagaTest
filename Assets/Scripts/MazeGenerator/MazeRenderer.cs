using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)] private int width = 10;
    
    [SerializeField]
    [Range(1, 50)] private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform wallPrefab;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);
    }

    void Update()
    {
        
    }

    private void Draw(WallState[,] maze)
    {
        for (int i = 0; i < width; i++) {

            for (int j = 0; j < height; j++) {

                WallState cell = maze[i, j];
                Vector3 position = new Vector3(-width / 2 + i, 0, -height / 2 + j);

                if (cell.HasFlag(WallState.UP)) {
                    Transform topWall = Instantiate(wallPrefab, transform);
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT)) {
                    Transform leftWall = Instantiate(wallPrefab, transform);
                    leftWall.position = position + new Vector3(-size/2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == (width - 1)) {

                    if (cell.HasFlag(WallState.RIGHT)) {
                        Transform rightWall = Instantiate(wallPrefab, transform);
                        rightWall.position = position + new Vector3(size / 2, 0, 0);
                        rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }

                }

                if (j == 0) {

                    if (cell.HasFlag(WallState.DOWN)) {
                        Transform downWall = Instantiate(wallPrefab, transform);
                        downWall.position = position + new Vector3(0, 0, -size / 2);
                        downWall.localScale = new Vector3(size, downWall.localScale.y, downWall.localScale.z);
                    }

                }
            }

        }
    }

}
