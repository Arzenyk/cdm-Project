using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondeScript : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetIdleState();
    }

    public void OnAttackEvent()
    {
        PlayAttackAnimation();
    }

    public void OnDamageEvent()
    {
        PlayDamageAnimation();
    }

    public void OnDefeatedEvent()
    {
        PlayDefeatedAnimation();
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
        SetIdleStateAfterDelay(1.0f); // Adjust the delay to match the length of the attack animation
    }

    public void PlayDamageAnimation()
    {
        animator.SetTrigger("Damage");
        SetIdleStateAfterDelay(1.0f); // Adjust the delay to match the length of the damage animation
    }

    public void PlayDefeatedAnimation()
    {
        animator.SetTrigger("Defeated");
    }

    private void SetIdleStateAfterDelay(float delay)
    {
        Invoke("SetIdleState", delay);
    }

    private void SetIdleState()
    {
        animator.SetBool("IsIdle", true);
    }

    // Example method to simulate game events
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Simulate attack event
        {
            PlayAttackAnimation();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Simulate damage event
        {
            PlayDamageAnimation();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) // Simulate defeated event
        {
            PlayDefeatedAnimation();
        }
    }
}
