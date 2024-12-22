using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    public Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public float CameraPitch = 2.0f; // Kameranýn oyuncunun üstünde duracaðý mesafe

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = new Vector3(0, CameraPitch, -5f); // Kamerayý biraz yukarý ve arkaya konumlandýr
    }

    // LateUpdate is called after Update methods
    void LateUpdate()
    {
        if (PlayerTransform == null)
            return;

        // Oyuncunun rotasyonuna göre kamerayý hareket ettir
        Quaternion playerRotation = PlayerTransform.rotation;
        Vector3 rotatedOffset = playerRotation * _cameraOffset;

        Vector3 targetPosition = PlayerTransform.position + rotatedOffset;

        // Kamerayý pürüzsüz bir þekilde yeni pozisyona taþý
        transform.position = Vector3.Slerp(transform.position, targetPosition, SmoothFactor);

        // Kamerayý oyuncuya doðru döndür
        transform.LookAt(PlayerTransform);
    }

    public void SetCameraTarget(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }
}
