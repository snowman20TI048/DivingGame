using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircle : MonoBehaviour
{

    ////* ��������ǉ� *////

    [Header("�ԗ֒ʉߎ��̓��_")]
    public int point;


    ////* �����܂� *////


    void Start()
    {

        // �A�^�b�`�����Q�[���I�u�W�F�N�g(�ԗ�)����]������
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }
}