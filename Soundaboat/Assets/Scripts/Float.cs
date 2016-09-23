using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour
{
    public float _waterLevel, _floatHeight;
    public Vector3 _centreOffset;
    public float _bounceDamp;

    void FixedUpdate()
    {
        Vector3 actionPoint = transform.position + transform.TransformDirection(_centreOffset);
        float forceFactor = 1f - ((actionPoint.y - _waterLevel)/_floatHeight);

        //if (forceFactor > 0f)
        //{
            Vector3 uplift = -Physics.gravity*(forceFactor - GetComponent<Rigidbody>().velocity.y*_bounceDamp);
            GetComponent<Rigidbody>().AddForceAtPosition(uplift, actionPoint);
        //}
    }
}
