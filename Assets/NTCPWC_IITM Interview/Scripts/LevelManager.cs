using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Dictionary<string, RobotController> robotDictionary = new Dictionary<string, RobotController>();
    public float DatalostThreshold;
    public float proximityWarningThreshold;
    public CameraController cameraController;
    public RobotCreation robotCreation;
    public GameObject robotPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ReceiveJson(string incomingJson)
    {
        TelemetryData newData = JsonUtility.FromJson<TelemetryData>(incomingJson);
        if (string.IsNullOrEmpty(newData.unitId) || newData.speed < 0)
        {
            //Debug.Log($"Invalid Data for{newData.unitId}. Droping Record.");
            return;
        }
        if (robotDictionary.ContainsKey(newData.unitId))
        {
            robotDictionary[newData.unitId].UpdateRobotData(newData);
        }
        else
        {

            SpawnNewRobot(newData);
        }
    }
    void SpawnNewRobot(TelemetryData Data)
    {
        Vector3 startPos = new Vector3(Data.x, 0, Data.z);
        GameObject poolRobot = robotCreation.GetRobot();

        if (poolRobot != null)
        {
            poolRobot.transform.position = startPos;
            RobotController newRobot = poolRobot.GetComponent<RobotController>();
            newRobot.UpdateRobotData(Data);
            robotDictionary.Add(Data.unitId, newRobot);
        }
        else
        {
            //Debug.Log("Reached robot creation limit.");
        }
    }

}
