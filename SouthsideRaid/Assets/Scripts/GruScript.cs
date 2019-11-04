using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruScript : BossScript
{
    bool leftBarrierActive = true;
    bool rightBarrierActive = true;
    [SerializeField] float leftBarrierHealth;
    [SerializeField] float rightBarrierHealth;
    public bool canBeHit = true;
    [SerializeField] float damageAccrued = 0.0f; // check if dealt enough damage to go into exposed state during attack


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        leftBarrierHealth = 0.25f * maxHealth;
        rightBarrierHealth = 0.25f * maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        float threshold = 0.1f * maxHealth;

        // select a random lane to attack
        PlayerLaneState attackThisLane = (PlayerLaneState)Random.Range(0, 3);

        // if not already attacking
        if (!(bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Left Slam") ||
                bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Middle Slam") ||
                bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Right Slam")))
        {
            // play animation
            switch (attackThisLane)
            {
                case PlayerLaneState.Left:
                    bossAnimator.Play("Left Slam");
                    break;
                case PlayerLaneState.Middle:
                    bossAnimator.Play("Middle Slam");
                    break;
                case PlayerLaneState.Right:
                    bossAnimator.Play("Right Slam");
                    break;

                default: break;
            }
        }

        // play sound

        // instantiate attack prefab

        // check dmg threshold to go to exposed/down state
        if (damageAccrued >= threshold)
        {
            stance = Stances.Down;
            bossAnimator.SetInteger("State", (int)AnimStates.Opening);
        }


        // return to idle when attack ends
        if ((   bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Left Slam") || 
                bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Middle Slam") || 
                bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Right Slam")) 
                && bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99 && !bossAnimator.IsInTransition(0))
        {
            bossAnimator.SetInteger("State", (int)AnimStates.Idle);
            stance = Stances.Idle;
            // reset damageAccrued
            damageAccrued = 0.0f;
            return;
        }
    }

    public override float dealDamage(float _damage, PlayerLaneState _playerLane)
    {
        //Debug.Log("dealDamge in GRU");

        // canBeHit is set to false when erupting
        if (canBeHit)
        {
            // TODO: sum damageAccrued by correctly
            if (stance == Stances.Attack)
            {
                damageAccrued += _damage;
            }
            // if player is in blocked lane reduce damage
            if (_playerLane == PlayerLaneState.Left)
            {
                // deal reduced damage
                if (leftBarrierActive)
                {
                    return base.dealDamage(_damage * 0.25f, _playerLane);
                }

                // deal double damage
                else
                {
                    return base.dealDamage(_damage * 2.0f, _playerLane);
                }
            }

            // if player is in blocked lane reduce damage
            else if (_playerLane == PlayerLaneState.Right)
            {
                // deal reduced damage
                if (rightBarrierActive)
                {
                    return base.dealDamage(_damage * 0.25f, _playerLane);
                }

                // deal double damage
                else
                {
                    return base.dealDamage(_damage * 2.0f, _playerLane);
                }
            }
            else
            {
                return base.dealDamage(_damage, _playerLane);
            }
        }
        else return 0.0f;
    }

    private void Erupt()
    {
        bossAnimator.Play("Erupt", 0);
        canBeHit = false;
    }
}
