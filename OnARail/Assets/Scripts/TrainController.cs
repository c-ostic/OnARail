using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainController : MonoBehaviour
{
    private Railway currRailway;
    private Station currStation;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currVelocity;
    private bool atJunction;

    public float speed = 100f;
    public float smoothing = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currStation = FindObjectOfType<Station>();
        transform.position = currStation.transform.position;
        currRailway = currStation.GetConnections().First();
    }

    private void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Clamp the position of the train between the endpoints of the railway
        currRailway.ClampPosition(transform);
    }

    void FixedUpdate()
    {        
        // move the train
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movement * speed * Time.fixedDeltaTime, ref currVelocity, smoothing);

        if (atJunction)
        {
            // if at a junction, choose the railway with the highest projection magnitude from the current station's railways
            Vector2 bestMove = Vector2.zero;
            Railway bestRail = currRailway;
            foreach (Railway railway in currStation.GetConnections())
            {
                Vector2 tempMove = Vector3.Project(rb.velocity, railway.GetDirectionVector());
                if (Mathf.Abs(tempMove.magnitude) > Mathf.Abs(bestMove.magnitude))
                {
                    bestMove = tempMove;
                    bestRail = railway;
                }
            }
            rb.velocity = bestMove;
            currRailway = bestRail;
        }
        else
        {
            rb.velocity = Vector3.Project(rb.velocity, currRailway.GetDirectionVector());
        }
    }

    public void SetJunction(bool isAtJunction, Station newStation = null)
    {
        atJunction = isAtJunction;
        currStation = newStation;
    }
}
