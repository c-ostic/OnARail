using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    private List<Railway> connections = new List<Railway>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<TrainController>().SetJunction(true, this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<TrainController>().SetJunction(false);
    }

    public void AddConnection(Railway rail)
    {
        connections.Add(rail);
    }

    public IEnumerable<Railway> GetConnections()
    {
        return connections;
    }
}
