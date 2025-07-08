using System;
using Tick;
using UnityEngine;
using Tick.Inspector;
public class FingerTest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PressGestureEvent pressGesture;
    private DragGestureEvent dragGestureEvent;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (pressGesture == null)
        {
            pressGesture = GetComponent<PressGestureEvent>();
        }

        pressGesture.OnPressEvent += OnPress;

        if (dragGestureEvent == null)
        {
            dragGestureEvent = GetComponent<DragGestureEvent>();
        }
        dragGestureEvent.OnDragBeginningEvent += OnDragBeginningEvent;
        dragGestureEvent.OnDraggingEvent += OnDraggingEvent;
        dragGestureEvent.OnDragEndEvent += OnDragEndEvent;
    }

    private void OnDragBeginningEvent(FingerOperationData data)
    {
        spriteRenderer.color = Color.red;
    }

    private void OnDraggingEvent(FingerOperationData data)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(data.FingerScreenCurrenPos.x, data.FingerScreenCurrenPos.y, 10));
        transform.position = worldPosition;
    }

    private void OnDragEndEvent(FingerOperationData data)
    {
        spriteRenderer.color = Color.white;
    }

    private void OnPress(FingerOperationData data)
    {
        spriteRenderer.color = spriteRenderer.color == Color.blue ? Color.white : Color.blue;
    }
    private void OnDisable()
    {
        pressGesture.OnPressEvent -= OnPress;
    }
}
