using UnityEngine;

public class JoystickCommandReader : MonoBehaviour
{
    [Header("Joystick References")]
    [SerializeField] private VirtualJoystick leftJoystick;
    [SerializeField] private VirtualJoystick rightJoystick;

    [Header("Current Output")]
    [SerializeField] private float linearX;
    [SerializeField] private float linearY;
    [SerializeField] private float angularZ;

    private Vector3 lastLoggedValues = Vector3.zero;

    private void Update()
    {
        if (leftJoystick == null || rightJoystick == null)
            return;

        linearX = leftJoystick.OutputY;
        angularZ = leftJoystick.OutputX;
        linearY = rightJoystick.OutputX;

        Vector3 current = new Vector3(linearX, linearY, angularZ);

        if (Vector3.Distance(current, lastLoggedValues) > 0.01f)
        {
            Debug.Log($"[JOYSTICK] linear_x={linearX:F3}, linear_y={linearY:F3}, angular_z={angularZ:F3}");
            lastLoggedValues = current;
        }
    }

    public float GetLinearX() => linearX;
    public float GetLinearY() => linearY;
    public float GetAngularZ() => angularZ;
}
