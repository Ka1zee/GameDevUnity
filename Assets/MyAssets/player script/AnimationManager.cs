using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // ��������� �� ����������
    private Animator anim;
    private WeaponSystem weaponSystem;

    // ��������� ��������� - ��������� ��� ��������� �������
    private readonly string PARAM_IS_MOVING = "IsMoving";
    private readonly string PARAM_HAS_WEAPON = "HasWeapon";
    private readonly string PARAM_ATTACK = "Attack";

    // ����� ��� �����
    private bool isAttacking = false;
    private float attackCooldown = 0f; // ������ ������� ��� �����
    private float attackCooldownTime = 0.5f; // ��� �������� � ��������

    void Start()
    {
        // �������� ��������� ���������
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("�������� �� ��������! ����� ��������� Animator �� ���������.");
            enabled = false;
            return;
        }

        // ������ WeaponSystem �� ��'��� ��� � ���� �������� ��'�����
        weaponSystem = GetComponentInChildren<WeaponSystem>();
        if (weaponSystem == null)
        {
            Debug.LogWarning("WeaponSystem �� ��������! ����� ���� ����������.");
        }
    }

    void Update()
    {
        // ������� �������� �����
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        // ���������� ��������� ����
        bool hasWeapon = weaponSystem != null && weaponSystem.HasWeapon;

        // ����������, �� �������� ��������
        float moveInput = Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")),
                                   Mathf.Abs(Input.GetAxis("Vertical")));
        bool isMoving = moveInput > 0.1f;

        // ������� ����� - ������� ����� ���� ��������� �������
        if (Input.GetMouseButtonDown(0) && hasWeapon && !isAttacking && attackCooldown <= 0)
        {
            StartAttack();
        }

        // ��������� ��������� ��������
        anim.SetBool(PARAM_IS_MOVING, isMoving && !isAttacking);
        anim.SetBool(PARAM_HAS_WEAPON, hasWeapon);
    }

    void StartAttack()
    {
        isAttacking = true;
        attackCooldown = attackCooldownTime;

        // �������: ���� ������� ������ ����� �������������
        anim.ResetTrigger(PARAM_ATTACK);
        // ������������ ������ �����
        anim.SetTrigger(PARAM_ATTACK);

        // ��������� ��� �� ��� �����
        anim.SetBool(PARAM_IS_MOVING, false);

        weaponSystem?.EnableHitboxForWindow();

        // ������������ ������ ��� ������������� �������� ����� �����
        // �� �������, ���� Animation Event �� �������
        Invoke("OnAttackComplete", 1.0f);
    }

    // ����� ����������� �� Animation Event � ���� �������� �����
    // ��� ����������� ����� Invoke
    public void OnAttackComplete()
    {
        // ³������� ������������ ������, ���� ����� ���������� ����� Animation Event
        CancelInvoke("OnAttackComplete");

        // ������� ���� �����
        isAttacking = false;

        weaponSystem?.DisableHitbox();

        // ���� ������� ������ �����
        anim.ResetTrigger(PARAM_ATTACK);
    }
}