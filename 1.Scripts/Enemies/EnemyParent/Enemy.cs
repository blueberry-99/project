using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적 부모 클래스
public class Enemy : MonoBehaviour
{
    public bool isDie;
    //Enemy Data (Scriptable Object)
    public EnemyData EnemyData;
    private EnemyHitCollider enemyHitCollider;
    private PlayerDetectTrigger playerDetectTrigger;

    public bool isPlayerDetected;
    public bool isPlayerInAttackRange;
    public bool isAttacked;
    public bool isPlayerHitted;
    public bool isFacingRight;

    public bool canAttack;

    //Components
    public Rigidbody2D RB;
    protected Collider2D Collider2D;

    //Colored 
    protected SpriteRenderer ColoredSpriteRenderer;
    protected Material ColoredMaterial;

    //GrayScaled
    public SpriteRenderer GrayScaledSpriteRenderer;
    protected Material GrayScaledMaterial;

    private bool isRecoiling;

    public bool canBeRecoiled;

    //PropertyID
    protected int fullGlowDissolveFadeID;
    protected int strongTintFadeID;
    protected int squishFadeID;

    //PropertyValue
    protected float tintFadeValue;
    protected float dieFadeValue;

    //Enemy Variables
    protected float maxHP;
    protected float currentHP;
    protected float normalizedCurrentHP;

    protected bool canBeHitted;

    private Vector2 HitSlashEffectPos;
    private Quaternion HitSlashRotation;

    private Vector2 HitBackEffectPos;
    private Quaternion HitBackEffectRotation;

    private Vector2 HitBackParticlePos;
    private Quaternion HitBackParticleRotation;

    public virtual void InheritedAwake()
    {
        //Get Public Scripts and Components
        if (EnemyData == null) Debug.LogError("Enemy Data Missing");
        if (GrayScaledSpriteRenderer == null) Debug.LogError("GrayScaledSpriteRenderer Missing");
        enemyHitCollider = GetComponentInChildren<EnemyHitCollider>();
        if (GetComponentInChildren<PlayerDetectTrigger>() != null) playerDetectTrigger = GetComponentInChildren<PlayerDetectTrigger>();
        if (!TryGetComponent<Rigidbody2D>(out RB)) Debug.LogError(gameObject.name + " > 'Rigidbody2D' Component Missing");
        if (!TryGetComponent<Collider2D>(out Collider2D)) Debug.LogError(gameObject.name + " > 'Collider2D' Component Missing");
        if (!TryGetComponent<SpriteRenderer>(out ColoredSpriteRenderer)) Debug.LogError(gameObject.name + " > 'SpriteRenderer' Component Missing");

        ColoredMaterial = ColoredSpriteRenderer.material;
        GrayScaledMaterial = GrayScaledSpriteRenderer.material;

        fullGlowDissolveFadeID = Shader.PropertyToID("_FullGlowDissolveFade");
        strongTintFadeID = Shader.PropertyToID("_StrongTintFade");
        squishFadeID = Shader.PropertyToID("_SquishFade");
        canBeRecoiled = true;
        //Colored 랑 GrayScaled 모두 tint 필요하고, (tintpropertyID)
        //GrayScaled는 적 체력 상황에 맞춰서 dissolve 수치 적용해야함.

        SetData();

    }

    public void Move(Vector2 temp)
    {
        if (!isRecoiling)
        {
            RB.velocity = temp;
            //RB.AddForce(temp, ForceMode2D.Force);
        }

        /* switch (type)
        {
            case "Force":
                Debug.Log("Force Applied");
                RB.AddForce(temp, ForceMode2D.Force);
                break;
            case "Impulse":
                RB.AddForce(temp, ForceMode2D.Impulse);
                break;
        } */
    }
    public void SetAttackCollider(Vector2 pos, float rotation)
    {
        enemyHitCollider.transform.localPosition = pos;
        enemyHitCollider.transform.localRotation = Quaternion.Euler(0, 0, rotation);


    }

    public void StartAttackCoolDown(float time)
    {
        StartCoroutine(AttackCoolDown(time));
    }

    IEnumerator AttackCoolDown(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        canAttack = false;
        yield return wfs;
        canAttack = true;
    }

    void SetData()
    {
        maxHP = EnemyData.maxHP;
        canBeHitted = true;
    }

    public virtual void InheritedStart()
    {
        currentHP = maxHP;
        normalizedCurrentHP = currentHP / maxHP;
        GrayScaledMaterial.SetFloat(fullGlowDissolveFadeID, normalizedCurrentHP);
        canAttack = true;
    }


    IEnumerator StartCustomUpdate(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        while (true)
        {
            CustomUpdate();
            yield return wfs;
        }
    }

    //Called Every Custom UpdateTime
    private void CustomUpdate()
    {
    }

    public void FlipToRight()
    {
        isFacingRight = true;
        transform.localScale = new Vector3(-1, 1, 1);
    }

    public void FlipToLeft()
    {
        isFacingRight = false;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void CheckIfPlayerHitted()
    {

    }

    public void SetHitSlashEffect(Vector3 position, Quaternion rotation)
    {
        HitSlashEffectPos = Collider2D.ClosestPoint(position);
        HitSlashRotation = rotation;
    }

    public void SetHitBackEffect(Vector3 position, Quaternion rotation)
    {
        HitBackEffectPos = Collider2D.ClosestPoint(position);
        HitBackEffectRotation = rotation;
    }

    public void SetHitBackParticle(Vector3 position, Quaternion rotation)
    {
        HitBackParticlePos = Collider2D.ClosestPoint(position);
        HitBackParticleRotation = rotation;
    }

    public void AttackHitted(int hitDirX, int hitDirY, float damage, float recoilAmount)
    {
        isAttacked = true;

        GenerateHitEffect();
        if (canBeRecoiled) Recoil(hitDirX, hitDirY, recoilAmount);
        Damaged(damage);
        StartCoroutine(StartTinting(0.1f));
        if (hitDirY == -1)
        {
            StartCoroutine(StartSquishing(0.15f));
        }
    }

    IEnumerator StartSquishing(float squishTime)
    {
        WaitForSeconds wfs = new WaitForSeconds(squishTime);
        GrayScaledMaterial.SetFloat(squishFadeID, 1);
        ColoredMaterial.SetFloat(squishFadeID, 1);
        yield return wfs;
        GrayScaledMaterial.SetFloat(squishFadeID, 0);
        ColoredMaterial.SetFloat(squishFadeID, 0);
    }

    private void Damaged(float damage)
    {
        currentHP -= damage;
        normalizedCurrentHP = currentHP / maxHP;
        GrayScaledMaterial.SetFloat(fullGlowDissolveFadeID, normalizedCurrentHP);
        if (currentHP <= 0) Die();

    }

    private void Die()
    {
        canBeHitted = false;
        enemyHitCollider.EnemyTriggerOff();
        StartCoroutine(TimeFreeze(0.15f));

        isDie = true;

        //PlayerDetachedEffectPool.DetachedEffectPool.GetFromPool("EnemyDieParticle_Red", transform.position, Quaternion.identity);
        //StartCoroutine(DieFade());
        //Destroy(gameObject, 1f);

    }

    IEnumerator DieFade()
    {
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();
        WaitForSeconds wfs = new WaitForSeconds(1);
        dieFadeValue = 1;

        while (dieFadeValue > 0)
        {
            dieFadeValue -= Time.deltaTime * 6;
            ColoredMaterial.SetFloat(fullGlowDissolveFadeID, dieFadeValue);
            yield return wfef;
        }
        yield return wfs;
    }

    IEnumerator StartTinting(float tintTime)
    {
        WaitForSeconds wfs = new WaitForSeconds(tintTime);
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();

        tintFadeValue = 1;
        GrayScaledMaterial.SetFloat(strongTintFadeID, tintFadeValue);
        ColoredMaterial.SetFloat(strongTintFadeID, tintFadeValue);

        yield return wfs;

        while (tintFadeValue > 0)
        {
            tintFadeValue -= Time.deltaTime * 10;
            GrayScaledMaterial.SetFloat(strongTintFadeID, tintFadeValue);
            ColoredMaterial.SetFloat(strongTintFadeID, tintFadeValue);
            yield return wfef;
        }

        //시간이 주어지면, normalized value는 1부터 시작해서, 0까지 주어진 시간에 맞추어 도달해야 함.

        /* GrayScaledMaterial.SetFloat(strongTintFadeID, 0);
        ColoredMaterial.SetFloat(strongTintFadeID, 0); */

    }

    private void GenerateHitEffect()
    {
        PlayerDetachedEffectPool.instance.GetFromPool("HitSlashEffect", HitSlashEffectPos, HitSlashRotation);
        PlayerColorMaskPool.instance.GetFromPool("ColorMask", HitSlashEffectPos, HitSlashRotation);
        PlayerDetachedEffectPool.instance.GetFromPool("HitBackEffect", HitBackEffectPos, HitBackEffectRotation);
        PlayerDetachedEffectPool.instance.GetFromPool("HitBackParticle", HitBackParticlePos, HitBackParticleRotation);
        //PlayerDetachedEffectPool.DetachedEffectPool.GetFromPool("SlashColorMask", HitSlashEffectPos, HitSlashRotation);
    }

    private void Recoil(int hitDirX, int hitDirY, float recoilAmount)
    {
        isRecoiling = true;
        StartCoroutine(RecoilTimer());
        //X Recoil
        if (hitDirX != 0)
        {
            RB.AddForce(Vector2.right * (recoilAmount * hitDirX), ForceMode2D.Impulse);
        }


        //Y Recoil
        if (hitDirY == 1 || hitDirY == -1)
            RB.AddForce(Vector2.up * (recoilAmount * hitDirY), ForceMode2D.Impulse);
    }
    IEnumerator RecoilTimer()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        yield return wfs;
        isRecoiling = false;
        //TODO...
        RB.velocity = new Vector2(0, RB.velocity.y);
    }

    IEnumerator TimeFreeze(float time)
    {
        Time.timeScale = 0f;
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(time);
        yield return wfsr;
        Time.timeScale = 1f;
    }
}
