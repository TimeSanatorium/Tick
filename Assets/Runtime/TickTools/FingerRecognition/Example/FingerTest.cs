using System;
using System.Collections;
using System.Collections.Generic;
using Tick;
using UnityEngine;

public class FingerTest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PressGestureEvent pressGesture;
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
    }

    private void OnPress()
    {
        spriteRenderer.color = spriteRenderer.color == Color.red ? Color.white : Color.red;
    }
    private void OnDisable()
    {
        pressGesture.OnPressEvent -= OnPress;
    }
}
