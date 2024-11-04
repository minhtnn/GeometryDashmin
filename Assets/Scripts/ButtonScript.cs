using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public float pressScale = 0.9f;  // Tỷ lệ thu nhỏ khi nhấn
    public float bounceBackSpeed = 10f;  // Tốc độ bật lại
    private Vector3 originalScale;  // Kích thước gốc của RectTransform
    private RectTransform rectTransform;
    private bool isPressed = false;

    private void Start()
    {
        // Lưu lại kích thước ban đầu của RectTransform
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform)
        {
            originalScale = rectTransform.localScale;
        }
    }

    public void OnPointerDown()
    {
        // Thu nhỏ khi nhấn
        
            rectTransform.localScale = originalScale * pressScale;
            isPressed = true;
        
    }

    public void OnPointerUp()
    {
            // Bật lại kích thước ban đầu khi nhả chuột
            isPressed = false;
    }

    public void Update()
    {
            if (!isPressed && rectTransform.localScale != originalScale)
            {
                // Bật lại kích thước từ từ để tạo hiệu ứng bounce
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, originalScale, Time.deltaTime * bounceBackSpeed);
            }
        
            
    }
}

