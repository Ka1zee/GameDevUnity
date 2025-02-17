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
        Debug.Log("Двері відкриваються!");
        _animator.SetBool("isOpen", true);
        yield return base.Open();
    }

    public override IEnumerator Close()
    {
        Debug.Log("Двері закриваються!");
        _animator.SetBool("isOpen", false);
        yield return base.Close();
    }

    // **Функції для Animation Event (Перейменовані, щоб уникнути конфлікту)**
    public void PlayOpenAnimationEvent()
    {
        Debug.Log("Animation Event: Двері відкрилися");
    }

    public void PlayCloseAnimationEvent()
    {
        Debug.Log("Animation Event: Двері закрилися");
    }
}

