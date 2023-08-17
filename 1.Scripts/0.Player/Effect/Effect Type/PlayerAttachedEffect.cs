using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachedEffect : MonoBehaviour
{
    
    public void DeleteEffect(float time)
    {
        Invoke(nameof(ReturnToPool), time);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        PlayerAttachedEffectPool.instance.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
