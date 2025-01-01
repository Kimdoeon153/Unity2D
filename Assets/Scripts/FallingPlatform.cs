using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float waitTime = 2f; // 기다릴 시간 (초)
    public float fallSpeed = 2f; // 떨어지는 속도

    private bool isFalling = false;
    private Vector3 originalPosition; // 원래 위치 저장
    private bool isPlayerOnPlatform = false; // 플레이어가 플랫폼에 있는지 확인

    private void Awake()
    {
        originalPosition = transform.position; // 원래 위치 저장
    }

    void Update()
    {
        if (isFalling)
        {
            // 플랫폼이 떨어지도록 이동
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    // 플랫폼에 플레이어가 닿았을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerOnPlatform)
        {
            isPlayerOnPlatform = true;
            isFalling = true; // 플레이어가 닿았을 때 플랫폼이 떨어지기 시작
            StartCoroutine(ResetPlatformAfterTime(waitTime)); // 일정 시간 후 플랫폼 재생성
        }
    }

    // 일정 시간 후 다시 플랫폼을 원위치로 되돌리고 멈춤
    private IEnumerator ResetPlatformAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // 일정 시간 대기
        isFalling = false; // 플랫폼 떨어지기 멈춤
        transform.position = originalPosition; // 원래 위치로 되돌리기
        yield return new WaitForSeconds(1f); // 잠시 멈춘 후 다시 재생성
        isPlayerOnPlatform = false; // 다시 플레이어가 올라올 수 있게
    }
}
