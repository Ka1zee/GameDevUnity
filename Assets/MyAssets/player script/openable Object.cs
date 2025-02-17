using UnityEngine;
using System.Collections;

public class OpenableObject : MonoBehaviour
{
    [SerializeField] protected float _openOrCloseTime = 1.0f;
    protected float _openOrCloseLerp;
    protected bool _isOpen = true;

    // Можна перевизначити цей метод у дочірніх класах
    public virtual IEnumerator Close()
    {
        while (_openOrCloseLerp > 0f)
        {
            _openOrCloseLerp -= Time.deltaTime / _openOrCloseTime;
            yield return null;
        }
    }

    // Можна перевизначити цей метод у дочірніх класах
    public virtual IEnumerator Open()
    {
        while (_openOrCloseLerp < 1f)
        {
            _openOrCloseLerp += Time.deltaTime / _openOrCloseTime;
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
