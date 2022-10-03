using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleFlower : MonoBehaviour
{
    private Animator anim;
    private BoxCollider boxCol;

    void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {

        // �w�肳�ꂽ�^�O�̃Q�[���I�u�W�F�N�g���N�������ꍇ�ɂ́A������s��Ȃ�
        if (col.gameObject.tag == "Water" || col.gameObject.tag == "FlowerCircle")
        {
            return;
        }

        // �N�����Ă��� col.gameObject(�܂�APenguin �Q�[���I�u�W�F�N�g)�ɑ΂��āATryGetComponent ���\�b�h�����s���APlayerController �N���X�̏����擾�ł��邩���肷��
        if (col.gameObject.TryGetComponent(out PlayerController player))
        {

            // PlayerController �N���X���擾�o�����ꍇ�̂݁A���� if ���̒��̏��������s�����

            // �H�ׂ�
            StartCoroutine(EatingTarget(player));
        }
    }

    /// <summary>
    /// �Ώۂ�H�ׂēf���o��
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private IEnumerator EatingTarget(PlayerController player)
    {

        // �R���C�_�[���I�t�ɂ��ďd�������h�~����
        boxCol.enabled = false;

        // �L���������̒����Ɉړ�������@�@�@�@�@�@�@//�@<=�@���@���̈�A�̏���(�R�s��)���A���́u�L���������̒����Ɉړ�������v�����ɂȂ�̂����l���Ă݂܂��傤�I�@��
        player.transform.SetParent(transform);
        player.transform.localPosition = new Vector3(0, -2.0f, 0);
        player.transform.SetParent(null);

        // �H�ׂ�A�j���Đ�
        anim.SetTrigger("attack");

        // �L�����̈ړ����ꎞ��~���A�L�[���͂��󂯕t���Ȃ���Ԃɂ���
        player.StopMove();

        // �H�ׂĂ���A�j���̎��Ԃ̊Ԃ��������𒆒f
        yield return new WaitForSeconds(0.75f);

        // �L�������ړ��ł����Ԃɖ߂�
        player.ResumeMove();

        // �L���������ɓf���o��
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 300, ForceMode.Impulse);

        // �L��������]������
        player.transform.DORotate(new Vector3(180, 0, 1080), 0.5f, RotateMode.FastBeyond360);

        // �X�R�A�𔼕��ɂ���
        player.HalveScore();

        // �������Ȃ�Ȃ������
        transform.DOScale(Vector3.zero, 0.5f);
        Destroy(gameObject, 0.5f);
    }
}