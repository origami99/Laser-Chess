using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _lerpSpeed = 1f;

    private float _xDeg;
    private float _yDeg;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _xDeg -= Input.GetAxis("Mouse X") * _speed;
            _yDeg += Input.GetAxis("Mouse Y") * _speed;
        }

        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = Quaternion.Euler(_yDeg, _xDeg, 0);

        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * _lerpSpeed);
    }
}
