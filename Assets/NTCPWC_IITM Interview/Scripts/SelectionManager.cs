using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] GameObject selectedObject;
    private Camera mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SelectObject();
    }
    public void SelectObject()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Mouse Clicked");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Water")))
            {

                selectedObject = hit.collider.gameObject;
                RobotController robot=selectedObject.GetComponent<RobotController>();
                UIManager.instance.SelectRobot(robot);
                LevelManager.instance.cameraController.FollowSelectedObject(selectedObject);
                //Debug.Log("Selected Object: " + selectedObject.name);
            }
        }

    }
}
