using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitCamera : MonoBehaviour {
    public Transform target;                 // Nhân vật
    public float rotateSpeed = 0.2f;
    public Vector3 cameraOffset = new Vector3(0, 5, -7);
    public Transform cameraTransform;        // Camera gắn vào đây
    public FixedJoystick joystick;           // Joystick để kiểm tra đang di chuyển

    private float camPitch = 20f;            // Góc nhìn dọc (pitch)
    public float minPitch = 10f;             // Giới hạn dưới
    public float maxPitch = 80f;             // Giới hạn trên

    void Start() {
        // Đặt vị trí camera ban đầu và nhìn vào nhân vật
        cameraTransform.localPosition = cameraOffset;
        cameraTransform.localRotation = Quaternion.Euler(camPitch, 0, 0);
    }

    void LateUpdate() {
        if (target == null) return;

        // CameraRig luôn theo vị trí nhân vật
        transform.position = target.position;

        // Kiểm tra xem có đang vuốt để xoay camera không
        if (Input.touchCount == 1 && !IsPointerOverUI() && !IsUsingJoystick()) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved) {
                float rotX = touch.deltaPosition.x * rotateSpeed;
                float rotY = -touch.deltaPosition.y * rotateSpeed;

                // Xoay ngang quanh nhân vật
                transform.Rotate(0, rotX, 0, Space.World);

                // Xoay dọc
                camPitch += rotY;
                camPitch = Mathf.Clamp(camPitch, minPitch, maxPitch);
                cameraTransform.localRotation = Quaternion.Euler(camPitch, 0, 0);
            }
        }
    }

    // Kiểm tra xem đang chạm vào UI (ví dụ joystick, nút...)
    bool IsPointerOverUI() {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            return false;
#else
    return EventSystem.current.IsPointerOverGameObject();
#endif
    }

    // Kiểm tra xem joystick đang được sử dụng để di chuyển
    private bool IsUsingJoystick() {
        return joystick != null &&
               (Mathf.Abs(joystick.Horizontal) > 0.01f || Mathf.Abs(joystick.Vertical) > 0.01f);
    }
}
