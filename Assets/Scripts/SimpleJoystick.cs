using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public RectTransform handle;
    public float handleRange = 50f;

    Vector2 inputVector;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out var pos))
        {
            pos = Vector2.ClampMagnitude(pos / (background.sizeDelta / 2), 1f);
            inputVector = pos;

            handle.anchoredPosition = inputVector * handleRange;

            if (player != null)
                player.SetMoveInput(inputVector);
        }
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        if (player != null)
            player.SetMoveInput(Vector2.zero);
    }
}
