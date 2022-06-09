using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] protected float _moveSpeed = 10;
    [SerializeField] protected float _elevation = 10;
    [SerializeField] protected float _mouseWheelSpeed = 50;
    protected Camera cam;

    public float MoveSpeed {
        get => _moveSpeed;
        set => Mathf.Max(0, _moveSpeed = value);
    }

    public float Elevation {
        get => _elevation;
        set => _elevation = Mathf.Max(0, Mathf.Min(30, value));
    }

    public float MouseWheelSpeed {
        get => _mouseWheelSpeed;
        set => _mouseWheelSpeed = Mathf.Max(0, value);
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mover la cámara en el plano
        Vector3 move = (Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical")) * Time.deltaTime * MoveSpeed;
        Vector3 pos = transform.position;
        pos += move;
        transform.position = pos;

        // Mover la altura de la cámara
        Elevation += Input.mouseScrollDelta.y * Time.deltaTime * MouseWheelSpeed;
        pos = cam.transform.localPosition;
        pos.y = Elevation;
        cam.transform.localPosition = pos;
    }
}
