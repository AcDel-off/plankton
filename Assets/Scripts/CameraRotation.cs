using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100.0f; // ���������������� ����
    [SerializeField] private float xRotationLimitUp = 90.0f;    // ����������� �������� �� ��� X (����)
    [SerializeField] private float xRotationLimitDown = 90.0f;    // ����������� �������� �� ��� X (�����)
    [SerializeField] private float yRotationLimit = 130.0f;    // ����������� �������� �� ��� Y (�����/������)

    private float xRotation = 0f;  // ������� ���� �������� �� ��� X
    private float yRotation = 0f;  // ������� ���� �������� �� ��� Y

    void Update()
    {
        RotateCamera();
    }

    // ����� ��� �������� ������ � �������������
    private void RotateCamera()
    {
        // �������� �������� ����������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // �������� ������� ���� �� ��� X, ������������ �������� �� ��� X
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, xRotationLimitUp, xRotationLimitDown);

        // �������� ������� ���� �� ��� Y, ������������ �������� �� ��� Y
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -yRotationLimit, yRotationLimit);

        // ��������� ��������
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
