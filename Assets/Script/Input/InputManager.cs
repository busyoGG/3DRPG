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
    None,
    Down,
    Up,
    Hold
}

public class InputManager : Singleton<InputManager>
{
    public delegate void InputCallback();

    InputCallback _callbackHandler;

    //Dictionary<string, InputCallback> _callbacks = new Dictionary<string, InputCallback>();

    private Dictionary<KeyCode, InputStatus> _curKey = new Dictionary<KeyCode, InputStatus>();

    private Dictionary<int, InputStatus> _curMouse = new Dictionary<int, InputStatus>();

    public void Init()
    {
        GameObject inputSystem = new GameObject();
        inputSystem.name = "InputSystem";

        inputSystem.AddComponent<InputScript>();

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            KeyListener(key);
        }

        for (int i = 0; i < 3; i++)
        {
            MouseListener(i);
        }
    }

    private void KeyListener(KeyCode key)
    {
        //_curKey.Add(key, InputStatus.None);
        InputCallback inputCallback = () =>
        {
            if (Input.GetKeyDown(key))
            {
                //_curKey[key] = InputStatus.Down;
                _curKey.Add(key, InputStatus.Down);
            }
            else if (Input.GetKeyUp(key))
            {
                _curKey[key] = InputStatus.Up;
            }
            else if (Input.GetKey(key))
            {
                _curKey[key] = InputStatus.Hold;
            }
            else
            {
                //_curKey[key] = InputStatus.None;
                _curKey.Remove(key);
            }
        };

        if (_callbackHandler != null)
        {
            _callbackHandler += inputCallback;
        }
        else
        {
            _callbackHandler = inputCallback;
        }
    }

    private void MouseListener(int key)
    {
        //_curMouse.Add(key, InputStatus.None);
        InputCallback inputCallback = () =>
        {
            if (Input.GetMouseButtonDown(key))
            {
                //_curMouse[key] = InputStatus.Down;
                _curMouse.Add(key, InputStatus.Down);
            }
            else if (Input.GetMouseButtonUp(key))
            {
                _curMouse[key] = InputStatus.Up;
            }
            else if (Input.GetMouseButton(key))
            {
                _curMouse[key] = InputStatus.Hold;
            }
            else
            {
                //_curMouse[key] = InputStatus.None;
                _curMouse.Remove(key);
            }
        };

        if (_callbackHandler != null)
        {
            _callbackHandler += inputCallback;
        }
        else
        {
            _callbackHandler = inputCallback;
        }
    }

    public Dictionary<KeyCode, InputStatus> GetKey()
    {
        return _curKey;
    }

    public Dictionary<int, InputStatus> GetMouse()
    {
        return _curMouse;
    }

    public void CheckInput()
    {
        if (_callbackHandler != null)
        {
            _callbackHandler.Invoke();
        }
    }

    public void AddEventListener(InputCallback callback)
    {
        _callbackHandler += callback;
    }

    public void RemoveEventListener(InputCallback callback)
    {
        _callbackHandler -= callback;
    }
}
