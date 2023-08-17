using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCollider : MonoBehaviour
{
    Player Player;
    Enemy Enemy;

    private bool canHitPlayer;

    private void Awake()
    {
        Enemy = transform.GetComponentInParent<Enemy>();
        canHitPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHitPlayer)
        {
            SendHitToPlayer(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (canHitPlayer)
        {
            SendHitToPlayer(other);
        }
    }

    private void SendHitToPlayer(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.isPlayerHitted = true;

            if (other.TryGetComponent<Player>(out Player))
            {
                Player.PlayerHit(Enemy.EnemyData.damage);
            }
            else
            {
                Debug.LogError("Player Component Missing");
            }
        }
    }

    public void EnemyTriggerOff()
    {
        canHitPlayer = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
