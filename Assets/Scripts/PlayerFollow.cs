using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform PlayerTransform;

    public Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    public float CameraPitch = 2.0f;

    void Start()
    {
        _cameraOffset = new Vector3(0, CameraPitch, -5f);
    }

    // LateUpdate is called after Update methods
    void LateUpdate()
    {
        if (PlayerTransform == null)
            return;

        // Oyuncunun rotasyonuna göre kamerayı hareket ettir
        Quaternion playerRotation = PlayerTransform.rotation;
        Vector3 rotatedOffset = playerRotation * _cameraOffset;

        Vector3 targetPosition = PlayerTransform.position + rotatedOffset;

        // Kameray� p�r�zs�z bir �ekilde yeni pozisyona ta��
        transform.position = Vector3.Slerp(transform.position, targetPosition, SmoothFactor);

        // Kameray� oyuncuya do�ru d�nd�r
        transform.LookAt(PlayerTransform);
    }

    public void SetCameraTarget(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }
}
