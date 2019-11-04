using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruScript : BossScript
{
    bool leftBarrierActive = true;
    bool rightBarrierActive = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        Debug.Log("GRU ATTACK");
    }

    //public void dealDamage(float _damage)
    //{
    //    Debug.Log("dealDamge in GRU");
    //    base.dealDamage(_damage);
    //}
}
