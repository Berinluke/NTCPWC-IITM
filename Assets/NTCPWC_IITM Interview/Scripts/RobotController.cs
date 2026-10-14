using UnityEngine;

public class RobotController : MonoBehaviour
{
    public string unitId;
    public string RobotName;
    public string timeStamp;
    public float xCoordinate;
    public float heading; // Angle in degrees
    public float zCoordinate;
    public float speed;
    public string Status;
    public float RotationSpeed = 100f; // Speed at which the robot rotates towards the target heading
    public bool istargetReched;
    public float StopThreshold;
    public float StateWaitTime;
    //public bool isDataNotRecived;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RobotMovement();
        checkTargetReached();
        /* if (istargetReched && !isDataNotRecived)
        {
            Debug.LogError("funtion calling" + gameObject.name);
            CheckWaitingTime();
        } */
        CheckWaitingTime();
    }
    public void UpdateRobotData(TelemetryData incomingData)
    {
        this.unitId = incomingData.unitId;
        this.RobotName = incomingData.name;
        this.timeStamp = incomingData.timestamp;
        this.xCoordinate = incomingData.x;
        this.heading = incomingData.heading;
        this.zCoordinate = incomingData.z;
        this.speed = incomingData.speed;
        Status = "Normal";
        // isDataNotRecived = false;
        StateWaitTime = 0f;
    }
    public void RobotMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xCoordinate, transform.position.y, zCoordinate), speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, heading, 0), RotationSpeed * Time.deltaTime);
    }
    public void checkTargetReached()
    {
        Vector3 target = new Vector3(xCoordinate, 0, zCoordinate);
        // float distanceToTaget = Vector3.Distance(transform.position, target);
        float sqrDistance = (transform.position - target).sqrMagnitude;
        if (sqrDistance < (StopThreshold * StopThreshold))
        {
            //Debug.Log($"{gameObject.name} has reached the target!");
            istargetReched = true;
            // isDataNotRecived = true;
        }
    }
    public void CheckWaitingTime()
    {
        StateWaitTime += Time.deltaTime;
        if (StateWaitTime > LevelManager.instance.DatalostThreshold)
        {
            Status = "Data Lost";
            //Debug.Log(Status+ gameObject.name);
        }
    }
}
