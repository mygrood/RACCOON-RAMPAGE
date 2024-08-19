using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform target;

    //������� ������
    private Camera cam;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    //������� �����
    public Renderer mapRenderer;
    public Tilemap tilemap;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    public TilemapCollider2D tilemapCollider;

    void Start()
    {
        Bounds bounds = tilemap.localBounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;

        //Bounds bounds = mapRenderer.bounds;
        /*Bounds bounds = tilemap.localBounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
        Debug.Log(bounds);
        */
        //CalculateTilemapBounds();
        cam = Camera.main;
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cam.aspect * cameraHalfHeight;
    }

    void LateUpdate()
    {
        Vector3 newPosition = target.position;

        //����������� ������ ��������� �����
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }


    void CalculateTilemapBounds()
    {
        // �������� ������� ����� Tilemap
        BoundsInt cellBounds = tilemap.cellBounds;

        // �������������� ����������� � ������������ ������� ����� � ������� ����������
        Vector3 minCellWorldPosition = tilemap.CellToWorld(cellBounds.min);
        Vector3 maxCellWorldPosition = tilemap.CellToWorld(cellBounds.max);

        // ������������� maxBounds �� ���� ������ ������
        Vector3 tileSize = tilemap.cellSize;
        maxCellWorldPosition += new Vector3(tileSize.x, tileSize.y, 0);

        // ��������� ����������� � ������������ ������
        minBounds = new Vector2(minCellWorldPosition.x, minCellWorldPosition.y);
        maxBounds = new Vector2(maxCellWorldPosition.x, maxCellWorldPosition.y);

        Debug.Log("Min Bounds: " + minBounds);
        Debug.Log("Max Bounds: " + maxBounds);
    }

    void OnDrawGizmos()
    {
        if (tilemap != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
            Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(minBounds.x, minBounds.y, 0));
        }
    }
}
