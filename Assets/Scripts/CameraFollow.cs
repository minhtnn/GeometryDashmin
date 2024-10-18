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
        private Vector3 offset = new Vector3(0f, 0f, -10f);
        private float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;
        [SerializeField]
        private Transform target;

        private Camera mainCamera;
        private float screenHeight;
        private float screenWidth;

        void Start()
        {
            mainCamera = Camera.main;
            screenHeight = mainCamera.orthographicSize * 2f;
            screenWidth = screenHeight * mainCamera.aspect;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 targetPosition = transform.position; // Bắt đầu với vị trí hiện tại của camera

            // Lấy tọa độ 50% khung hình theo chiều X
            float cameraXMidpoint = transform.position.x - (screenWidth / 2) + (screenWidth * 0.5f); // Vị trí 50% khung hình

            // Di chuyển camera theo chiều X khi player đạt đúng 50% khung hình
            if (target.position.x > cameraXMidpoint)
            {
                targetPosition.x = target.position.x;  // Di chuyển camera theo player
            }

            // Lấy tọa độ 80% chiều cao khung hình theo chiều Y
            float cameraYThreshold = transform.position.y + (screenHeight * 0.8f); // 80% chiều cao

            // Di chuyển camera theo chiều Y khi player nhảy trên 80% chiều cao khung hình
            if (target.position.y > cameraYThreshold)
            {
                targetPosition.y = target.position.y;
            }

            // SmoothDamp để camera di chuyển mượt mà
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, smoothTime);
        }
    }

}
