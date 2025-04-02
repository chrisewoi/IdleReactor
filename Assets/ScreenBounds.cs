using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static float left, right, top, bottom;
    private Camera cam;
    void Awake()
    {
        cam = Camera.main;
        cam.ScreenToWorldPoint(Vector2.zero);
        cam.ScreenToWorldPoint(Vector2.one);
        cam.ScreenToWorldPoint(Vector2.right);
        cam.ScreenToWorldPoint(Vector2.up);
        left = cam.ScreenToWorldPoint(Vector2.zero).x;
        right = -cam.ScreenToWorldPoint(Vector2.zero).x;
        top = -cam.ScreenToWorldPoint(Vector2.zero).y;
        bottom = cam.ScreenToWorldPoint(Vector2.zero).y;
    }

}
