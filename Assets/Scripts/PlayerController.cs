using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isWalking;//定义一个是否行走的bool值
    private Vector3 lastInteractionDir;//用于交互
    private Clearcounter selectedCounter;

    [SerializeField] private float moveSpeed = 7f;//private保证不会被其他脚本直接访问和修改，避免出现意外错误。
    [SerializeField] private GameInput gameInput;//引入已经写好的Gameinput类
    [SerializeField] private LayerMask countersLayerMask;


    public event EventHandler OnSelectedCounterChanged;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {

        HandleMovement();
        HandleInteractions();
    }
    
    
    
    //animator 调用
    public bool IsWalking()
    {
        return isWalking;
    }
    
    
    
    //移动系统
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);//unity中的y轴指向天空

        /*碰撞检测*/
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);//return a bool value,CapsuleCast需要起始点，终点，半径，移动方向，最大检测距离

        //当碰到墙体时只检测x和z一个轴的输入
        if (!canMove)
        {

            Vector3 moveDirx = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirx, moveDistance);
            if (canMove)
            {

                moveDir = moveDirx;
            }
            else
            {
                //Debug.Log("Can't move in X axis. Trying Z axis...");
                Vector3 moveDirz = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirz, moveDistance);
                if (canMove)
                {

                    moveDir = moveDirz;
                }
                else
                {

                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;//加上 Time.deltaTime 后，可以保证物体的移动速度始终与时间相对应，不受帧率的影响
        }


        isWalking = moveDir != Vector3.zero;//每一帧判断iswalking是否为true，当movedir不为0即移动时为true

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);//Slerp是一个插值，根据指定的起始值和终止值，在指定的时间内通过线性或曲线计算，得到一个新的中间值。使得转向更丝滑
    }

        /*交互的距离*/
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;//实现即使不移动也能保证交互
        }


        float interactDistance = 2f;
        
        if(Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit,interactDistance,countersLayerMask))//发射射线检测碰撞，从当前位置，朝向，最大距离三个参数,并且返回了一个碰撞到的对象
        {
            if(raycastHit.transform.TryGetComponent(out Clearcounter clearCounter))//TryGetComponent与GetComponent区别在前者默认组件不为空，如果碰撞的物体有这个脚本组件，返回的是一个bool值
            {
                //柜台存在
                /*clearCounter.Interact();*/
                if(clearCounter != selectedCounter)
                {
                    selectedCounter = clearCounter;
                }
                
            }
            else
            {
                selectedCounter = null;
            }
        }
        else
        {
            selectedCounter = null;
        }
        Debug.Log(selectedCounter);

    }
}
