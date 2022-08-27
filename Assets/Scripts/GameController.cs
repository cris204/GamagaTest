using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
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
        mazeRender.GenerateMaze();
    }


}
