using UnityEngine;
using System.Collections.Generic;
///
///  Object Pooling for Robots
///
public class RobotCreation : MonoBehaviour
{
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private int numberOfRobots = 10;
    [SerializeField] private Queue<GameObject> robotPool = new Queue<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateRobots();
    }
    public void CreateRobots()
    {
        for (int i = 0; i < numberOfRobots; i++)
        {
            GameObject robot = Instantiate(robotPrefab, spawnParent);
            robot.name = "Robot_" + i;
            robot.transform.SetParent(spawnParent);
            robot.SetActive(false);
            robotPool.Enqueue(robot);
        }
    }
    public GameObject GetRobot()
    {
        if (robotPool.Count > 0)
        {
            GameObject robot = robotPool.Dequeue(); // Instantly pull the first one
            //robot.SetActive(true);
            return robot;
        }
        return null;
    }
}
