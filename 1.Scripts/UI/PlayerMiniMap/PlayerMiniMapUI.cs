using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniMapUI : MonoBehaviour
{
    public void ShowMiniMap()
    {
        gameObject.SetActive(true);
    }

    public void HideMiniMap()
    {
        gameObject.SetActive(false);
    }
}
