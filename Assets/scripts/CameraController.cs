using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    ////* ��������ǉ� *////


    [SerializeField]
    private PlayerController playerController;


    //[SerializeField]                        //  <=  �폜���܂�
    //private GameObject playerObj;           //  <=  �폜���܂�


    ////* �����܂� *////


    private Vector3 offset;


    void Start()
    {
        // �J�����ƒǏ]�Ώۂ̃Q�[���I�u�W�F�N�g�Ƃ̋�����␳�l�Ƃ��Ď擾
        offset = transform.position - playerController.transform.position;   //  <=  ���@���������������܂�
    }


    void Update()
    {

        ////* ��������ǉ� *////


        // ������ԂɂȂ�����
        if (playerController.inWater == true)
        {
            // �����ŏ������~���� => �������牺�̏����͓����Ȃ��Ȃ�̂ŁA�J�����̒Ǐ]���~�܂�
            return;
        }


        ////* �����܂� *////


        // �Ǐ]�Ώۂ�����ꍇ
        if (playerController != null)
        {�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@//  <=  ���������������܂�

            // �J�����̈ʒu��Ǐ]�Ώۂ̈ʒu + �␳�l�ɂ���
            transform.position = playerController.transform.position + offset;  //   <=  ���������������܂�
        }


        ////* �����܂� *////

    }
}
