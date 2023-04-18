using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isWalking;//����һ���Ƿ����ߵ�boolֵ
    private Vector3 lastInteractionDir;//���ڽ���
    private Clearcounter selectedCounter;

    [SerializeField] private float moveSpeed = 7f;//private��֤���ᱻ�����ű�ֱ�ӷ��ʺ��޸ģ���������������
    [SerializeField] private GameInput gameInput;//�����Ѿ�д�õ�Gameinput��
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
    
    
    
    //animator ����
    public bool IsWalking()
    {
        return isWalking;
    }
    
    
    
    //�ƶ�ϵͳ
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);//unity�е�y��ָ�����

        /*��ײ���*/
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);//return a bool value,CapsuleCast��Ҫ��ʼ�㣬�յ㣬�뾶���ƶ�������������

        //������ǽ��ʱֻ���x��zһ���������
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
            transform.position += moveDir * Time.deltaTime * moveSpeed;//���� Time.deltaTime �󣬿��Ա�֤������ƶ��ٶ�ʼ����ʱ�����Ӧ������֡�ʵ�Ӱ��
        }


        isWalking = moveDir != Vector3.zero;//ÿһ֡�ж�iswalking�Ƿ�Ϊtrue����movedir��Ϊ0���ƶ�ʱΪtrue

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);//Slerp��һ����ֵ������ָ������ʼֵ����ֵֹ����ָ����ʱ����ͨ�����Ի����߼��㣬�õ�һ���µ��м�ֵ��ʹ��ת���˿��
    }

        /*�����ľ���*/
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;//ʵ�ּ�ʹ���ƶ�Ҳ�ܱ�֤����
        }


        float interactDistance = 2f;
        
        if(Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit,interactDistance,countersLayerMask))//�������߼����ײ���ӵ�ǰλ�ã�������������������,���ҷ�����һ����ײ���Ķ���
        {
            if(raycastHit.transform.TryGetComponent(out Clearcounter clearCounter))//TryGetComponent��GetComponent������ǰ��Ĭ�������Ϊ�գ������ײ������������ű���������ص���һ��boolֵ
            {
                //��̨����
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
