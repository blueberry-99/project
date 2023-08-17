using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile_Boss_Queen : MonoBehaviour
{
    Enemy Enemy;
    Animator Animator;
    Rigidbody2D RB;

    private bool isPlayerHitted;

    public float speed;


    private void Awake()
    {
        if (!TryGetComponent<Rigidbody2D>(out RB)) Debug.LogError("RigidBody2d Missing");
        if (!TryGetComponent<Animator>(out Animator)) Debug.LogError("Animator Missing");
    }

    private void OnEnable()
    {
        isPlayerHitted = false;
    }

    private void OnDisable()
    {
        isPlayerHitted = false;
    }

    private void Update()
    {
        if (!isPlayerHitted) transform.Translate(Vector2.right * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //when enemy Hitted
        if (other.gameObject.tag.Equals("Player") && !isPlayerHitted)
        {
            isPlayerHitted = true;
            Animator.Play("SkillProjectile_Disappear");

            PlayerManager.instance.GetComponent<Player>().PlayerHit(1);
        }

    }
}
