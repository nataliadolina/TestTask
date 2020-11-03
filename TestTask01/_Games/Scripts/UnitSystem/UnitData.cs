﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnitSystem
{
    public class UnitData : MonoBehaviour
    {
        public StatSystem Stats;

        public Weapon StartingWeapon;
        
        [HideInInspector] public Image hpBar;
        [HideInInspector] public Text hpPoint;
        
        public Action OnDamage { get; set; }
        public Action OnRegen { get; set; }
        
        private float m_AttackCoolDown;
        private float m_HitCoolDown;

        public void Init()
        {
            Stats.Init(this);
        }

        private bool regenerationsBlock;
        void Update()
        {
            if (m_AttackCoolDown > 0.0f)
                m_AttackCoolDown -= Time.deltaTime;
            
            if (m_HitCoolDown > 0.0f)
                m_HitCoolDown -= Time.deltaTime;
            else if (!regenerationsBlock)
            {
                regenerationsBlock = true;
                StartCoroutine(AutoRegen());
            }
        }

        public bool CanAttackReach(UnitData target)
        {
            return StartingWeapon.CanHit(this, target);
        }
        
        public bool CanAttackTarget(UnitData target)
        {
            if (!CanAttackReach(target))
                return false;

            if (m_AttackCoolDown > 0.0f)
                return false;

            return true;
        }

        public bool TargetIsLive(UnitData target)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;

            return true;
        }
   
        public void Attack(UnitData target)
        {
            StartingWeapon.Attack(this, target);
        }
        
        public void AttackTriggered()
        {
            m_HitCoolDown = 5f;
            regenerationsBlock = false;
            m_AttackCoolDown = StartingWeapon.Stats.Speed - (Stats.stats.agility * 0.5f * 0.001f * StartingWeapon.Stats.Speed);
        }

        public void Damage(Weapon.AttackData attackData)
        {
            m_HitCoolDown = 5f;
            regenerationsBlock = false;
            Stats.Damage(attackData);
            OnDamage?.Invoke();
        }

        private IEnumerator AutoRegen()
        {
            while (Stats.CurrentHealth < Stats.stats.health && regenerationsBlock)
            {
                Stats.CurrentHealth++;
                OnRegen?.Invoke();
                yield return new WaitForSeconds(1);
            }
        }
    }
}

