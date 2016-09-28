using UnityEngine;
using System.Collections;

public class PlayerSoundController : MonoBehaviour
{
    private FadeSounds _waterSounds;
    private FadeSounds _boatSounds;
    private BoatUserControl _playerMovement;
    private Rigidbody _playerRigidbody;
    private float _velocityLimit = 0.3f;

    void Start()
    {
        _waterSounds = GameObject.Find("WaterMoveSound").GetComponent<FadeSounds>();
        _boatSounds = GameObject.Find("BoatMoveSound").GetComponent<FadeSounds>();
        _playerMovement = GameObject.FindWithTag("Player").GetComponent<BoatUserControl>();
        _playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_playerMovement._moving || _playerRigidbody.velocity.x > _velocityLimit || _playerRigidbody.velocity.z > _velocityLimit)
        {
            if (!_waterSounds._playingSound)
                _waterSounds.SwitchSound();
            if(!_boatSounds._playingSound)
                _boatSounds.SwitchSound();
        }
        else
        {
            _waterSounds.StopSound();
            _boatSounds.StopSound();
        }
    }
}
