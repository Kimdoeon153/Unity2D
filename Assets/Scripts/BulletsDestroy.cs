using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsDestroy : MonoBehaviour
{
    public float destroyTime = 5f; // 총알이 사라질 시간 (초 단위)

    void Start()
    {
        // 일정 시간이 지난 후 총알을 삭제
        Destroy(gameObject, destroyTime);
    }
}
