using UnityEngine;

namespace Henmova
{
    public class Player_Health : Base_Health
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private GameObject _damageFX;

        void Start()
        {
            AddHealth(_maxHealth);
        }

        public override void TakeDamage(int Damage)
        {
            base.TakeDamage(Damage);
            _damageFX.SetActive(true);                                                                                                              
        }
    }
}
