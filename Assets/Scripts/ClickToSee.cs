using UnityEngine;

public class ClickToInteract : MonoBehaviour
{
    public Camera playerCamera; // Kamera referansý

    void Update()
    {
        // Eðer sol fare tuþuna basýldýysa
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast ile týklanan objeyi kontrol et
            if (Physics.Raycast(ray, out hit))
            {
                // Týklanan objenin tag'ini kontrol et veya iþlem yap
                Debug.Log("Týklanan obje: " + hit.transform.name);

                // Örneðin, bir obje týklandýðýnda bir fonksiyon çaðýrabiliriz
                if (hit.transform.CompareTag("Interactable"))
                {
                    InteractWithObject(hit.transform);
                }
            }
        }
    }

    void InteractWithObject(Transform obj)
    {
        // Obje ile etkileþime geç
        Debug.Log(obj.name + " ile etkileþimde bulunuldu.");
        // Burada objeye göre bir iþlem yapabilirsiniz
    }
}
