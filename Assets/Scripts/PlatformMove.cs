using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Vector3 startPos; // 시작 위치
    public Vector3 endPos;   // 끝 위치
    public float speed = 2.0f; // 이동 속도
    private bool movingToEnd = true;

    void Start()
    {
        // 초기 위치 설정
        if (startPos == Vector3.zero)
            startPos = transform.position;
    }

    void Update()
    {
        // 이동 로직
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
                movingToEnd = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.1f)
                movingToEnd = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos, endPos);
    }





}
