using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsDestroy : MonoBehaviour
{
    public float destroyTime = 5f; // �Ѿ��� ����� �ð� (�� ����)

    void Start()
    {
        // ���� �ð��� ���� �� �Ѿ��� ����
        Destroy(gameObject, destroyTime);
    }
}
