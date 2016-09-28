using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public float _speed = 50.0f;

    public float _dampTime = 0.15f;
    private Vector3 _velocity;
    public Transform _moveTarget, _LookTarget;
    private float _delta;

    void Start()
    {
        _delta = Vector3.Distance(transform.position, _moveTarget.position);
        _velocity = Vector3.zero;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _LookTarget.Rotate(new Vector3(0, mouseX, 0) * Time.deltaTime * _speed);

        if (_moveTarget)
        {
            Vector3 direction = Vector3.Normalize(_moveTarget.position - _LookTarget.position);
            Vector3 destination = direction * _delta + _moveTarget.position;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _dampTime);

            transform.LookAt(_LookTarget.position);
        }
    }
}
