using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_GhostPainter : NPC
{
    public Dialogue dialogue2;

    public Item item_Glass;

    Animator animator;
    public Animator GuideUIAnimator;
    Transform talkPosition;

    Player player;

    public static NPC_GhostPainter instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            animator = GetComponent<Animator>();
            GuideUIAnimator = transform.GetChild(1).GetComponent<Animator>();
            talkPosition = transform.GetChild(0).transform;
            instance = this;
            istalkRight = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out player)) Debug.LogError("Player Componenet Missing");
            player.canTalk = true;
            player.talkPosition = this.talkPosition.position;
            player.npc = this;
            GuideUIAnimator.SetTrigger("show");
            if (PlayerManager.instance.isGlassGained)
            {
                dialogue = dialogue2;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            player.canTalk = false;
            GuideUIAnimator.SetTrigger("hide");
        }
    }

    public override void TalkStart()
    {
        animator.SetTrigger("enter");
        if (!PlayerManager.instance.isGlassGained)
        {
            player.TalkState.talkStateItem = item_Glass;
            player.TalkState.isItemGet = true;
            player.TalkState.itemDialogCount = 1;
        }
    }

    public override void HideGuide()
    {
        base.HideGuide();
        GuideUIAnimator.SetTrigger("hide");
    }

    public override void TalkFinished()
    {
        animator.SetTrigger("exit");
        GuideUIAnimator.SetTrigger("show");
        if (PlayerManager.instance.isGlassGained)
        {
            dialogue = dialogue2;
        }
    }
}
