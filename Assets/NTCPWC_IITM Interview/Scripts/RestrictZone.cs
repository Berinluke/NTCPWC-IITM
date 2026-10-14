using UnityEngine;

public class RestrictZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int targetLayer;
    void Start()
    {
        targetLayer = LayerMask.NameToLayer("Water");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            Debug.Log("Robot entered the restrict zone" + other.gameObject.name);

            // You can add additional logic here, such as stopping the robot or triggering an event.
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            Debug.Log("Robot exited the restrict zone" + other.gameObject.name);

            // You can add additional logic here, such as resuming the robot's movement or triggering an event.
        }
    }
}
