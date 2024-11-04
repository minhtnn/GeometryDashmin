using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 offset = new Vector3(0, 0, -10f);
        private float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;
        [SerializeField] private float maxX;

        [SerializeField] private Transform target;

        // Update is called once per frame
        void Update()
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 cameraPosition = Camera.main.transform.position;
            //Get 0.5 camera's height
            float cameraHeight = Camera.main.orthographicSize;
            //Get 0.5 camera's width
            float cameraWidth = cameraHeight * Camera.main.aspect;
            float x = cameraPosition.x - cameraWidth * (0.7f);
            float y1 = cameraPosition.y + cameraHeight * (0.7f);
            float y2 = cameraPosition.y - cameraHeight * (0.7f);
            if (target.position.x >= x && (cameraPosition.x + cameraWidth) <= maxX)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x,
                    ((targetPosition - offset).y >= y1 || (targetPosition - offset).y <= y2) ? targetPosition.y : cameraPosition.y, cameraPosition.z),
                    ref velocity, smoothTime);
            }
            if ((cameraPosition.x + cameraWidth) > maxX)
            {
                //MenuScript.isPause = true;
            }
        }
    }
}
