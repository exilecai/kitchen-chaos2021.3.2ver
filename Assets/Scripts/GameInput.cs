using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();//这个player.enable是 PlayerInputActions中的函数,用于启动player action map

        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(OnInteractAction != null) {
            OnInteractAction(this, EventArgs.Empty);
        }
        
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();


        /*   Vector2 inputVector = new Vector2(0, 0);//实际输入只有两个轴
           if (Input.GetKey(KeyCode.W))
           {
               inputVector.y = 1;
           }
           if (Input.GetKey(KeyCode.S))
           {
               inputVector.y = -1;
           }
           if (Input.GetKey(KeyCode.A))
           {
               inputVector.x = -1;
           }
           if (Input.GetKey(KeyCode.D))
           {
               inputVector.x = +1;
           }
   */


        inputVector = inputVector.normalized;//保证向量永远是单位向量 不会使对角线移动速度过快

        return inputVector;
    }
}
