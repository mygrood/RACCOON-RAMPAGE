using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    
    //размеры камеры
    private Camera cam;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    //границы карты
    public SpriteRenderer mapRenderer;
    private Vector2 minBounds; 
    private Vector2 maxBounds;     
    

    void Start()
    {
        Bounds bounds = mapRenderer.bounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;

        cam = Camera.main;
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cam.aspect * cameraHalfHeight;
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.position;
        
        //ограничение камеры границами карты
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

        newPosition.z = transform.position.z; 
        transform.position = newPosition;
    }
}
