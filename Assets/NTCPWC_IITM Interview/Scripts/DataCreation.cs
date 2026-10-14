using System.Collections;
using UnityEngine;
[System.Serializable]
public class TelemetryData
{
    public string unitId;
    public string name;
    public string timestamp;
    public float x;
    public float z;
    public float heading;
    public float speed;
}
public class DataCreation : MonoBehaviour
{

    public float minTimeDataCration;
    public float maxTimeDataCration;
    public float minXCoordinate;
    public float maxXCoordinate;
    public float minZCoordinate;
    public float maxZCoordinate;
    public int minId;
    public int maxId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateDataRoutine());
    }

    IEnumerator GenerateDataRoutine()
    {
        while (true)
        {
            float randomWait = Random.Range(minTimeDataCration, maxTimeDataCration);
            yield return new WaitForSeconds(randomWait);
            CreatAndSendJsonData();
        }

    }
    void CreatAndSendJsonData()
    {
        TelemetryData data = new TelemetryData();
        int randomId = Random.Range(minId, maxId);
        data.unitId = "BOT_" + randomId.ToString("000");
        data.name = "Robot" + randomId;
        data.timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        data.x = Random.Range(minXCoordinate, maxXCoordinate);
        data.z = Random.Range(minZCoordinate, maxZCoordinate);
        data.heading = Random.Range(0f, 360f);
        data.speed = Random.Range(2f, 15f);
        string jsonData = JsonUtility.ToJson(data);
        //Debug.Log($"Json Data:{jsonData}");
        LevelManager.instance.ReceiveJson(jsonData);
    }

}
