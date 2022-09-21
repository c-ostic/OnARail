using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrainController : MonoBehaviour
{
    public float speed = 100f;
    public float speedSmoothing = 6f;
    public float positionSmoothing = 10f;
    public float rotationSpeed = 5f;
    public List<GameObject> seats;

    private Railway currRailway;
    private Junction currJunction;
    private Station currStation;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool atJunction;
    private bool atStation;
    private List<Passenger> passengers = new List<Passenger>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Junction[] allJunctions = FindObjectsOfType<Junction>();
        currJunction = allJunctions[Random.Range(0, allJunctions.Length)];

        transform.position = currJunction.transform.position;
        currRailway = currJunction.GetConnections().First();
    }

    private void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // rotate the train to match the rail
        Quaternion rotationTarget = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, rb.velocity));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);

        // if at a station, check for loading/unloading
        if (atStation)
        {
            // Load passengers with left click
            if (Input.GetMouseButtonDown(0))
            {
                LoadPassengers();
            }

            // Unload passengers with right click
            if (Input.GetMouseButtonDown(1))
            {
                UnloadPassengers();
            }
        }
    }

    // Loads as many passengers as possible from the current station
    private void LoadPassengers()
    {
        while (passengers.Count() < seats.Count())
        {
            Passenger nextPassenger = currStation.LoadPassenger();
            if (nextPassenger != null)
            {
                passengers.Add(nextPassenger);

                // sets the parent to the first empty seat available
                Transform openSeat = seats.First(seat => seat.transform.childCount == 0).transform;
                nextPassenger.transform.parent = openSeat;
                nextPassenger.transform.position = openSeat.position;
            }
            else
            {
                break;
            }
        }
    }

    // Unloads any matching passengers at the current station
    private void UnloadPassengers()
    {
        for (int i = passengers.Count() - 1; i >= 0; i--)
        {
            // if the passenger is at the correct destination
            if (passengers[i].GetDestination() == currStation.GetColor())
            {
                // release the passenger from the seat
                Passenger passenger = passengers[i];
                passengers.Remove(passenger);
                passenger.transform.parent = null;

                // and unload them to the station
                currStation.UnloadPassenger(passenger);
            }
        }
    }

    void FixedUpdate()
    {        
        // move the train
        rb.velocity = Vector2.Lerp(rb.velocity, movement * speed, speedSmoothing * Time.fixedDeltaTime);

        // adjust the velocity
        if (atJunction)
        {
            // if at a junction, choose the railway with the highest projection magnitude from the current junction's railways
            Vector2 bestMove = Vector2.zero;
            Railway bestRail = currRailway;
            foreach (Railway railway in currJunction.GetConnections())
            {
                Vector2 tempMove = railway.ClampVelocity(transform, movement * speed);

                if (tempMove.sqrMagnitude > bestMove.sqrMagnitude)
                {
                    bestMove = tempMove;
                    bestRail = railway;
                }
            }
            rb.velocity = bestMove;
            currRailway = bestRail;
            rb.position = Vector2.Lerp(rb.position, currJunction.transform.position, positionSmoothing * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = currRailway.ClampVelocity(transform, rb.velocity);
            rb.position = Vector2.Lerp(rb.position, currRailway.ClampPosition(transform), positionSmoothing * Time.fixedDeltaTime);
        }
    }

    public void SetJunction(bool isAtJunction, Junction newJunction = null)
    {
        atJunction = isAtJunction;
        currJunction = newJunction;
    }

    public void SetStation(bool isAtStation, Station newStation = null)
    {
        atStation = isAtStation;
        currStation = newStation;
    }
}
