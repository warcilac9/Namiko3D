using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputHandler : MonoBehaviour
{
PlayerInput input;

[Header("MoveValues")]
Vector2 movementValue = Vector2.zero;

[HideInInspector]public float moveValX;
[HideInInspector]public float moveValY;



public delegate void Movement();
public Movement moving;

public delegate void Jumped();
public Jumped jumping;
 


public delegate void attackType(int value);
public attackType typeAttack;

private int punchID = 0;
private int kickID = 1;
private int magicID = 2;


public void Move(InputAction.CallbackContext context)
{
    movementValue = context.ReadValue<Vector2>().normalized;
    moveValX = movementValue.x;
    moveValY = movementValue.y;
    if (context.performed)
    {
        moving?.Invoke();
    }
}

public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            jumping?.Invoke();
        }
    }

public void Kick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            typeAttack?.Invoke(kickID);
        }
    }

public void Magic(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            typeAttack?.Invoke(magicID);
        }
    }

public void Punch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            typeAttack?.Invoke(punchID);
        }
    }

}


