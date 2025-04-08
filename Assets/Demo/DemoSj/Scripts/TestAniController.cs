using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAniController : MonoBehaviour
{
    private Animator animator;
    private float loopUseTime;
    private float loopEndTime = 2f;
    private float chaseUseTime;
    private float chaseEndTime = 2f;
    private bool loopAttack;
    private bool chase;
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        

    }

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

}
