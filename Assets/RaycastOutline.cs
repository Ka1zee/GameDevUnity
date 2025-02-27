using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastOutline : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    private float _maxRayDistance = 4f;
    private Outline lastOutlineObject;

    void Update()
    {
        Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _maxRayDistance, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _maxRayDistance))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            if (hit.transform.gameObject.CompareTag("Item") || hit.transform.gameObject.CompareTag("Weapon"))
            {
                Outline outline = hit.transform.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    Debug.Log("Outline component found on: " + hit.transform.name);

                    if (lastOutlineObject != null && lastOutlineObject != outline)
                    {
                        lastOutlineObject.enabled = false;
                    }

                    lastOutlineObject = outline;
                    lastOutlineObject.enabled = true;
                }
                else
                {
                    Debug.LogWarning("Outline component not found on: " + hit.transform.name);
                }
            }
            else
            {
                if (lastOutlineObject != null)
                {
                    lastOutlineObject.enabled = false;
                    lastOutlineObject = null;
                }
            }
        }
        else
        {
            if (lastOutlineObject != null)
            {
                lastOutlineObject.enabled = false;
                lastOutlineObject = null;
            }
        }
    }
}

