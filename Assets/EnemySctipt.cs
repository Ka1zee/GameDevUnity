using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{

    private int HP = 100;
    public Animator animator;
    public Slider healthBar;


    void Update()
    {
        healthBar.value = HP;

    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            animator.SetTrigger("death");
            GetComponent<Collider>().enabled = false;
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
