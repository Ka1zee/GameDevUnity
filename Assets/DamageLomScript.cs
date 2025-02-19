using UnityEngine;

public class DamageLomScript : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().TakeDamage(damageAmount);
        }
    }
}
