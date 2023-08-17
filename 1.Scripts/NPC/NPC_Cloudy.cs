using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Cloudy : NPC
{
    Animator animator;
    public Animator GuideUIAnimator;
    Transform talkPosition;

    Player player;

    public static NPC_Cloudy instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            animator = GetComponent<Animator>();
            talkPosition = transform.GetChild(0).transform;
            instance = this;
            istalkRight = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //SavePoint 트리거 안에 플레이어가 들어오면
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out player)) Debug.LogError("Player Componenet Missing");
            player.canTalk = true;
            player.talkPosition = this.talkPosition.position;

            player.npc = this;
            GuideUIAnimator.Play("Show");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            player.canTalk = false;
            GuideUIAnimator.Play("Hide");
        }
    }

    public override void TalkStart()
    {
        animator.Play("Open");
        GuideUIAnimator.Play("Hide");
    }

    public override void TalkFinished()
    {
        animator.Play("Close");
        GuideUIAnimator.Play("Show");
    }
}
