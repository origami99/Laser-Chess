using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private float outerLeft = 5;
    [SerializeField] private float outerRight = 20f;

    private Vector3 _dragOrigin;
    private bool _cameraDragging = true;

    void Update()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float left = Screen.width * 0.2f;
        float right = Screen.width - (Screen.width * 0.2f);

        if (mousePosition.x < left)
        {
            _cameraDragging = true;
        }
        else if (mousePosition.x > right)
        {
            _cameraDragging = true;
        }

        if (_cameraDragging)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y);

            if (move.x > 0f)
            {
                if (this.transform.position.x < outerRight)
                {
                    transform.Translate(move, Space.World);
                }
            }
            else
            {
                if (this.transform.position.x > outerLeft)
                {
                    transform.Translate(move, Space.World);
                }
            }
        }
    }
}