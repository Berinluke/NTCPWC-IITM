using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public struct RobotPair
    {
        public string bot1;
        public string bot2;
    }
    public struct ZoneKey
    {
        public string bot1;
        public string ZoneId;
    }

    [Header("Proximity")]
    public GameObject proximityWarningPanel;
    public Text proximityWarningText;
    private Dictionary<RobotPair, string> activeWarning = new Dictionary<RobotPair, string>();

    [Header("RestirctZone")]
    public GameObject restrictZonePannel;
    public Text restrictZoneText;
    private Dictionary<ZoneKey, string> activeZoneWarning = new Dictionary<ZoneKey, string>();

    [Header("Robot Detail")]
    public GameObject robotDetailPanel;
    public Text robotDetailText;
    public RobotController currentlySelectedRobot;
    private StringBuilder sb = new StringBuilder();
    private float nextUiUpdate = 0f;
    void Start()
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
    void Update()
    {
        if (currentlySelectedRobot != null && Time.time > nextUiUpdate)
        {
            UpdateRobotDetail();
            nextUiUpdate = Time.time + 0.2f;
        }
    }
    //---------------------------------------------------------------------------------Proximity warning
    public void ShowProximityWarning(string robotId1, string robotId2, float distance)
    {
        RobotPair pairKey = new RobotPair { bot1 = robotId1, bot2 = robotId2 };
        activeWarning[pairKey] = $"Warning:{robotId1} & {robotId2}({distance:F1}m)";

        UpdateProximityWarningDisplay();

    }
    public void HideProximityWarning(string robotId1, string robotId2)
    {
        RobotPair pairKey = new RobotPair { bot1 = robotId1, bot2 = robotId2 };
        if (activeWarning.ContainsKey(pairKey))
        {
            activeWarning.Remove(pairKey);
        }
        UpdateProximityWarningDisplay();
    }
    void UpdateProximityWarningDisplay()
    {
        if (activeWarning.Count > 0)
        {
            proximityWarningPanel.SetActive(true);
            /* proximityWarningText.text = string.Join("\n", activeWarning.Values); */
            sb.Clear();
            foreach(var warning in activeWarning.Values)
            {
                sb.AppendLine(warning);
            }
            //Debug.LogWarning(proximityWarningText.text);
            proximityWarningText.text=sb.ToString();
        }
        else
        {
            proximityWarningPanel.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------Restrict Zone warning
    public void ShowZoneWarning(string robotId, string zoneName)
    {

        ZoneKey uniqueKey = new ZoneKey { bot1 = robotId, ZoneId = zoneName };
        activeZoneWarning[uniqueKey] = $"Alert:{robotId} has entered {zoneName}!";
        updateZoneWarningDisplay();
    }
    public void HideZoneWarning(string robotId, string zoneName)
    {
        ZoneKey uniqueKey = new ZoneKey { bot1 = robotId, ZoneId = zoneName };
        if (activeZoneWarning.ContainsKey(uniqueKey))
        {
            activeZoneWarning.Remove(uniqueKey);
        }
        updateZoneWarningDisplay();
    }
    void updateZoneWarningDisplay()
    {
        if (activeZoneWarning.Count > 0)
        {
            restrictZonePannel.SetActive(true);
            //restrictZoneText.text = string.Join("\n", activeZoneWarning.Values);
            sb.Clear();
            foreach(var warning in activeZoneWarning.Values)
            {
                sb.AppendLine(warning);
            }
            restrictZoneText.text = sb.ToString();
            //Debug.LogWarning(restrictZoneText.text);
        }
        else
        {
            restrictZonePannel.SetActive(false);
        }
    }
    //---------------------------------------------------------------------------------Robot Detail
    public void SelectRobot(RobotController newRobot)
    {
        if (currentlySelectedRobot != null)
        {
            currentlySelectedRobot.SetSelected(false);
        }
        currentlySelectedRobot = newRobot;
        currentlySelectedRobot.SetSelected(true);

        robotDetailPanel.SetActive(true);
    }
    public void HideRobotDetail()
    {
        if (currentlySelectedRobot != null)
        {
            currentlySelectedRobot.SetSelected(false);
            currentlySelectedRobot = null;
            robotDetailPanel.SetActive(false);
        }
    }
    void UpdateRobotDetail()
    {
        robotDetailText.text =
        $"ID\t\t:  {currentlySelectedRobot.unitId}\n" +
        $"Name\t\t:  {currentlySelectedRobot.RobotName}\n" +
        $"Speed\t\t:  {currentlySelectedRobot.speed:F1}m/s\n" +
        $"Heading\t:  {currentlySelectedRobot.heading:F0}deg\n" +
        $"Position\t:  X {currentlySelectedRobot.transform.position.x:F1} / Z {currentlySelectedRobot.transform.position.z:F1}\n" +
        $"Data Age\t:  {currentlySelectedRobot.StateWaitTime:F1}\n" +
        $"Status\t\t:  {currentlySelectedRobot.Status}\n";


    }
}
