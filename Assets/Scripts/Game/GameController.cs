using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { 

    Waiting = 0,
    Playing = 1,
    End = 2
}


public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("References")]
    public MazeRenderer mazeRender;
    public Grid grid;
    public PlayerController player;
    public CameraSmoothFollow gameCamera;

    [Header("Config")]
    public GameState currentState;

    public bool generatedPath;
    internal bool showingPath;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    private void Start()
    {
        GenerateMaze();

    }

    public void RestartGame()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        showingPath = false;
        currentState = GameState.Waiting;
        mazeRender.GenerateMaze();
        player.transform.position = mazeRender.GetStartPosition();
        gameCamera.MoveInstant();
        grid.SetGridSize(mazeRender.width * (int)mazeRender.spaceSize * 2, mazeRender.height * (int)mazeRender.spaceSize * 2);
        grid.pathLine.gameObject.SetActive(false);
        currentState = GameState.Playing;
        generatedPath = false;

        Vector3 minPos = mazeRender.nodes[0].position;
        Vector3 maxPos = mazeRender.finalObjectTransform.position;

        gameCamera.SetUpCamera(minPos.x / 2, maxPos.x, minPos.z / 2, maxPos.z);

    }

    public void ToggleAstarPath()
    {
        showingPath = !showingPath;
        if (showingPath) {
            if (!generatedPath) {
                CreateAstarPath();
            }

            grid.pathLine.gameObject.SetActive(true);
        } else {

            grid.pathLine.gameObject.SetActive(false);
        }
    }

    public void CreateAstarPath()
    {
        grid.GeneratePath();
        generatedPath = true;
    }

    public void CompleteMaze()
    {
        UIGameController.Instance.CompleteGame();
        currentState = GameState.End;
    }
    public bool NeedToShowPath()
    {
        return generatedPath && showingPath && currentState == GameState.Playing;
    }

    public void ChangeMazeSize(int width, int height)
    {

        if (width != -1) {
            mazeRender.width = width;
        }
        if (height != -1) {
            mazeRender.height = height;
        }
    }

}
