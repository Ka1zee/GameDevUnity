using UnityEngine;

public class DestroyWithChance : MonoBehaviour
{
    [Range(0, 100)] public int chanceToDestroy = 100; // Шанс удаления (в %)

    private void Start()
    {
        if (Random.Range(0, 100) < chanceToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
