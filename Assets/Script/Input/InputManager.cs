using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputManager : Singleton<InputManager>
{
    public delegate void InputCallback();

    InputCallback _callbackHandler;

    //Dictionary<string, InputCallback> _callbacks = new Dictionary<string, InputCallback>();

    private Dictionary<InputKey, InputStatus> _curKey = new Dictionary<InputKey, InputStatus>();

    public void Init()
    {
        GameObject inputSystem = new GameObject();
        inputSystem.name = "InputSystem";

        inputSystem.AddComponent<InputScript>();

        foreach (InputKey key in Enum.GetValues(typeof(InputKey)))
        {
            if (key == InputKey.MouseLeft || key == InputKey.MouseRight || key == InputKey.MouseMid)
            {
                MouseListener(key);
            }
            else
            {
                KeyListener(key);
            }
        }
    }

    private void KeyListener(InputKey key)
    {
        KeyCode transKey = (KeyCode)key;
        InputCallback inputCallback = () =>
        {
            if (Input.GetKeyDown(transKey))
            {
                _curKey.Add(key, InputStatus.Down);
            }
            else if (Input.GetKeyUp(transKey))
            {
                _curKey[key] = InputStatus.Up;
            }
            else if (Input.GetKey(transKey))
            {
                _curKey[key] = InputStatus.Hold;
            }
            else
            {
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

    private void MouseListener(InputKey key)
    {
        int transKey = Mathf.Abs((int)key) - 1;
        InputCallback inputCallback = () =>
        {
            if (Input.GetMouseButtonDown(transKey))
            {
                //_curMouse[key] = InputStatus.Down;
                _curKey.Add(key, InputStatus.Down);
            }
            else if (Input.GetMouseButtonUp(transKey))
            {
                _curKey[key] = InputStatus.Up;
            }
            else if (Input.GetMouseButton(transKey))
            {
                _curKey[key] = InputStatus.Hold;
            }
            else
            {
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

    public Dictionary<InputKey, InputStatus> GetKey()
    {
        return _curKey;
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
