using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    
    public void DeleteEffect(float time)
    {
        Invoke(nameof(ReturnToPool), time);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        GameObjectPool.gameObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
