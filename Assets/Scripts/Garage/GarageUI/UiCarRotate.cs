using UnityEngine;

namespace Garage.GarageUI
{
    public class UiCarRotate : MonoBehaviour
    {
        [SerializeField] float sensitivity;
        private bool isDragging = false;
        private float rotationY = 0f;
        private Camera mainCamera;

        void Start()
        {
            rotationY = -transform.rotation.eulerAngles.y;
            mainCamera = Camera.main;
        }

        void Update()
        {
            OnInput();
        }

        private void OnInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Car"))
                    {
                        isDragging = true;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

                rotationY += mouseX;
                transform.rotation = Quaternion.Euler(0f, -rotationY, 0f);
            }
        }
    }
}