using UnityEngine;

public class DragCameraX : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float dragSpeed = 0.01f;

    [Header("World X Limits")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    private Vector3 lastPosition;
    private bool isDragging = false;

    void Update()
    {
        HandleMouse();
        HandleTouch();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            MoveCamera(delta.x);
            lastPosition = Input.mousePosition;
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            lastPosition = touch.position;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 delta = touch.position - (Vector2)lastPosition;
            MoveCamera(delta.x);
            lastPosition = touch.position;
        }
    }

    void MoveCamera(float deltaX)
    {
        float newX = transform.position.x - deltaX * dragSpeed;

        // Clamp 
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector3(
            newX,
            transform.position.y,
            transform.position.z
        );
    }
}
