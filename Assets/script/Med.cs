using UnityEngine;

public class Med : MonoBehaviour
{
    public GameObject Model;
    public int healAmount = 50; // Количество восстанавливаемого здоровья

    public void Heal()
    {
        // Проверяем, меньше ли текущее здоровье 100
        if (NewMonoBehaviourScript.playerhealth < 100)
        {
            NewMonoBehaviourScript.playerhealth += healAmount;

            // Ограничиваем максимум 100 хп
            if (NewMonoBehaviourScript.playerhealth > 100)
            {
                NewMonoBehaviourScript.playerhealth = 100;
            }
            Destroy(Model); // Удаляем аптечку
        }
    }
}
