using Sirenix.OdinInspector;
using UnityEngine;

namespace Henmova
{
    public class Base_Health : MonoBehaviour, IDamgable
    {
        [Title("----- Health Settings -----")]

        public int CurrentHealth { get; private set; }
        public bool IsDead { get; private set; }

        public void AddHealth(int Amount)
        {
            if (IsDead) return;

            CurrentHealth += Amount;

            // Assuming there's a maximum health limit, you can set it here
            // For example, if max health is 100:
            // CurrentHealth = Mathf.Min(CurrentHealth, 100);
        }

        public virtual void TakeDamage(int Damage)
        {
            if (IsDead) return;

            CurrentHealth -= Damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsDead = true;
                Die();
            }
        }

        public virtual void Die()
        {
            // Dead
        }
    }
}
