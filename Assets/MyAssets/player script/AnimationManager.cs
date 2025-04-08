using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Посилання на компоненти
    private Animator anim;
    private WeaponSystem weaponSystem;

    // Параметри аніматора - константи для уникнення помилок
    private readonly string PARAM_IS_MOVING = "IsMoving";
    private readonly string PARAM_HAS_WEAPON = "HasWeapon";
    private readonly string PARAM_ATTACK = "Attack";

    // Змінні для стану
    private bool isAttacking = false;
    private float attackCooldown = 0f; // Додаємо кулдаун для атаки
    private float attackCooldownTime = 0.5f; // Час кулдауну в секундах

    void Start()
    {
        // Отримуємо компонент аніматора
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Аніматор не знайдено! Додай компонент Animator до персонажа.");
            enabled = false;
            return;
        }

        // Шукаємо WeaponSystem на об'єкті або в його дочірніх об'єктах
        weaponSystem = GetComponentInChildren<WeaponSystem>();
        if (weaponSystem == null)
        {
            Debug.LogWarning("WeaponSystem не знайдено! Зброя буде недоступна.");
        }
    }

    void Update()
    {
        // Обробка кулдауну атаки
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Перевіряємо наявність зброї
        bool hasWeapon = weaponSystem != null && weaponSystem.HasWeapon;

        // Обчислюємо, чи рухається персонаж
        float moveInput = Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")),
                                   Mathf.Abs(Input.GetAxis("Vertical")));
        bool isMoving = moveInput > 0.1f;

        // Обробка атаки - можлива тільки коли закінчився кулдаун
        if (Input.GetMouseButtonDown(0) && hasWeapon && !isAttacking && attackCooldown <= 0)
        {
            StartAttack();
        }

        // Оновлюємо параметри анімації
        anim.SetBool(PARAM_IS_MOVING, isMoving && !isAttacking);
        anim.SetBool(PARAM_HAS_WEAPON, hasWeapon);
    }

    void StartAttack()
    {
        isAttacking = true;
        attackCooldown = attackCooldownTime;

        // Важливо: явно скидаємо тригер перед встановленням
        anim.ResetTrigger(PARAM_ATTACK);
        // Встановлюємо тригер атаки
        anim.SetTrigger(PARAM_ATTACK);

        // Зупиняємо рух під час атаки
        anim.SetBool(PARAM_IS_MOVING, false);

        // Встановлюємо таймер для автоматичного скидання стану атаки
        // на випадок, якщо Animation Event не спрацює
        Invoke("OnAttackComplete", 1.0f);
    }

    // Метод викликається як Animation Event в кінці анімації атаки
    // або автоматично через Invoke
    public void OnAttackComplete()
    {
        // Відміняємо автоматичний виклик, якщо метод викликаний через Animation Event
        CancelInvoke("OnAttackComplete");

        // Скидаємо стан атаки
        isAttacking = false;

        // Явно скидаємо тригер атаки
        anim.ResetTrigger(PARAM_ATTACK);
    }
}