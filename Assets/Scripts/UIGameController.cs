using UnityEngine;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{

    //Called By Button Event;
    public void ReGenerateMaze()
    {
        GameController.Instance.RestartGame();
    }

}
