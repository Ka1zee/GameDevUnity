using UnityEngine;

public class DamageLomScript : MonoBehaviour
{
    public int damageAmount = 20;
    private WeaponDamage weaponDamage;

    private void Awake()
    {
        weaponDamage = GetComponent<WeaponDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (weaponDamage != null)
            {
                weaponDamage.TryDealDamage(enemy, other.ClosestPoint(transform.position));
            }
            else if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
    }
}
