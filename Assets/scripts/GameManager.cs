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

    [SerializeField]
    private AudioManager audioManager;




    [SerializeField, Header("�X�e�[�W�������_����������ꍇ�ɂ̓`�F�b�N����")]
    private bool isRandomStaging;

    [SerializeField, Header("�ړ�����ԗւ̊���"), Range(0, 100)]
    private int movingFlowerCirclePercent;

    [SerializeField, Header("�傫�����ω�����ԗւ̊���"), Range(0, 100)]
    private int scalingFlowerCirclePercent;

    [SerializeField]
    private FlowerCircle flowerCirclePrefab;�@�@�@�@// �ԗւ̃v���t�@�u�E�Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��� FlowerCircle �X�N���v�g���A�T�C������(�����v���t�@�u)

    [SerializeField]
    private Transform limitLeftBottom;�@�@�@�@�@�@�@// �L�����̈ړ������p�̃I�u�W�F�N�g�𐶐��ʒu�̐����ɂ����p����

    [SerializeField]
    private Transform limitRightTop;�@�@�@�@�@�@�@�@// �L�����̈ړ������p�̃I�u�W�F�N�g�𐶐��ʒu�̐����ɂ����p����



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


            // �Q�[���N���A��BGM���Đ�����
            audioManager.PlayBGM(AudioManager.BgmType.GameClear);


        }
    }




    IEnumerator Start()�@�@�@�@�@�@�@�@�@// �߂�l�� void �ł͂Ȃ��̂Œ���
    {
        // Update���~�߂�
        isGoal = true;

        // �ԗւ������_���Ŕz�u����ꍇ
        if (isRandomStaging)
        {

            // �ԗւ̐����������s���B���̏������I������܂ŁA���̏����𒆒f����
            yield return StartCoroutine(CreateRandomStage());
        }

        // Update���ĊJ
        isGoal = false;
        Debug.Log(isGoal);
    }

    /// <summary>
    /// �����_���ŉԗւ𐶐����ăX�e�[�W�쐬
    /// </summary>
    private IEnumerator CreateRandomStage()
    {

        // �ԗւ̍����̃X�^�[�g�ʒu
        float flowerHeight = goal.position.y;

        // �ԗւ𐶐�������
        int count = 0;
        Debug.Log("�����̉ԗւ̃X�^�[�g�ʒu : " + flowerHeight);

        // �ԗւ̍������L�����̈ʒu�ɓ��B����܂ŁA���[�v�������s���ĉԗւ𐶐�����B�L�����̈ʒu�ɓ��B�����烋�[�v���I������
        while (flowerHeight <= player.transform.position.y)
        {

            // �ԗւ̍��������Z(float �^�� Random.Range ���\�b�h�� 10.0f ���܂�)
            flowerHeight += Random.Range(5.0f, 10.0f);

            Debug.Log("���݂̉ԗւ̐����ʒu : " + flowerHeight);

            // �ԗւ̈ʒu��ݒ肵�Đ���
            FlowerCircle flowerCircle = Instantiate(flowerCirclePrefab, new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), flowerHeight, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)), Quaternion.identity);

            // �ԗւ̏����ݒ���Ăяo���B�����ɂ͕]����̖߂�l�𗘗p����B���̂Ƃ��A�ړ����邩�ǂ����A�傫����ς��邩�ǂ����̏��������Ƃ��ēn��
            flowerCircle.SetUpMovingFlowerCircle(Random.Range(0, 100) <= movingFlowerCirclePercent, Random.Range(0, 100) <= scalingFlowerCirclePercent);

            // �ԗւ̐����������Z
            count++;

            Debug.Log("�ԗւ̍��v������ : " + count);

            // 1�t���[���������f�B�@�@���@���̏��������Ȃ��Ɩ������[�v����Unity���t���[�Y���܂��B
            yield return null;
        }

        Debug.Log("�����_���X�e�[�W����");
    }




}