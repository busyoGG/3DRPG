using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Mouse,
    Keyboard,
    Controller
}

public enum InputStatus
{
    Down,
    Up,
    Hold
}

public class InputManager : Singleton<InputManager>
{
    delegate void InputCallback();

    InputCallback _callbackHandler;

    Dictionary<string, InputCallback> _callbacks = new Dictionary<string, InputCallback>();

    public void Init()
    {
        GameObject inputSystem = new GameObject();
        inputSystem.name = "InputSystem";

        inputSystem.AddComponent<InputScript>();
    }

    public void CheckInput()
    {
        if (_callbackHandler != null)
        {
            _callbackHandler.Invoke();
        }
    }

    public void AddKeyboardInputCallback(KeyCode key, InputStatus state, Action callback)
    {
        string id = key.GetHashCode() + "-" + callback.GetHashCode();

        InputCallback inputCallback = () =>
        {
            switch (state)
            {
                case InputStatus.Down:
                    if (Input.GetKeyDown(key))
                    {
                        callback();
                    }
                    break;
                case InputStatus.Up:
                    if (Input.GetKeyUp(key))
                    {
                        callback();
                    }
                    break;
                case InputStatus.Hold:
                    if (Input.GetKey(key))
                    {
                        callback();
                    }
                    break;
            }

        };

        _callbacks.Add(id, inputCallback);
        if (_callbackHandler != null)
        {
            _callbackHandler += inputCallback;
        }
        else
        {
            _callbackHandler = inputCallback;
        }
    }

    public void AddMouseInputCallback(int key, InputStatus state, Action callback)
    {
        string id = key.GetHashCode() + "-" + callback.GetHashCode();

        InputCallback inputCallback = () =>
        {

            switch (state)
            {
                case InputStatus.Down:
                    if (Input.GetMouseButtonDown(key))
                    {
                        callback();
                    }
                    break;
                case InputStatus.Up:
                    if (Input.GetMouseButtonUp(key))
                    {
                        callback();
                    }
                    break;
                case InputStatus.Hold:
                    if (Input.GetMouseButton(key))
                    {
                        callback();
                    }
                    break;
            }
        };

        _callbacks.Add(id, inputCallback);
        if (_callbackHandler != null)
        {
            _callbackHandler += inputCallback;
        }
        else
        {
            _callbackHandler = inputCallback;
        }
    }

    public void RemoveInputCallback(KeyCode key, Action callback)
    {
        string id = key.GetHashCode() + "-" + callback.GetHashCode();
        InputCallback action;
        _callbacks.TryGetValue(id, out action);
        _callbackHandler -= action;
        _callbacks.Remove(id);
    }

    public void RemoveInputCallback(int key, Action callback)
    {
        string id = key.GetHashCode() + "-" + callback.GetHashCode();
        InputCallback action;
        _callbacks.TryGetValue(id, out action);
        _callbackHandler -= action;
        _callbacks.Remove(id);
    }
}
