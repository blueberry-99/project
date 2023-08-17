using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAttackRangeTrigger : MonoBehaviour
{
    Enemy Enemy;
    Collider2D Collider2D;
    // Start is called before the first frame update
    private void Awake()
    {
        Enemy = transform.GetComponentInParent<Enemy>();
        Collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Enemy.isPlayerInAttackRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.isPlayerInAttackRange = false;
        }
    }
}
