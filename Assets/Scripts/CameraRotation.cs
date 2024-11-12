using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100.0f; // Чувствительность мыши
    [SerializeField] private float xRotationLimitUp = 90.0f;    // Ограничение вращения по оси X (вниз)
    [SerializeField] private float xRotationLimitDown = 90.0f;    // Ограничение вращения по оси X (вверх)
    [SerializeField] private float yRotationLimit = 130.0f;    // Ограничение вращения по оси Y (влево/вправо)

    private float xRotation = 0f;  // Текущий угол вращения по оси X
    private float yRotation = 0f;  // Текущий угол вращения по оси Y

    void Update()
    {
        RotateCamera();
    }

    // Метод для вращения камеры с ограничениями
    private void RotateCamera()
    {
        // Получаем значения перемещения мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Изменяем текущий угол по оси X, ограничиваем вращение по оси X
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, xRotationLimitUp, xRotationLimitDown);

        // Изменяем текущий угол по оси Y, ограничиваем вращение по оси Y
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -yRotationLimit, yRotationLimit);

        // Применяем вращение
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
