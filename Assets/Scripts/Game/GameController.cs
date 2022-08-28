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
    public PlayerController player;
    public CameraSmoothFollow gameCamera;
    public NewPathFinding pathFinding;
    public LineRenderer pathLine;

    [Header("Config")]
    public GameState currentState;

    public bool generatedPath;
    internal bool needToGenerate;
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
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_BACKGROUND_PATH, 0.4f,true);
        if (pathFinding == null) {
            pathFinding = new NewPathFinding();
        }
        GenerateMaze();
    }
    
    List<NodeData> path = new List<NodeData>();

    private void Update()
    {
        if (NeedToShowPath()) {

            if (player.rb.velocity.magnitude > 0.1f || needToGenerate) {
                
                player.currentNode = mazeRender.GetNearNodeByDistance(player.transform.position, player.currentNode.GetNeighbours(true));
                path.Clear();
                path = pathFinding.FindPath((int)player.currentNode.Index.x, (int)player.currentNode.Index.y, mazeRender.width - 1, mazeRender.height - 1);

                if (path == null) return;
                pathLine.positionCount = path.Count;
                pathLine.SetPosition(0, player.transform.position);
                for (int i = 1; i < path.Count; i++) {
                    pathLine.SetPosition(i, path[i].nodePosition.position);
                }
                needToGenerate = false;
            }
        }
    }

    public void RestartGame()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        pathLine.gameObject.SetActive(false);
        showingPath = false;
        currentState = GameState.Waiting;
        mazeRender.GenerateMaze();
        player.transform.position = mazeRender.GetStartPosition();
        gameCamera.MoveInstant();
        currentState = GameState.Playing;
        generatedPath = false;

        Vector3 minPos = mazeRender.nodesTranform[0].position;
        Vector3 maxPos = mazeRender.finalObjectTransform.position;

        gameCamera.SetUpCamera(minPos.x / 2, maxPos.x / 1.2f, minPos.z / 2, maxPos.z / 1.2f);
        player.currentNode = mazeRender.GetNodeInPosition(0, 0);
    }

    public void ToggleAstarPath()
    {
        showingPath = !showingPath;
        if (showingPath) {
            if (!generatedPath) {
                CreateAstarPath();
            }

            needToGenerate = true;
            pathLine.gameObject.SetActive(true);
        } else {

            pathLine.gameObject.SetActive(false);
        }
    }

    public void CreateAstarPath()
    {
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
