using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적들의 부모 클래스 > 모든 적들은 얘 상속 받아서 만들 것
public class Enemy1 : MonoBehaviour
{
    //플레이어한테 피격당했을 때, 피격 이펙트를 발생할 pos, rot

    //For Effects
    private Vector2 HitSlashEffectPos;
    private Quaternion HitSlashRotation;

    private Vector2 HitBackEffectPos;
    private Quaternion HitBackEffectRotation;

    private Vector2 HitBackParticlePos;
    private Quaternion HitBackParticleRotation;

    //for tint when hitted
    int tintPropertyID;
    float tintValue;
    [SerializeField] SpriteRenderer ColoredRenderer;
    Material coloredMaterial;
    [SerializeField] SpriteRenderer GrayScaledRenderer;
    Material grayScaledMaterial;


    //Components
    SpriteRenderer SpriteRenderer;
    Rigidbody2D RB;
    [SerializeField] Animator Animator;

    BoxCollider2D boxCollider2D;
    //private int dir;
    public float moveSpeed;


    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        coloredMaterial = ColoredRenderer.material;
        grayScaledMaterial = GrayScaledRenderer.material;
        tintPropertyID = Shader.PropertyToID("_StrongTintFade");
    }

    public void SetHitSlashEffect(Vector3 position, Quaternion rotation)
    {
        HitSlashEffectPos = boxCollider2D.ClosestPoint(position);
        HitSlashRotation = rotation;
    }

    public void SetHitBackEffect(Vector3 position, Quaternion rotation)
    {
        HitBackEffectPos = boxCollider2D.ClosestPoint(position);
        HitBackEffectRotation = rotation;
    }

    public void SetHitBackParticle(Vector3 position, Quaternion rotation)
    {
        HitBackParticlePos = boxCollider2D.ClosestPoint(position);
        HitBackParticleRotation = rotation;
    }

    public void AttackHitted(int hitDirX, int hitDirY)
    {
        //HitEffect
        PlayerDetachedEffectPool.instance.GetFromPool("HitSlashEffect", HitSlashEffectPos, HitSlashRotation);
        PlayerDetachedEffectPool.instance.GetFromPool("SlashColorMask", HitSlashEffectPos, HitSlashRotation);
        PlayerDetachedEffectPool.instance.GetFromPool("HitBackEffect", HitBackEffectPos, HitBackEffectRotation);
        PlayerDetachedEffectPool.instance.GetFromPool("HitBackParticle", HitBackParticlePos, HitBackParticleRotation);

        //ColorEffect
        //PlayerDetachedEffectPool.DetachedEffectPool.GetFromPool("PlayerColorMaskTrail", transform.position, Quaternion.identity);
        Animator.Rebind();
        Animator.Play("HitColorMask");

        coloredMaterial.SetFloat(tintPropertyID, 1);
        grayScaledMaterial.SetFloat(tintPropertyID, 1);
        StartCoroutine(ResetColor(0.15f));

        RB.AddForce(Vector2.right * (10 * hitDirX), ForceMode2D.Impulse);
    }

    private void Update()
    {

    }

    IEnumerator ResetColor(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        yield return wfs;
        coloredMaterial.SetFloat(tintPropertyID, 0);
        grayScaledMaterial.SetFloat(tintPropertyID, 0);
    }
}
