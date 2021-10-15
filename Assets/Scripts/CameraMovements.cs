using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovements : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private new Camera camera;

    [SerializeField]
    private GridManager gridManager;

    [SerializeField]
    private float size = 10;

    private const int MINSIZE = 4;
    private const int MAXSIZE = 13;

    private bool mDragging;

    private void Start()
    {
        camera = this.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        // zoom
        size -= Input.GetAxis("Mouse ScrollWheel");
        zoom(size);

        // dragging
        // https://faramira.com/implement-camera-pan-and-zoom-controls-in-unity2d/
        // Save the position in worldspace.
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
            mDragging = true;
        }

        if (Input.GetMouseButton(2) && mDragging)
        {
            Vector3 diff = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            diff.z = 0.0f;
            camera.transform.position += diff;
        }
        if (Input.GetMouseButtonUp(2))
        {
            mDragging = false;
        }
    }

    private void zoom(float size)
    {
        if (size < MINSIZE)
        {
            size = MINSIZE;
        }
        if (size > MAXSIZE)
        {
            size = MAXSIZE;
            transform.position = new Vector3(0, 0, -10);
        }
        
        this.size = size;//save size

        this.camera.orthographicSize = this.size;
    }

    public void ResetCam()
    {
        zoom(999);
    }
}
