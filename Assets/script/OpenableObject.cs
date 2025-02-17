using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class OpenableObject : MonoBehaviour
{
    [SerializeField] protected float _opeOrCloseTime = 1f;
    protected float _openOrCloseLerp;
    protected bool _isOpen = true;

    public virtual IEnumerator Close()
    {
        while (_openOrCloseLerp < 0f)
        {
            yield return null;
        }
    }
    public virtual IEnumerator Open()
    {
        while (_openOrCloseLerp > 0f)
        {
            yield return null;
        }
    }
    public void OpenOrClose()
    {
        _isOpen = !_isOpen;

        StopAllCoroutines();

        if (_isOpen)
        {
            StartCoroutine(Close());
        }
        else
        {
            StartCoroutine(Open());
        }
    }
}
