using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxSpeed = 0.5f; // Velocidad del efecto de parallax

    private float _length, _startPosition;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float distance = (_mainCamera.transform.position.x * parallaxSpeed);
        transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

        if (_mainCamera.transform.position.x > (_startPosition + _length))
        {
            _startPosition += _length;
        }
        else if (_mainCamera.transform.position.x < (_startPosition - _length))
        {
            _startPosition -= _length;
        }
    }
}
