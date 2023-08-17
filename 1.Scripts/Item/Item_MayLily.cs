using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MayLily : Item
{
    BoxCollider2D boxCollider2D;
    Player player;
    Animator animator;
    public Animator guideAnimator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        animator.Play("Appear");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //SavePoint 트리거 안에 플레이어가 들어오면
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out player)) Debug.LogError("Player Componenet Missing");
            player.canItemRoot = true;
            player.itemRootPosition = transform.position;
            player.item = this;
            guideAnimator.Play("Show");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out player)) Debug.LogError("Player Componenet Missing");
            player.canItemRoot = false;
            guideAnimator.Play("Hide");
        }
    }

    public override void PlayerGetItem()
    {
        base.PlayerGetItem();
        ItemUI.instance.ShowItemUI(this);
        PlayerManager.instance.canSkill = true;
        PlayerManager.instance.canInAirDash = true;
        PlayerManager.instance.canWallJump = true;

        boxCollider2D.enabled = false;
        player.canItemRoot = false;
        animator.Play("Disappear");
        guideAnimator.Play("Hide");
    }
}
