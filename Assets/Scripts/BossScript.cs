using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum BossState
{
    Idle,
    BulletPattern1,
    BulletPattern2,
    BulletPattern3,
    BulletPattern4
}

public class BossScript : MonoBehaviour
{
    public BossState curState;
    private PlayerMove player;
    private Rigidbody rigid;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public GameObject BulletA;
    public GameObject BulletB;
    public GameObject shootPoing;

    private float StateDuration = 3f; // 상태 지속 시간
    private float startTimer; // 타이머 변수

    private void Start()
    {
        player = GetComponent<PlayerMove>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(BossState.Idle);
    }

    private void Update()
    {
        startTimer -= Time.deltaTime;

        if (startTimer <= 0)
        {
            ChangeState(GetNextState());
        }

        Invoke(nameof(ChangeState), 1f); // 상태 실행 전에 1초 대기
    }

    // 상태 변경
    void ChangeState(BossState newState)
    {
        curState = newState;
        startTimer = StateDuration;

        CancelInvoke(); // 이전의 Invoke 호출을 취소
        switch (curState)
        {
            case BossState.BulletPattern1:
                InvokeRepeating("BulletPattern1", 0f, 0.5f); // 0.5초마다 BulletPattern1을 실행
                break;
            case BossState.BulletPattern2:
                InvokeRepeating("BulletPattern2", 0f, 1f); // 1초마다 BulletPattern2를 실행
                break;
            case BossState.BulletPattern3:
                InvokeRepeating("BulletPattern3", 0f, 0.7f); // 0.7초마다 BulletPattern3을 실행
                break;
            case BossState.BulletPattern4:
                InvokeRepeating("BulletPattern4", 0f, 1f); // 2초 후 BulletPattern4 실행
                break;
            default: // Idle 상태에서는 Invoke 취소
                break;
        }
    }

    // 상태 전환을 위한 조건
    BossState GetNextState()
    {
        // 현재 상태에서 다음 상태로 진행
        if (curState == BossState.BulletPattern4) // 마지막 상태일 경우
        {
            return BossState.BulletPattern1; // 처음 상태로 돌아감
        }
        else
        {
            return curState + 1; // 다음 상태로 이동
        }
    }

    void BulletPattern1() // 직선 탄막
    {
        GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.gravityScale = 0f;

        // 발사 방향 설정
        Vector2 direction = new Vector2(-1f, 0f);

        // 속도 설정
        bulletRigid.velocity = direction.normalized * 5f;
    }

    void BulletPattern2() //360도 발사
    {
        int bulletCount = 20; // 한 번에 발사할 총알 수
        float bulletSpeed = 5f;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // 원형으로 퍼져 나가는 방향 설정
            float angle = i * (360f / bulletCount);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle ), Mathf.Sin(Mathf.Deg2Rad * angle));

            // 속도 설정
            bulletRigid.velocity = direction * bulletSpeed;
        }
    }

    void BulletPattern3() //샷건
    {
        int bulletCount = 23; // 한 번에 발사할 총알 수
        float bulletSpeed = 4f; // 총알 속도
        float waveFrequency = 1f;
        float waveAmplitude = 1f;

        for (int i = 0; i < bulletCount; i++)
        {
            // 총알 생성
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // 물결의 위아래 움직임을 추가
            float waveOffset = Mathf.Sin((Time.time + i) * waveFrequency) * waveAmplitude;

            // 발사 방향 설정
            Vector2 direction = new Vector2(-1f, waveOffset); // 왼쪽으로 가면서 파도처럼 위아래로 움직임

            // 속도 설정
            bulletRigid.velocity = direction.normalized * bulletSpeed;
        }
    }

    void BulletPattern4() // 폭발총알
    {
        int bulletCount = 5;
        float bulletSpeed = 5f;
        float explosionDelay = 2f; // 폭발까지의 지연 시간

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // 발사 방향 설정
            float angle = i * (360f / bulletCount);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            bulletRigid.velocity = direction * bulletSpeed;

            // 일정 시간 후 폭발
            StartCoroutine(ExplosionAfterDelay(bullet, explosionDelay));
        }
    }

    IEnumerator ExplosionAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 폭발 효과를 위한 작은 총알들 발사
        int subBulletCount = 6;
        float explosionSpeed = 8f; // 속도를 조금 더 증가시켜 볼 것

        for (int i = 0; i < subBulletCount; i++)
        {
            GameObject subBullet = Instantiate(BulletB, bullet.transform.position, Quaternion.identity);
            Rigidbody2D subBulletRigid = subBullet.GetComponent<Rigidbody2D>();
            subBulletRigid.gravityScale = 0f;  // 중력 무시

            // 분산된 방향으로 작은 총알 발사
            float angle = i * (360f / subBulletCount);
            Vector2 subBulletDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // 작은 총알에 속도 설정
            subBulletRigid.velocity = subBulletDirection * explosionSpeed;
        }

        // 폭발된 총알은 삭제
        Destroy(bullet);
    }
}
