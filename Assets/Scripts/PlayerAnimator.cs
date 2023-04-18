using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour

    
{
    //private const string IS_WALKING = "IsWalking";


    [SerializeField] private PlayerController player;

    private Animator animator;



    private void Awake()/*����Ϸ����ʱִ��*/
    {
        animator = GetComponent<Animator>();//�����õ����

    }

    private void Update()
    {
        animator.SetBool("IsWalking", player.IsWalking());//����ΪIsWalking��bool����ΪPlayerController���class�е�IsWalking��ֵ
    }

}
