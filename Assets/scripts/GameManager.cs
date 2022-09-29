using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;     // �L�����̈ʒu���

    [SerializeField]
    private Transform goal;              // �S�[���n�_(����)�̈ʒu���

    [SerializeField]
    private Text txtDistance;            // �����̒l���󂯎���čX�V���邽�߂̃R���|�[�l���g��������

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private ResultPopUp resultPopUp;


    ////* ��������ǉ� *////


    [SerializeField]
    private AudioManager audioManager;


    ////* �����܂� *////


    private float distance;              // �L�����Ɛ��ʂ܂ł̋����̌v���p

    private bool isGoal;                 // �S�[������p�B������ 0 �ȉ��ɂȂ�����S�[���Ɣ��肵�� true �ɂ���Bfalse �̊Ԃ̓S�[�����Ă��Ȃ����(��������Ɠ��� bool �^�̗��p���@)


    void Update()
    {
        // ������ 0 �ȉ��ɂȂ�����S�[�������Ɣ��肵�ċ����̌v�Z�͍s��Ȃ��悤�ɂ���
        if (isGoal == true)
        {

            // return ������ƁA���̏����������̏����͏�������Ȃ�
            return;
        }

        // Y���������̏��Ȃ̂ŁA�o���̍����̒l�����Z���č����l�������Ƃ���
        distance = player.transform.position.y - goal.position.y;

        txtDistance.text = distance.ToString("F2");

        // ������ 0 �ȉ��ɂȂ�����
        if (distance <= 0)
        {

            // ������ 0 �ȉ��ɂȂ����̂ŁA�S�[���Ɣ��肷��
            isGoal = true;

            txtDistance.text = 0.ToString("F2");

            // �J�����������̃J�����ɖ߂�
            cameraController.SetDefaultCamera();

            // ���U���g�\��
            resultPopUp.DisplayResult();


            ////* ��������ǉ� *////


            // �Q�[���N���A��BGM���Đ�����
            audioManager.PlayBGM(AudioManager.BgmType.GameClear);


            ////* �����܂� *////


        }
    }
}