using System;
using UnityEngine;

internal enum BoatDriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }

    internal enum SpeedType
    {
        MPH,
        KPH
    }

    public class BoatController : MonoBehaviour
    {
        [SerializeField] private BoatDriveType _boatDriveType = BoatDriveType.FourWheelDrive;
        [SerializeField] private WheelCollider[] _wheelColliders = new WheelCollider[4];
        //[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];
        [SerializeField] private Vector3 _centreOfMassOffset;
        [SerializeField] private float _maximumSteerAngle = 25f;
        [Range(0, 1)] [SerializeField] private float _steerHelper = 0.644f; // 0 is raw physics , 1 the car will grip in the direction it is facing
        [Range(0, 1)] [SerializeField] private float _tractionControl = 1.0f; // 0 is no traction control, 1 is full interference
        [SerializeField] private float _fullTorqueOverAllWheels = 2500f;
        [SerializeField] private float _reverseTorque = 500f;
        //[SerializeField] private float _maxHandbrakeTorque = 0.00088f;
        [SerializeField] private float _downforce = 100f;
        [SerializeField] private SpeedType _speedType;
        [SerializeField] private float _topspeed = 150f;
        [SerializeField] private static int _noOfGears = 5;
        [SerializeField] private float _revRangeBoundary = 1f;
        [SerializeField] private float _slipLimit = 0.3f;
        [SerializeField] private float _brakeTorque = 20000f;

        private Quaternion[] _wheelMeshLocalRotations;
        private Vector3 _prevpos, _pos;
        private float _steerAngle;
        private int _gearNum;
        private float _gearFactor;
        private float _oldRotation;
        private float _currentTorque;
        private Rigidbody _rigidbody;
        private const float _reversingThreshold = 0.01f;

        public bool Skidding { get; private set; }
        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle{ get { return _steerAngle; }}
        public float CurrentSpeed{ get { return _rigidbody.velocity.magnitude*2.23693629f; }}
        public float MaxSpeed{get { return _topspeed; }}
        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        // Use this for initialization
        private void Start()
        {
            _wheelMeshLocalRotations = new Quaternion[4];
            _wheelColliders[0].attachedRigidbody.centerOfMass = _centreOfMassOffset;

            _rigidbody = GetComponent<Rigidbody>();
            _currentTorque = _fullTorqueOverAllWheels - (_tractionControl*_fullTorqueOverAllWheels);
        }


        private void GearChanging()
        {
            float f = Mathf.Abs(CurrentSpeed/MaxSpeed);
            float upgearlimit = (1/(float) _noOfGears)*(_gearNum + 1);
            float downgearlimit = (1/(float) _noOfGears)*_gearNum;

            if (_gearNum > 0 && f < downgearlimit)
            {
                _gearNum--;
            }

            if (f > upgearlimit && (_gearNum < (_noOfGears - 1)))
            {
                _gearNum++;
            }
        }


        // simple function to add a curved bias towards 1 for a value in the 0-1 range
        private static float CurveFactor(float factor)
        {
            return 1 - (1 - factor)*(1 - factor);
        }


        // unclamped version of Lerp, to allow value to exceed the from-to range
        private static float ULerp(float from, float to, float value)
        {
            return (1.0f - value)*from + value*to;
        }


        private void CalculateGearFactor()
        {
            float f = (1/(float) _noOfGears);
            // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
            // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
            var targetGearFactor = Mathf.InverseLerp(f*_gearNum, f*(_gearNum + 1), Mathf.Abs(CurrentSpeed/MaxSpeed));
            _gearFactor = Mathf.Lerp(_gearFactor, targetGearFactor, Time.deltaTime*5f);
        }


        private void CalculateRevs()
        {
            // calculate engine revs (for display / sound)
            // (this is done in retrospect - revs are not used in force/power calculations)
            CalculateGearFactor();
            var gearNumFactor = _gearNum/(float) _noOfGears;
            var revsRangeMin = ULerp(0f, _revRangeBoundary, CurveFactor(gearNumFactor));
            var revsRangeMax = ULerp(_revRangeBoundary, 1f, gearNumFactor);
            Revs = ULerp(revsRangeMin, revsRangeMax, _gearFactor);
        }


        public void Move(float steering, float accel, float footbrake)
        {
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                _wheelColliders[i].GetWorldPose(out position, out quat);
            }

            //clamp input values
            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1*Mathf.Clamp(footbrake, -1, 0);

            //Set the steer on the front wheels.
            //Assuming that wheels 0 and 1 are the front wheels.
            _steerAngle = steering*_maximumSteerAngle;
            _wheelColliders[0].steerAngle = _steerAngle;
            _wheelColliders[1].steerAngle = _steerAngle;

            SteerHelper();
            ApplyDrive(accel, footbrake);
            CapSpeed();

            //CalculateRevs();
            GearChanging();

            AddDownForce();
            TractionControl();
        }


        private void CapSpeed()
        {
            float speed = _rigidbody.velocity.magnitude;
            switch (_speedType)
            {
                case SpeedType.MPH:

                    speed *= 2.23693629f;
                    if (speed > _topspeed)
                        _rigidbody.velocity = (_topspeed/2.23693629f) * _rigidbody.velocity.normalized;
                    break;

                case SpeedType.KPH:
                    speed *= 3.6f;
                    if (speed > _topspeed)
                        _rigidbody.velocity = (_topspeed/3.6f) * _rigidbody.velocity.normalized;
                    break;
            }
        }


        private void ApplyDrive(float accel, float footbrake)
        {

            float thrustTorque;
            switch (_boatDriveType)
            {
                case BoatDriveType.FourWheelDrive:
                    thrustTorque = accel * (_currentTorque / 4f);
                    for (int i = 0; i < 4; i++)
                    {
                        _wheelColliders[i].motorTorque = thrustTorque;
                    }
                    break;

                case BoatDriveType.FrontWheelDrive:
                    thrustTorque = accel * (_currentTorque / 2f);
                    _wheelColliders[0].motorTorque = _wheelColliders[1].motorTorque = thrustTorque;
                    break;

                case BoatDriveType.RearWheelDrive:
                    thrustTorque = accel * (_currentTorque / 2f);
                    _wheelColliders[2].motorTorque = _wheelColliders[3].motorTorque = thrustTorque;
                    break;

            }

            for (int i = 0; i < 4; i++)
            {
                if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, _rigidbody.velocity) < 50f)
                {
                    _wheelColliders[i].brakeTorque = _brakeTorque*footbrake;
                }
                else if (footbrake > 0)
                {
                    _wheelColliders[i].brakeTorque = 0f;
                    _wheelColliders[i].motorTorque = -_reverseTorque*footbrake;
                }
            }
        }


        private void SteerHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelhit;
                _wheelColliders[i].GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero)
                    return; // wheels arent on the ground so dont realign the rigidbody velocity
            }

            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(_oldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - _oldRotation) * _steerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                _rigidbody.velocity = velRotation * _rigidbody.velocity;
            }
            _oldRotation = transform.eulerAngles.y;
        }


        // this is used to add more grip in relation to speed
        private void AddDownForce()
        {
            _wheelColliders[0].attachedRigidbody.AddForce(-transform.up*_downforce*
                                                         _wheelColliders[0].attachedRigidbody.velocity.magnitude);
        }


        // checks if the wheels are spinning and is so does three things
        // 1) emits particles
        // 2) plays tiure skidding sounds
        // 3) leaves skidmarks on the ground
        // these effects are controlled through the WheelEffects class
        /*private void CheckForWheelSpin()
        {
            // loop through all wheels
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelHit;
                _wheelColliders[i].GetGroundHit(out wheelHit);

                // is the tire slipping above the given threshhold
                if (Mathf.Abs(wheelHit.forwardSlip) >= _slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= _slipLimit)
                {
                    m_WheelEffects[i].EmitTyreSmoke();

                    // avoiding all four tires screeching at the same time
                    // if they do it can lead to some strange audio artefacts
                    if (!AnySkidSoundPlaying())
                    {
                        m_WheelEffects[i].PlayAudio();
                    }
                    continue;
                }

                // if it wasnt slipping stop all the audio
                if (m_WheelEffects[i].PlayingAudio)
                {
                    m_WheelEffects[i].StopAudio();
                }
                // end the trail generation
                m_WheelEffects[i].EndSkidTrail();
            }
        }*/

        // crude traction control that reduces the power to wheel if the car is wheel spinning too much
        private void TractionControl()
        {
            WheelHit wheelHit;
            switch (_boatDriveType)
            {
                case BoatDriveType.FourWheelDrive:
                    // loop through all wheels
                    for (int i = 0; i < 4; i++)
                    {
                        _wheelColliders[i].GetGroundHit(out wheelHit);

                        AdjustTorque(wheelHit.forwardSlip);
                    }
                    break;

                case BoatDriveType.RearWheelDrive:
                    _wheelColliders[2].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    _wheelColliders[3].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;

                case BoatDriveType.FrontWheelDrive:
                    _wheelColliders[0].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);

                    _wheelColliders[1].GetGroundHit(out wheelHit);
                    AdjustTorque(wheelHit.forwardSlip);
                    break;
            }
        }


        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= _slipLimit && _currentTorque >= 0)
            {
                _currentTorque -= 10 * _tractionControl;
            }
            else
            {
                _currentTorque += 10 * _tractionControl;
                if (_currentTorque > _fullTorqueOverAllWheels)
                {
                    _currentTorque = _fullTorqueOverAllWheels;
                }
            }
        }


        /*private bool AnySkidSoundPlaying()
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_WheelEffects[i].PlayingAudio)
                {
                    return true;
                }
            }
            return false;
        }*/
    }

