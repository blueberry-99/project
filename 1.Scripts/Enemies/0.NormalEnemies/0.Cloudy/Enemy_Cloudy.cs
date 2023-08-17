using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cloudy : Enemy
{

    void Awake()
    {

        InheritedAwake();
    }

    void Start()
    {
        InheritedStart();

    }

    public override void InheritedAwake()
    {
        base.InheritedAwake();
    }

    public override void InheritedStart()
    {
        base.InheritedStart();
    }


    //TODO : 적 AI 및 행동 스크립트 작성
}
