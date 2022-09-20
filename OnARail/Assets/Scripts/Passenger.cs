using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color destination;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetDestination(Color dest)
    {
        destination = dest;
        sprite.color = dest;
    }

    public Color GetDestination()
    {
        return destination;
    }
}
