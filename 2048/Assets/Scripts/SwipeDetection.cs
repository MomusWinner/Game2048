using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction);

    private Vector2 tapPosition;
    private Vector2 lastDirection;   
    private Vector2 swipeDelta;

    [SerializeField]
    private int deadZone;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tapPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                lastDirection = Input.GetTouch(0).position;
                CheckSwipe();
            }
        }
    }

    private void CheckSwipe()
    {
        swipeDelta = lastDirection - tapPosition;
  
        if(swipeDelta.magnitude > deadZone)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                else
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down);
            }
        }
        ResetSwipe();
    }

    private void ResetSwipe()
    {
        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
        lastDirection = Vector2.zero;
    }
}