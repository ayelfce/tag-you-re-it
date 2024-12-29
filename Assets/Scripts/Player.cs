using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource footStep;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    public PhotonView view;
    [SerializeField] private TextMeshProUGUI usernameText;
    private Vector3 velocity;
    private float turnSmoothVelocity;
    public bool sobelenemez = false;

    private bool isWalking;
    public bool isSeen;
    public bool isInSeek = false;

    private Transform cameraTransform; // Kamera referansı

    private void Start()
    {

        if (view.IsMine)
        {
            usernameText.text = PhotonNetwork.NickName;
        }
        else
        {
            usernameText.text = view.Owner.NickName; // Diğer oyuncuların kullanıcı adını göster
        }

        // Kamera referansını bul
        cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("MainCamera not found!");
        }

    }

    private void Update()
    {
        if (!view.IsMine || cameraTransform == null)
            return;

        // Hareket girdilerini al
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // S tuşunu devre dışı bırak
        if (vertical < 0)
            vertical = 0;

        // Hareket yönü
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Eğer hareket varsa isWalking'i güncelle
        isWalking = direction.magnitude >= 0.001f;

        if (isWalking)
        {
            footStep.enabled = true;
            // Kameraya göre hedef açıyı hesapla
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Karakteri döndür
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Hareket yönü
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else 
        {
            footStep.enabled = false;
        }

        // Yerçekimi
        velocity.y += gravity * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public string GetPlayerName()
    {
        return usernameText.text;
    }

    public void enteredSeek()
    {
        isInSeek = true;
    }

    public void exitedSeek()
    {
        isInSeek = false;
    }
}
