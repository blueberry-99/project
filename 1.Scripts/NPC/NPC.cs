using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    
    public int itemGiveCount;
    public bool istalkRight;

    public virtual void TalkStart()
    {

    }

    public virtual void TalkFinished()
    {

    }

    public virtual void HideGuide()
    {

    }
}
