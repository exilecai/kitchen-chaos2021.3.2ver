using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour

    
{
    //private const string IS_WALKING = "IsWalking";


    [SerializeField] private PlayerController player;

    private Animator animator;



    private void Awake()/*当游戏运行时执行*/
    {
        animator = GetComponent<Animator>();//参数得到组件

    }

    private void Update()
    {
        animator.SetBool("IsWalking", player.IsWalking());//将名为IsWalking的bool设置为PlayerController这个class中的IsWalking的值
    }

}
