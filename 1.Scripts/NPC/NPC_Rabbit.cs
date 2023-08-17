using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Rabbit : MonoBehaviour

{
    Rigidbody2D RB;
    Animator Animator;
    public Vector2 JumpPower;

    private float counter;

    //int dir = 1;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        counter += Time.deltaTime;

        if (3 < counter && counter < 7)
        {
            Animator.Play("Rabbit_Staring_Up");
        }

        if (counter > 5)
        {
            Animator.Play("Rabbit_Staring_Down");
            counter = 0;
        }
        /* if (counter > 1)
        {
            Animator.Play("Rabbit_Jump");

            transform.localScale = new Vector3(dir, 1, 1);
            RB.velocity = new Vector2(JumpPower.x * dir, JumpPower.y);
            dir *= -1;
            counter = 0;
        } */
    }
}
