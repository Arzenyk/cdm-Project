using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColmillosScript : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        SetEmptyState();
    }

    public void OnAttackEvent()
    {
        PlayAttackAnimation();
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
        Debug.LogError("Andaaaa");
    }

    private void SetEmptyState()
    {
        animator.SetBool("IsEmpty", true);
    }
}
