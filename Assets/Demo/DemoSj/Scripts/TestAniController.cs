using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAniController : MonoBehaviour
{
    // TODO: LJH
    public int ID;
    // ~TODO

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        // TODO: LJH
        //transform.localPosition = Vector3.zero;
        // ~TODO
    }
    // ~TODO

    [SerializeField] private Animator animator;
    private float loopUseTime;
    private float loopEndTime = 2f;
    private float chaseUseTime;
    private float chaseEndTime = 2f;
    private bool loopAttack;
    private bool chase;
       
    public void OnAttck()
    {
        animator.SetTrigger("Attack");
    }

    public void OnOffLoopAttck()
    {
        if (!loopAttack)
        {
            loopAttack = true;
            animator.SetBool("LoopAttack", loopAttack);
        }
        else
        {
            loopAttack = false;
            animator.SetBool("LoopAttack", loopAttack);
        }
    }

    public void OnChase()
    {
        if (!chase)
        {
            chase = true;
            animator.SetBool("Chase", chase);
        }
        else
        {
            chase = false;
            animator.SetBool("Chase", chase);
        }
    }

    // TODO: LJH
    public void OnChase(bool isChase)
    {
        chase = isChase;
        animator.SetBool("Chase", chase);
    }
    // ~TODO

    public void OnHit()
    {
        animator.SetTrigger("Hit");
    }

    public void OnDrop()
    {
        animator.SetTrigger("Drop");
    }

    public void OnDead()
    {
        animator.SetTrigger("Dead");        
    }

    // TODO: LJH
    //public void OnDeadAnimationEnd()
    //{
    //    Destroy(gameObject);
    //}
    // ~TODO
}
