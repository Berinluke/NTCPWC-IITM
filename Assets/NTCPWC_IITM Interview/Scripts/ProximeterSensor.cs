using UnityEngine;

public class ProximeterSensor : MonoBehaviour
{
    public RobotController myRobot;
    public SphereCollider proximityTrigger;
    private int robotLayer;
    private float nextProximityUpdate = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        robotLayer = LayerMask.NameToLayer("Water");
        if (LevelManager.instance != null)
        {
            proximityTrigger.radius = LevelManager.instance.proximityWarningThreshold/* /2f */;

        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == robotLayer)
        {
            if (Time.time < nextProximityUpdate) return;
            nextProximityUpdate = Time.time + 0.2f;
            
            RobotController otherRobot = other.GetComponent<RobotController>();
            //Debug.LogError($"{other.gameObject.name} is in the range of {gameObject.name}");
            if (otherRobot != null && string.Compare(myRobot.unitId, otherRobot.unitId) < 0)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                UIManager.instance.ShowProximityWarning(myRobot.unitId, otherRobot.unitId, distance);
                //Debug.LogError("proximity warning" + name);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == robotLayer)
        {
            RobotController otherRobot = other.GetComponent<RobotController>();
            if (otherRobot != null && string.Compare(myRobot.unitId, otherRobot.unitId) < 0)
            {
                UIManager.instance.HideProximityWarning(myRobot.unitId, otherRobot.unitId);
            }
        }
    }
}
