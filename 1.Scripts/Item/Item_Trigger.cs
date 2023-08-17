using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Trigger : MonoBehaviour
{
    public Transform itemTransform;
    Player player;
    void OnTriggerEnter2D(Collider2D other)
    {
        //SavePoint 트리거 안에 플레이어가 들어오면
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out player)) Debug.LogError("Player Componenet Missing");

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
        }
    }
}
