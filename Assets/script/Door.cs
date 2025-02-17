using System.Collections;
using UnityEngine;

public class Door : OpenableObject
{
    [SerializeField] private float _rotateByDegress = -90f;
    private Vector3 _startRotation;
    private Vector3 _endRotarion;

    private bool _isMoving;
    void Start()
    {
        _startRotation = transform.rotation.eulerAngles;
        _endRotarion = _startRotation + Vector3.up * _rotateByDegress;
    }

    public override IEnumerator Close()
    {
        _isMoving = true;

        while (_openOrCloseLerp > 0f)
        {
            _openOrCloseLerp -= Time.deltaTime / _opeOrCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_endRotarion), _openOrCloseLerp);
            yield return null;
        }
        _isMoving = false;
    }
    public override IEnumerator Open()
    {
        _isMoving = true;

        while (_openOrCloseLerp < 1f)
        {
            _openOrCloseLerp += Time.deltaTime / _opeOrCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_endRotarion), _openOrCloseLerp);
            yield return null;
        }
        _isMoving = false;
    }
}
