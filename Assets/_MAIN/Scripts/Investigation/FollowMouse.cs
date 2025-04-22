using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera currentCamera;
    
    void Start() {
        currentCamera = GameManager.instance.invCamera;
    }

    void Update() {
        FollowMousePosition();
    }

    public void FollowMousePosition() {
        transform.position = GetWorldPositionFromMouse();
    }

    public Vector2 GetWorldPositionFromMouse() {
        return currentCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
