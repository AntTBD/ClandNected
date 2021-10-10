using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Camera camera;

    [SerializeField]
    private GridManager gridManager;

    [SerializeField]
    private float size = 10;

    private const int MINSIZE = 4;
    private const int MAXSIZE = 13;

    private void Start()
    {
        this.camera = this.gameObject.GetComponent<Camera>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        size -= Input.GetAxis("Mouse ScrollWheel");
        if (size < MINSIZE)
        {
            size = MINSIZE;
        }
        if (size > MAXSIZE)
        {
            size = MAXSIZE;
            transform.position = new Vector3(0, 0, -10);
        }

        this.camera.orthographicSize = size;

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        transform.Translate(move, Space.World);
        int width = gridManager.GetGrid().GetWidth();
        int height = gridManager.GetGrid().GetHeight();
        Vector3 originPosition = gridManager.GetGrid().GetOriginPosition();
        float x = Mathf.Clamp(transform.position.x, originPosition.x, originPosition.x + width);
        float y = Mathf.Clamp(transform.position.y, originPosition.y, originPosition.y + height);
        transform.position = new Vector3(x, y, -10);

    }
}