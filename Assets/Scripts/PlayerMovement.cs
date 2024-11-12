using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;  // Скорость движения персонажа
    [SerializeField] private float mouseSensitivity = 100.0f;  // Чувствительность мыши для вращения камеры
    private float xRotation = 0f;  // Текущий угол поворота по оси X (для предотвращения переворачивания камеры)
    [SerializeField] private GameObject controlHint, interactionHint, doorObject;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        // Скрываем и блокируем курсор в центре экрана для полного погружения
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        MovePlayer();
        DoorChecker();
    }

    // Управление камерой
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Ограничиваем угол поворота камеры по оси X

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);  // Поворот персонажа по горизонтали
    }

    // Управление движением персонажа
    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");  // Движение по оси X (влево/вправо)
        float moveZ = Input.GetAxis("Vertical");    // Движение по оси Z (вперед/назад)

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
