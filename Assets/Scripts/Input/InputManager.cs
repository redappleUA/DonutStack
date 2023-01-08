using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public event EndTouchEvent OnEndTouch;


    private Touch touchContorls;

    public void Awake()
    {
        touchContorls = new();
    }

    private void OnEnable()
    {
        touchContorls.Enable();
    }

    private void OnDisable()
    {
        touchContorls.Disable();
    }

    private void Start()
    {
        touchContorls.TouchControl.TouchPress.started += ctx => StartTouch(ctx);
        touchContorls.TouchControl.TouchPress.started += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(touchContorls.TouchControl.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(touchContorls.TouchControl.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }
}
