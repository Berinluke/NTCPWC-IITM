using UnityEngine;

public class RestrictZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int targetLayer;
    public string ZoneId;
    void Start()
    {
        targetLayer = LayerMask.NameToLayer("Water");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            RobotController robot = other.GetComponent<RobotController>();
            //Debug.Log("Robot entered the restrict zone" + other.gameObject.name);
            UIManager.instance.ShowZoneWarning(robot.unitId, ZoneId);

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
        {
            RobotController robot = other.GetComponent<RobotController>();
            //Debug.Log("Robot exited the restrict zone" + other.gameObject.name);
            UIManager.instance.HideZoneWarning(robot.unitId, ZoneId);

        }
    }
}
