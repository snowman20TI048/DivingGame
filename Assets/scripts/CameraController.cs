using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;    //�@<=�@���@�ǉ�

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;


    ////* ��������ǉ� *////


    [SerializeField]
    private Camera fpsCamera;                // ��l�̃J������ Camera �R���|�[�l���g�̑���p

    [SerializeField]
    private Camera selfishCamera;            // ���B��J������ Camera �R���|�[�l���g�̑���p

    [SerializeField]
    private Button btnChangeCameara;         // �J�����̐���p�{�^���� Button �R���|�[�l���g�̑���p

    private int cameraIndex;                 // ���ݓK�p���Ă���J�����̒ʂ��ԍ�
    private Camera mainCamera;               // Main Camera �Q�[���I�u�W�F�N�g�� Camera �R���|�[�l���g�̑���p


    ////* �����܂� *////


    private Vector3 offset;


    void Start()
    {
        // �J�����ƒǏ]�Ώۂ̃Q�[���I�u�W�F�N�g�Ƃ̋�����␳�l�Ƃ��Ď擾
        offset = transform.position - playerController.transform.position;


        ////* ��������ǉ� *////

        // MainCamera Tag �����Q�[���I�u�W�F�N�g(MainCamera �Q�[���I�u�W�F�N�g)�� Camera �R���|�[�l���g���擾���đ��
        mainCamera = Camera.main;

        // �{�^����OnClick�C�x���g�Ƀ��\�b�h��o�^
        btnChangeCameara.onClick.AddListener(ChangeCamera);

        // �J�����������J����(�O�l�̃J����)�ɖ߂� 
        SetDefaultCamera();


        ////* �����܂� *////

    }


    void Update()
    {
        // ������ԂɂȂ�����
        if (playerController.inWater == true)
        {
            // �����ŏ������~���� => �������牺�̏����͓����Ȃ��Ȃ�̂ŁA�J�����̒Ǐ]���~�܂�
            return;
        }

        // �Ǐ]�Ώۂ�����ꍇ
        if (playerController != null)
        {

            // �J�����̈ʒu��Ǐ]�Ώۂ̈ʒu + �␳�l�ɂ���
            transform.position = playerController.transform.position + offset;
        }
    }


    ////* �������烁�\�b�h���Q�ǉ� *////

    /// <summary>
    /// �J������ύX(�{�^�����������тɌĂяo�����)
    /// </summary>
    private void ChangeCamera()
    {

        // ���݂̃J�����̒ʂ��ԍ��ɉ����āA���̃J������p�ӂ��Đ؂�ւ���
        switch (cameraIndex)
        {
            case 0:
                cameraIndex++;
                mainCamera.enabled = false;
                fpsCamera.enabled = true;
                break;
            case 1:
                cameraIndex++;
                fpsCamera.enabled = false;
                selfishCamera.enabled = true;
                break;
            case 2:
                cameraIndex = 0;
                selfishCamera.enabled = false;
                mainCamera.enabled = true;
                break;
        }
    }


    /// <summary>
    /// �J�����������J����(�O�l�̃J����)�ɖ߂�
    /// </summary>
    public void SetDefaultCamera()
    {
        cameraIndex = 0;

        mainCamera.enabled = true;
        fpsCamera.enabled = false;
        selfishCamera.enabled = false;
    }


    ////* �����܂� *////

}

