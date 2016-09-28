using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {

    private AudioSource _audio;
    private Renderer _renderer;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _renderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        _audio.Play();
        _renderer.enabled = false;

        float audioLenght = _audio.clip.length;

        StartCoroutine(Hide(audioLenght));
    }

    IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
