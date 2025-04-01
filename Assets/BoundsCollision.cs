using System;
using UnityEngine;

public class BoundsCollision : MonoBehaviour
{
    public enum Position { Left, Right, Top, Bottom }
    [SerializeField] private Position position;
    public Vector2 normal;
    void Awake()
    {
        switch (position)
        {
            case Position.Left:
                normal = Vector2.right;
                break;
            case Position.Right:
                normal = Vector2.left;
                break;
            case Position.Top:
                normal = Vector2.down;
                break;
            case Position.Bottom:
                normal= Vector2.up;
                break;
        }
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Emission"))
        {
            EmissionData emissionData = other.gameObject.GetComponent<EmissionData>();
            emissionData.direction = Vector2.Reflect(emissionData.direction, normal);
        }
    }
}
