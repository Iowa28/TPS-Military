using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject aimCamera;

    [SerializeField]
    private GameObject aimCanvas;

    [SerializeField]
    private GameObject thirdPersonCamera;

    [SerializeField]
    private GameObject thirdPersonCanvas;

    private bool isAiming;

    private void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
            
            SwitchToAimCamera();
        }
        else
        {
            isAiming = false;
            
            SwitchToThirdCamera();
        }
    }

    private void SwitchToAimCamera()
    {
        thirdPersonCamera.SetActive(false);
        thirdPersonCanvas.SetActive(false);
        aimCamera.SetActive(true);
        aimCanvas.SetActive(true);
    }

    private void SwitchToThirdCamera()
    {
        thirdPersonCamera.SetActive(true);
        thirdPersonCanvas.SetActive(true);
        aimCamera.SetActive(false);
        aimCanvas.SetActive(false);
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}
