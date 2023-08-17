using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornCollider : MonoBehaviour
{
    public bool playerFacingRightAfterTP;
    public Transform TeleportTransform;

    Vector3 teleportPos = Vector3.zero;

    Player Player;
    Enemy enemy;

    private bool canHitPlayer;

    private void Awake()
    {
        canHitPlayer = true;
        teleportPos = TeleportTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHitPlayer)
        {
            SendHitToPlayer(other);
        }
        SendHitToEnemy(other);
    }
    /* 
        //무적 상태여도 가시에는 맞을 것이므로, 이거 필요없을듯?
        private void OnTriggerStay2D(Collider2D other)
        {
            if (canHitPlayer)
            {
                SendHitToPlayer(other);
            }
        } 
    */


    private void SendHitToPlayer(Collider2D other)
    {
        /* 
        캐릭터에게 전달해야 할 정보
        1. 순간이동해야 하는 좌표
        2. 순간이동 후 플레이어가 바라보아야 할 방향
         */

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out Player))
            {
                Player.PlayerHitByThorn(1, TeleportTransform.position, playerFacingRightAfterTP);
                //canHitPlayer = false;
            }
            else
            {
                Debug.LogError("Player Component Missing");
            }
        }
    }

    private void SendHitToEnemy(Collider2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitCollider"))
        {
            if (other.transform.parent.TryGetComponent<Enemy>(out enemy))
            {
                enemy.AttackHitted(0, 0, 9999f, 5f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(TeleportTransform.position, 0.5f);
    }
}
