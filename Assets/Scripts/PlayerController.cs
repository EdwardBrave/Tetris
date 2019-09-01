using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.1F;
    public float rotationSpeed = 0.1F;

    public const byte NONE          = 0;
    public const byte UP            = 1;
    public const byte DOWN          = 2;
    public const byte LEFT          = 3;
    public const byte RIGHT         = 4;
    public const byte ROTATE_LEFT   = 5;
    public const byte ROTATE_RIGHT  = 6;
    private byte _activeEvent;

    private byte activeState
    {
        get => _activeEvent;
        set
        {
            StopCoroutine(nameof(Movement));
            _activeEvent = value;
            if (_activeEvent != NONE)
                StartCoroutine(nameof(Movement));
        }
    }

    IEnumerator Movement()
    {
        while(_activeEvent != NONE)
        {
            onClick(_activeEvent);
            if (_activeEvent == ROTATE_LEFT || _activeEvent == ROTATE_RIGHT)
                yield return new WaitForSeconds(rotationSpeed * Time.deltaTime * 3);
            else
                yield return new WaitForSeconds(moveSpeed * Time.deltaTime * 3);
        }
    }

    public void onClick(int buttonEvent)
    {
        switch (buttonEvent)
        {
            case UP:
                GameManager.Self.figure.MoveTo(Figure.Direction.Up);
                break;
            case DOWN:
                if (!GameManager.Self.figure.MoveTo(Figure.Direction.Down))
                    GameManager.Self.FlourIsReached();
                break;
            case LEFT:
                GameManager.Self.figure.MoveTo(Figure.Direction.Left);
                break;
            case RIGHT:
                GameManager.Self.figure.MoveTo(Figure.Direction.Right);
                break;
            case ROTATE_LEFT:
                GameManager.Self.figure.Rotate(false);
                break;
            case ROTATE_RIGHT:
                GameManager.Self.figure.Rotate();
                break;
        }
    }
    
    public void ButtonPressed(int buttonEvent)
    {
        if (buttonEvent != _activeEvent)
        {
            _activeEvent = (byte)buttonEvent;
            onClick(_activeEvent);
        }
    }
    
    public void ButtonReleased(int buttonEvent)
    {
        if (_activeEvent == buttonEvent)
        {
            _activeEvent = NONE;
            onClick(_activeEvent);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            activeState = DOWN;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            activeState = LEFT;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            activeState = RIGHT;
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.E))
            activeState = ROTATE_RIGHT;
        else if (Input.GetKeyDown(KeyCode.Q))
            activeState = ROTATE_LEFT;
        else if ( _activeEvent == DOWN && (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) ||
                  _activeEvent == LEFT && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) ||
                  _activeEvent == RIGHT && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) ||
                  _activeEvent == ROTATE_RIGHT && (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.E)) ||
                  _activeEvent == ROTATE_LEFT && (Input.GetKeyUp(KeyCode.Q)))
            activeState = NONE;
    }

}
