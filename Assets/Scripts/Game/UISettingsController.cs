using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettingsController : MonoBehaviour
{
    [Header("Width")]
    public Slider maxWidthSlider;
    public TextMeshProUGUI widthValueText;

    [Header("Height")]
    public Slider maxHeightSlider;
    public TextMeshProUGUI heightValueText;

    private GameController gameController;
    private int width;
    private int height;

    public void Start()
    {
        gameController = GameController.Instance;
    }

    #region Called By Button Event
    public void Open()
    { 
        if(gameController == null) {
            gameController = GameController.Instance;
        }

        maxWidthSlider.minValue = gameController.mazeRender.minMazeSize;
        maxWidthSlider.maxValue = gameController.mazeRender.maxMazeSize;
        maxWidthSlider.value = gameController.mazeRender.width;
        widthValueText.text = maxWidthSlider.value.ToString();


        maxHeightSlider.minValue = gameController.mazeRender.minMazeSize;
        maxHeightSlider.maxValue = gameController.mazeRender.maxMazeSize;
        maxHeightSlider.value = gameController.mazeRender.height;
        heightValueText.text = maxHeightSlider.value.ToString();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ApplyChanges()
    {
        gameController.ChangeMazeSize(width, height);
        gameController.RestartGame();
        Close();
    }

    public void ChangeWidthSlider(float value)
    {
        width = (int)value;
        widthValueText.text = width.ToString();
    }
    public void ChangeHeightSlider(float value)
    {
        height = (int)value;
        heightValueText.text = height.ToString();
    }
    #endregion
}
