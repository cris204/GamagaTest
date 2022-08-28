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
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_VICTORY_PATH, 0.25f);
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
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_BUTTON_PATH, 0.5f);
        GameController.Instance.ToggleAstarPath();
    }


    //Called By Button Event;
    public void ReGenerateMaze()
    {
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_BUTTON_PATH, 0.5f);
        GameController.Instance.RestartGame();
    }
    #endregion

}
