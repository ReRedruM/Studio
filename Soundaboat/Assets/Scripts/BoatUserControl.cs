using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (BoatController))]
    public class BoatUserControl : MonoBehaviour
    {
        private BoatController _boatController; // the car controller we want to use
        public bool _moving;


        private void Awake()
        {
            // get the car controller
            _boatController = GetComponent<BoatController>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (h != 0.0f || v != 0.0f)
                _moving = true;
            else
                _moving = false;
            _boatController.Move(h, v, v);
        }
}
