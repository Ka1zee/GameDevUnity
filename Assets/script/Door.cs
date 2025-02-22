using System.Collections;
using UnityEngine;

public class Door : OpenableObject
{
    [SerializeField] private float _rotateByDegrees = -90f;
    [SerializeField] private AudioSource _audioSource; // Добавлен AudioSource

    private Vector3 _startRotation;
    private Vector3 _endRotation;
    private bool _isMoving;

    void Start()
    {
        _startRotation = transform.rotation.eulerAngles;
        _endRotation = _startRotation + Vector3.up * _rotateByDegrees;
    }

    public override IEnumerator Close()
    {
        _isMoving = true;

        // Воспроизводим звук закрытия двери
        if (_audioSource != null)
            _audioSource.Play();

        while (_openOrCloseLerp > 0f)
        {
            _openOrCloseLerp -= Time.deltaTime / _opeOrCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_endRotation), _openOrCloseLerp);
            yield return null;
        }
        _isMoving = false;
    }

    public override IEnumerator Open()
    {
        _isMoving = true;

        // Воспроизводим звук открытия двери
        if (_audioSource != null)
            _audioSource.Play();

        while (_openOrCloseLerp < 1f)
        {
            _openOrCloseLerp += Time.deltaTime / _opeOrCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_endRotation), _openOrCloseLerp);
            yield return null;
        }
        _isMoving = false;
    }
}
