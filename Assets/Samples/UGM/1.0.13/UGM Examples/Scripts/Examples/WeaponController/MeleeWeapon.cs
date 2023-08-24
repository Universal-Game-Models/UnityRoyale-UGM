﻿using System.Collections;
using UnityEngine;

namespace UGM.Examples.WeaponController
{
    public class MeleeWeapon : Weapon
    {
        private static readonly int MeleeAttackHash = Animator.StringToHash("MeleeAttack");
        private MeleeWeaponType weaponType;
        private GameObject recentlyHit;
        private float attackDuration = 0.75f;

        public void Init(int damage, MeleeWeaponType weaponType)
        {
            this.damage = damage;
            this.weaponType = weaponType;

            //Add a rigidbody to detect child colliders
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public override void Attack()
        {
            base.Attack();
            //Tell the animator to do a melee attack
            if (animator)
            {
                animator.SetTrigger(MeleeAttackHash);
            }
            //Wait for the attack to complete
            StartCoroutine(WaitToStopAttacking());
        }

        public override void StopAttacking()
        {
            //Dont call the base as it will set isAttacking to false onMouseUp
            //base.StopAttacking();
        }

        private IEnumerator WaitToStopAttacking()
        {
            yield return new WaitForSeconds(attackDuration);
            isAttacking = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (isAttacking && collision.gameObject.layer != LayerMask.NameToLayer("Player") && (recentlyHit == null || collision.gameObject != recentlyHit))
            {
                recentlyHit = collision.gameObject;
                Invoke("ResetRecent", attackDuration);
                OnHit(collision.gameObject);
            }
        }

        private void ResetRecent()
        {
            recentlyHit = null;
        }
    }
}