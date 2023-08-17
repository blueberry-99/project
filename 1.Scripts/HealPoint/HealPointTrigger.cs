using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPointTrigger : MonoBehaviour
{
    Player Player;
    Vector2 healPointPosition;

    void Awake()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //SavePoint 트리거 안에 플레이어가 들어오면
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out Player)) Debug.LogError("Player Componenet Missing");
            healPointPosition = transform.GetChild(0).transform.position;
            Player.SetCanHeal(true);
            Player.SetHealPointPosition(healPointPosition);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Player.SetCanHeal(false);
        }
    }
}
