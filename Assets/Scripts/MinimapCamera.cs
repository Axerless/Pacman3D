using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Transform cameraTransfrom;
    void Update()
    {
        Vector3 newPosition = playerTransform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, cameraTransfrom.eulerAngles.y, 0f);

    }
}
