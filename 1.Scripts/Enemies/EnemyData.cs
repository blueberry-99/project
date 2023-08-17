using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [Header("HP")]
    public int maxHP;

    [Header("Damage")]
    public int damage;

    [Header("Base Move Speed")]
    public float baseMoveSpeed;

    [Header("KnockBackAmount")]
    public float recoilToPlayerAmount; //공격이 플레이어를 넉백시키는 정도
    public float selfRecoilAmount; // 플레이어의 공격에 의해 넉백당하는 정도

    [Header("CollisionSize")]
    public Vector2 size;
    public CapsuleDirection2D direction;
    public float angle;


    [Header("Drop HP")]
    public float dropHpAmount; //죽었을 때 떨어지는 HP의 양.

    [Header("Drop Money or Exp")]
    public float dropMoneyAmount;

    [Header("Drop Item")]
    public float dropItem;
    //만약 dropItem이 true 이면, drop List도 넣어주기.
}
