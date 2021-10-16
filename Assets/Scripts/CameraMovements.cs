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

    [Header("Smartphone Settings")]
    // variables for camera pan
    public float speedPan = 0.01f;

    // variables for camera zoom in and out
    public float perspectiveZoomSpeed = 0.01f;
    public float orthoZoomSpeed = 0.01f;


    private void Start()
    {
        camera = this.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (IfAndroidIsUsed() == false)
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

    // https://forum.unity.com/threads/mobile-touch-to-orbit-pan-and-zoom-camera-without-fix-target-in-one-script.522607/
    private bool IfAndroidIsUsed()
    {
        bool androidUsed = false;
        // This part is for camera pan only & for 2 fingers stationary gesture
        if (Input.touchCount > 0 && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * speedPan, -touchDeltaPosition.y * speedPan, 0);
            androidUsed = true;
        }

        //this part is for zoom in and out 
        if (Input.touchCount == 2)
        {

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;


            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

            if (camera.orthographic)
            {
                //camera.orthographicSize += deltaMagDiff * orthoZoomSpeed;
                //camera.orthographicSize = Mathf.Max(camera.orthographicSize, .1f);
                zoom(camera.orthographicSize + (deltaMagDiff * orthoZoomSpeed));
            }
            else
            {
                camera.fieldOfView += deltaMagDiff * perspectiveZoomSpeed;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, .1f, 179.9f);
            }
            androidUsed = true;
        }
        return androidUsed;
    }
}
