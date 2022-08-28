using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettingsController : MonoBehaviour
{
    public MixerController mixerController;

    [Header("Width")]
    public Slider maxWidthSlider;
    public TextMeshProUGUI widthValueText;

    [Header("Height")]
    public Slider maxHeightSlider;
    public TextMeshProUGUI heightValueText;

    [Header("Volume")]
    public Slider volumeSlider;

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
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_BUTTON_PATH, 0.5f);
        if (gameController == null) {
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

        volumeSlider.value = mixerController.GetVolume();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        PoolManager.Instance.GetObject(Env.AUDIO_SOURCE).GetComponent<PlaySound>().PlayAudio(Env.SOUND_BUTTON_PATH, 0.5f);
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
    public void ChangeVolumeSlider(float value)
    {
        mixerController.SetVolume(value);
    }

    #endregion
}
