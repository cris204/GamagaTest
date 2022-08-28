using UnityEngine;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    private static UIGameController instance;
    public static UIGameController Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject completeMazePanel;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public void CompleteGame()
    {
        completeMazePanel.SetActive(true);
    }

    public void ResetGame()
    {
        completeMazePanel.SetActive(false);
        ReGenerateMaze();
    }

    #region Called By Button Event

    //Called By Button Event;
    public void ShowPath()
    {
        GameController.Instance.ToggleAstarPath();
    }


    //Called By Button Event;
    public void ReGenerateMaze()
    {
        
        GameController.Instance.RestartGame();
    }
    #endregion

}
