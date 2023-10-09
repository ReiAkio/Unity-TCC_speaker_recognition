using System.Collections;
using UnityEngine;

public class PetAnimationManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool CanPlayerAct()
    {
        return Connection.statusCode == 200;
    }

    public void WalkRight()
    {
        animator.SetBool("WalkRight", true);
        animator.SetBool("WalkLeft", false);
    }

    public void WalkLeft()
    {
        animator.SetBool("WalkLeft", true);
        animator.SetBool("WalkRight", false);
    }

    public void StopWalking()
    {
        animator.SetBool("WalkRight", false);
        animator.SetBool("WalkLeft", false);
    }

    public void ExecuteOkRight()
    {
        animator.SetTrigger("Ok_Right");
    }

    public void ExecuteOkLeft()
    {
        animator.SetTrigger("Ok_Left");
    }
}