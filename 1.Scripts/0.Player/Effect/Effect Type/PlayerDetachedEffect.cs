using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetachedEffect : MonoBehaviour
{
    public void DeleteEffect(float time)
    {
        Invoke(nameof(ReturnToPool), time);
    }
    
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        PlayerDetachedEffectPool.instance.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
