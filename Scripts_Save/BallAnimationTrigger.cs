using UnityEngine;

public class BallAnimationTrigger : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("BallRoll"); // Replace "BallRoll" with your animation name
    }
}
