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

    public MazeRenderer mazeRender;
    public Grid grid;
    public GameObject player;
    public GameState currentState;

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
        currentState = GameState.Waiting;
        mazeRender.GenerateMaze();
        player.transform.position = mazeRender.GetStartPosition();
        currentState = GameState.Playing;
        grid.SetGridSize(mazeRender.width, mazeRender.height);
        StartCoroutine(WaitToCreatePath());
    }

    IEnumerator WaitToCreatePath()
    {
        yield return new WaitForSeconds(0.25f);
        grid.GeneratePath();
    }

    public void CompleteMaze()
    {
        UIGameController.Instance.CompleteGame();
        currentState = GameState.End;
    }

}
