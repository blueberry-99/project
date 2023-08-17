using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScaledEnemy : MonoBehaviour
{
    Animator Animator;
    SpriteRenderer SpriteRenderer;
    Material Material;

    int PropertyID_DissolveScale;
    Vector2 DissolveScale;

    public void Start()
    {
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Material = SpriteRenderer.material;
        PropertyID_DissolveScale = Shader.PropertyToID("_FullGlowDissolveNoiseScale");

        DissolveScale = new Vector2(0.1f, 0.1f);
        Material.SetVector(PropertyID_DissolveScale, DissolveScale);

        //StartCoroutine(AdjustNoiseScale(0.05f));
    }
    //Play Same Animation from Original Colored Enemy
    public void PlayAnimation(string name)
    {
        Animator.Play(name);
    }

    //아니면 애니메이션이 바뀔 때마다,
    IEnumerator AdjustNoiseScale(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        while (true)
        {
            yield return wfs;
            DissolveScale = new Vector2(DissolveScale.x + 0.001f, DissolveScale.y + 0.001f);
            Material.SetVector(PropertyID_DissolveScale, DissolveScale);
        }
    }
}
