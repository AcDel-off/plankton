using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;  // �������� �������� ���������
    [SerializeField] private float mouseSensitivity = 100.0f;  // ���������������� ���� ��� �������� ������
    private float xRotation = 0f;  // ������� ���� �������� �� ��� X (��� �������������� ��������������� ������)
    [SerializeField] private GameObject controlHint, interactionHint, doorObject;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        // �������� � ��������� ������ � ������ ������ ��� ������� ����������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        MovePlayer();
        DoorChecker();
    }

    // ���������� �������
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // ������������ ���� �������� ������ �� ��� X

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);  // ������� ��������� �� �����������
    }

    // ���������� ��������� ���������
    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");  // �������� �� ��� X (�����/������)
        float moveZ = Input.GetAxis("Vertical");    // �������� �� ��� Z (������/�����)

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    private void DoorChecker()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 3f);
        if (hit.transform != null)
        {
            if (hit.transform.CompareTag("Finish"))
            {
                interactionHint.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    doorObject.GetComponent<Door>().StartCutScene();
                }
            }
        }
        else 
        { 
            interactionHint.SetActive(false); 
        }
    }
}
