using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public int damageCount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NewMonoBehaviourScript.Damage(damageCount);
        }
    }
}
