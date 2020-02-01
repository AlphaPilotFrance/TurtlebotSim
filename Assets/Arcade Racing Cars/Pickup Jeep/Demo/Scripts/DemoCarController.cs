using System.Collections.Generic;
using UnityEngine;

namespace Lowscope.ARC
{
    public class DemoCarController : MonoBehaviour
    {
        [SerializeField] private float accelerationTime = 0;
        [SerializeField] private float maxSpeed = 5;

        [SerializeField] private Transform[] wheels = null;
        [SerializeField] private Rigidbody rigidBody = null;
        [SerializeField] private GameObject farCam = null;
        [SerializeField] private MeshRenderer carBase = null;

        [SerializeField] private float maxWheelRotation = 40f;
        [SerializeField] private string colorPropertyName = "";

        private float currentAccelerationTime = 0;
        private float currentVelocity;
        private float currentHorizontal;

        private Quaternion frontWheelRotation;

        private float totalRotation = 0;
        private float addedRotation = 0;
        private float hueTime;
        private float brightness = 1;

        private List<Transform> children = new List<Transform>();

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
        }

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            totalRotation = horizontal * maxWheelRotation;

            for (int i = 0; i < wheels.Length; i++)
            {
                Quaternion targetWheelRotation = Quaternion.Euler(addedRotation, (wheels[i].localPosition.z > 0) ? totalRotation : 0, 0);
                wheels[i].localRotation = Quaternion.Lerp(wheels[i].localRotation, targetWheelRotation, Time.deltaTime * 20f);
            }

            if (vertical != 0)
            {
                currentAccelerationTime += (vertical > 0) ? Time.deltaTime : -Time.deltaTime;
            }
            else
            {
                currentAccelerationTime = Mathf.MoveTowards(currentAccelerationTime, 0, Time.deltaTime);
            }

            currentAccelerationTime = Mathf.Clamp(currentAccelerationTime, -accelerationTime, accelerationTime);

            if (vertical >= 1)
            {
                farCam.gameObject.SetActive(true);
            }
            else
            {
                farCam.gameObject.SetActive(false);
            }

            addedRotation += currentAccelerationTime;

            if (Input.GetKey(KeyCode.Alpha2))
            {
                hueTime += Time.deltaTime;
                if (hueTime > 1)
                {
                    hueTime = 0;
                }
            }

            if (Input.GetKey(KeyCode.Alpha1))
            {
                brightness += Time.deltaTime;
                if (brightness > 1)
                {
                    brightness = 0;
                }
            }

            carBase.material.SetColor(colorPropertyName, Color.HSVToRGB(hueTime, brightness, 1));
        }

        private void FixedUpdate()
        {
            currentVelocity = (currentAccelerationTime * maxSpeed) * Time.deltaTime;

            rigidBody.MovePosition(rigidBody.transform.position + (transform.forward * currentVelocity));

            if (currentVelocity != 0)
            {
                rigidBody.MoveRotation(Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(0, ((totalRotation * 30) * currentAccelerationTime) * Time.deltaTime, 0), Time.deltaTime));
            }
        }
    }
}