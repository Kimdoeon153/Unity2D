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

    private float StateDuration = 3f; // ���� ���� �ð�
    private float startTimer; // Ÿ�̸� ����

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

        Invoke(nameof(ChangeState), 1f); // ���� ���� ���� 1�� ���
    }

    // ���� ����
    void ChangeState(BossState newState)
    {
        curState = newState;
        startTimer = StateDuration;

        CancelInvoke(); // ������ Invoke ȣ���� ���
        switch (curState)
        {
            case BossState.BulletPattern1:
                InvokeRepeating("BulletPattern1", 0f, 0.5f); // 0.5�ʸ��� BulletPattern1�� ����
                break;
            case BossState.BulletPattern2:
                InvokeRepeating("BulletPattern2", 0f, 1f); // 1�ʸ��� BulletPattern2�� ����
                break;
            case BossState.BulletPattern3:
                InvokeRepeating("BulletPattern3", 0f, 0.7f); // 0.7�ʸ��� BulletPattern3�� ����
                break;
            case BossState.BulletPattern4:
                InvokeRepeating("BulletPattern4", 0f, 1f); // 2�� �� BulletPattern4 ����
                break;
            default: // Idle ���¿����� Invoke ���
                break;
        }
    }

    // ���� ��ȯ�� ���� ����
    BossState GetNextState()
    {
        // ���� ���¿��� ���� ���·� ����
        if (curState == BossState.BulletPattern4) // ������ ������ ���
        {
            return BossState.BulletPattern1; // ó�� ���·� ���ư�
        }
        else
        {
            return curState + 1; // ���� ���·� �̵�
        }
    }

    void BulletPattern1() // ���� ź��
    {
        GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.gravityScale = 0f;

        // �߻� ���� ����
        Vector2 direction = new Vector2(-1f, 0f);

        // �ӵ� ����
        bulletRigid.velocity = direction.normalized * 5f;
    }

    void BulletPattern2() //360�� �߻�
    {
        int bulletCount = 20; // �� ���� �߻��� �Ѿ� ��
        float bulletSpeed = 5f;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // �������� ���� ������ ���� ����
            float angle = i * (360f / bulletCount);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle ), Mathf.Sin(Mathf.Deg2Rad * angle));

            // �ӵ� ����
            bulletRigid.velocity = direction * bulletSpeed;
        }
    }

    void BulletPattern3() //����
    {
        int bulletCount = 23; // �� ���� �߻��� �Ѿ� ��
        float bulletSpeed = 4f; // �Ѿ� �ӵ�
        float waveFrequency = 1f;
        float waveAmplitude = 1f;

        for (int i = 0; i < bulletCount; i++)
        {
            // �Ѿ� ����
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // ������ ���Ʒ� �������� �߰�
            float waveOffset = Mathf.Sin((Time.time + i) * waveFrequency) * waveAmplitude;

            // �߻� ���� ����
            Vector2 direction = new Vector2(-1f, waveOffset); // �������� ���鼭 �ĵ�ó�� ���Ʒ��� ������

            // �ӵ� ����
            bulletRigid.velocity = direction.normalized * bulletSpeed;
        }
    }

    void BulletPattern4() // �����Ѿ�
    {
        int bulletCount = 5;
        float bulletSpeed = 5f;
        float explosionDelay = 2f; // ���߱����� ���� �ð�

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(BulletA, transform.position, Quaternion.identity);
            Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
            bulletRigid.gravityScale = 0f;

            // �߻� ���� ����
            float angle = i * (360f / bulletCount);
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            bulletRigid.velocity = direction * bulletSpeed;

            // ���� �ð� �� ����
            StartCoroutine(ExplosionAfterDelay(bullet, explosionDelay));
        }
    }

    IEnumerator ExplosionAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���� ȿ���� ���� ���� �Ѿ˵� �߻�
        int subBulletCount = 6;
        float explosionSpeed = 8f; // �ӵ��� ���� �� �������� �� ��

        for (int i = 0; i < subBulletCount; i++)
        {
            GameObject subBullet = Instantiate(BulletB, bullet.transform.position, Quaternion.identity);
            Rigidbody2D subBulletRigid = subBullet.GetComponent<Rigidbody2D>();
            subBulletRigid.gravityScale = 0f;  // �߷� ����

            // �л�� �������� ���� �Ѿ� �߻�
            float angle = i * (360f / subBulletCount);
            Vector2 subBulletDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            // ���� �Ѿ˿� �ӵ� ����
            subBulletRigid.velocity = subBulletDirection * explosionSpeed;
        }

        // ���ߵ� �Ѿ��� ����
        Destroy(bullet);
    }
}
