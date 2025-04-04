using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitVsync : MonoBehaviour
{
    void Start()
    {
        // Đồng bộ tốc độ khung hình (FPS) với tốc độ làm tươi của màn hình
        QualitySettings.vSyncCount = 1;
    }
}
