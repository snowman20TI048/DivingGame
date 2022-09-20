using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;     // �L�����̈ʒu���

    [SerializeField]
    private Transform goal = null;       // �S�[���n�_(����)�̈ʒu���

    private float distance;              // �L�����Ɛ��ʂ܂ł̋����̌v���p


    private bool isGoal;                 // �S�[������p�B������ 0 �ȉ��ɂȂ�����S�[���Ɣ��肵�� true �ɂ���Bfalse �̊Ԃ̓S�[�����Ă��Ȃ����(��������Ɠ��� bool �^�̗��p���@)



    [SerializeField]
    private Text txtDistance;            // �����̒l���󂯎���čX�V���邽�߂̃R���|�[�l���g�������� UnityEmgne.UI���K�v


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




        // ������ 0 �ȉ��ɂȂ�����(������ distance == 0 �Ƃ͂��Ȃ��悤�ɂ��܂��B�Ȃ��Ȃ�Adistance �̒l�� 0 �s�b�^���ɒl���~�܂邱�Ƃ��o���Ȃ����߂ł�)
        if (distance <= 0)
        {

            // ������ 0 �ȉ��ɂȂ����̂ŁA�S�[���Ɣ��肷��
            isGoal = true;

            // ������ 0 �ɂ���(���̏������Ȃ��ƁAdistance �̒l���}�C�i�X�����ɂȂ�܂��B�����ɏ����Ȃ��ŏ��������s���Ă݂܂��傤)
            distance = 0;

            // TODO ���������ۂ̏�����ǉ�����

        }

        // Console�r���[�ɋ�����\������
        //Debug.Log(distance.ToString("F2"));

        // �����̉�ʕ\�����X�V
        txtDistance.text = distance.ToString("F2");


    }
}