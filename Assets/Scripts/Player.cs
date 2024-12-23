// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using Photon.Pun;
// using TMPro;

// public class Player : MonoBehaviour
// {
//     [SerializeField] private CharacterController controller;
//     [SerializeField] private float moveSpeed = 6f;
//     [SerializeField] private float turnSmoothTime = 0.1f;
//     [SerializeField] private float gravity = -9.81f;
//     public PhotonView view;
//     [SerializeField] private TextMeshPro usernameText;
//     private Vector3 velocity;
//     private float turnSmoothVelocity;

//     private bool isWalking;

//     private void Start()
//     {
//         if (view.IsMine)
//         {
//             usernameText.text = PhotonNetwork.NickName; // Kullanıcı adı ayarla
//         }
//         else
//         {
//             usernameText.text = view.Owner.NickName; // Diğer oyuncuların kullanıcı adını göster
//         }
//     }

//     private void Update()
//     {
//         float horizontal = Input.GetAxisRaw("Horizontal");
//         float vertical = Input.GetAxisRaw("Vertical");
//         Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//         isWalking = direction != Vector3.zero;

//         if (direction.magnitude >= 0.1f && view.IsMine)
//         {
//             float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
//             transform.rotation = Quaternion.Euler(0f, angle, 0f);

//             Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
//             controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
//         }

//         velocity.y += gravity * Time.deltaTime;

//         if (controller.isGrounded && velocity.y < 0)
//         {
//             velocity.y = -2f;
//         }

//         controller.Move(velocity * Time.deltaTime);
//     }

//     public bool IsWalking()
//     {
//         return isWalking;
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    public PhotonView view;
    [SerializeField] private TextMeshPro usernameText;
    private Vector3 velocity;
    private float turnSmoothVelocity;

    private bool isWalking;

    private Transform cameraTransform; // Kamera referansı

    private void Start()
    {
        if (view.IsMine)
        {
            usernameText.text = PhotonNetwork.NickName; // Kullanıcı adı ayarla
        }
        else
        {
            usernameText.text = view.Owner.NickName; // Diğer oyuncuların kullanıcı adını göster
        }

        // Kamera referansını bul
        cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("MainCamera bulunamadı! Sahneye bir kamera eklediğinizden ve tag'ini 'MainCamera' olarak ayarladığınızdan emin olun.");
        }
    }

    private void Update()
    {
        if (!view.IsMine || cameraTransform == null)
            return;

        // Hareket girdilerini al
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // S tuşunu devre dışı bırak, ancak geri hareketi engellemiyoruz
        if (vertical < 0)
            vertical = 0;

        // Hareket yönü
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Eğer hareket varsa isWalking'i güncelle
        isWalking = direction.magnitude >= 0.1f;

        if (isWalking)
        {
            // Kameraya göre hedef açıyı hesapla
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Karakteri döndür
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Hareket yönü
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        // Yerçekimi ekle
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
}
