using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] float swayAmount;
    [SerializeField] float smooth;

    float verticalMouse;
    float horizontalMouse;

    private void Update()
    {
        verticalMouse = Input.GetAxisRaw("Mouse Y") * swayAmount;
        horizontalMouse = Input.GetAxisRaw("Mouse X") * swayAmount;

        Quaternion swayX = Quaternion.AngleAxis(-verticalMouse, Vector3.right);
        Quaternion swayY = Quaternion.AngleAxis(horizontalMouse, Vector3.up);

        Quaternion targetSway = swayX * swayY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetSway, smooth * Time.deltaTime);
    }
}
