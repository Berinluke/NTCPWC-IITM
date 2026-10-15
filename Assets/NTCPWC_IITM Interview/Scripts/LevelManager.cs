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
    //public GameObject robotPrefab;
    private TelemetryData reusableData = new TelemetryData();
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
        JsonUtility.FromJsonOverwrite(incomingJson,reusableData);
        if (string.IsNullOrEmpty(reusableData.unitId) || reusableData.speed < 0)
        {
            //Debug.Log($"Invalid Data for{newData.unitId}. Droping Record.");
            return;
        }
        if (robotDictionary.ContainsKey(reusableData.unitId))
        {
            robotDictionary[reusableData.unitId].UpdateRobotData(reusableData);
        }
        else
        {

            SpawnNewRobot(reusableData);
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
            robotDictionary.Add(Data.unitId, newRobot);
            newRobot.UpdateRobotData(Data);
            poolRobot.SetActive(true);
        }
        else
        {
            //Debug.Log("Reached robot creation limit.");
        }
    }

}
