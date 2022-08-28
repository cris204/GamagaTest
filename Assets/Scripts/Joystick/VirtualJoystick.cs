using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image bgImage;
    public Image joystickImage;
    public Vector2 inputDir;
    public float offset;

#if !UNITY_ANDROID && !UNITY_IPHONE
    private void Start()
    {
        gameObject.SetActive(false);
    }
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputDir = Vector2.zero;
        joystickImage.rectTransform.anchoredPosition = inputDir;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;
        float bgImageSizeX = bgImage.rectTransform.sizeDelta.x;
        float bgImageSizeY = bgImage.rectTransform.sizeDelta.y;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos)) {
            pos.x /= bgImageSizeX;
            pos.y /= bgImageSizeY;

            inputDir = pos;
            inputDir = inputDir.magnitude > 1 ? inputDir.normalized : inputDir;

            joystickImage.rectTransform.anchoredPosition = new Vector2(inputDir.x * (bgImageSizeX / offset), inputDir.y * (bgImageSizeY / offset));
        }
    }


}
