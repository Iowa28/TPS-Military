using UnityEngine;
using UnityEngine.InputSystem;

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
    
    public void OnAim(InputValue value)
    {
        if (value.isPressed)
        {
            SwitchToAimCamera();
        }
        else
        {
            SwitchToThirdCamera();
        }
    }
}
