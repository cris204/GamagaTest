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

    [Header("Config")]
    public MazeRenderer mazeRender;
    public Grid grid;
    public PlayerController player;
    public GameState currentState;

    internal bool generatedPath;
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
        grid.SetGridSize(mazeRender.width * (int)mazeRender.size * 2, mazeRender.height * (int)mazeRender.size * 2);
        grid.pathLine.gameObject.SetActive(false);
        currentState = GameState.Playing;
        generatedPath = false;
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

}
