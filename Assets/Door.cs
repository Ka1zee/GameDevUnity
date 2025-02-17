using UnityEngine;
using System.Collections;

public class DoorController : OpenableObject
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override IEnumerator Open()
    {
        Debug.Log("���� ������������!");
        _animator.SetBool("isOpen", true);
        yield return base.Open();
    }

    public override IEnumerator Close()
    {
        Debug.Log("���� ������������!");
        _animator.SetBool("isOpen", false);
        yield return base.Close();
    }

    // **������� ��� Animation Event (������������, ��� �������� ��������)**
    public void PlayOpenAnimationEvent()
    {
        Debug.Log("Animation Event: ���� ���������");
    }

    public void PlayCloseAnimationEvent()
    {
        Debug.Log("Animation Event: ���� ���������");
    }
}

