using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;

    [Header("�������x")]
    public float fallSpeed;

    [Header("��������p�Btrue�Ȃ璅����")]
    public bool inWater;


    ////* ��������ǉ� *////

    // �L�����̏�Ԃ̎��
    public enum AttitudeType
    {
        Straight,        // �����~(�ʏ펞)
        Prone,           // ����
    }

    [Header("���݂̃L�����̎p��")]
    public AttitudeType attitudeType;


    ////* �����܂� *////


    private Rigidbody rb;

    private float x;
    private float z;

    private Vector3 straightRotation = new Vector3(180, 0, 0);     // ������(���ʕ���)�Ɍ�����ۂ̉�]�p�x�̒l

    private int score;                                             // �ԗւ�ʉ߂����ۂ̓��_�̍��v�l�Ǘ��p


    ////* ��������ǉ� *////


    private Vector3 proneRotation = new Vector3(-90, 0, 0);        // �����̎p���̉�]�p�x�̒l


    ////* �����܂� *////


    [SerializeField, Header("�����Ԃ��̃G�t�F�N�g")]
    private GameObject waterEffectPrefab = null;

    [SerializeField, Header("�����Ԃ���SE")]
    private AudioClip splashSE = null;

    [SerializeField]
    private Text txtScore;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // �����̎p����ݒ�(���𐅖ʕ����Ɍ�����)
        transform.eulerAngles = straightRotation;


        ////* ��������ǉ� *////


        // ���݂̎p�����u�����~�v�ɕύX(���܂܂ł̎p��)
        attitudeType = AttitudeType.Straight;


        ////* �����܂� *////

    }

    void FixedUpdate()
    {

        // �L�[���͂̎�t
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // velocity(���x)�ɐV�����l�������Ĉړ�
        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);
    }

    // IsTrigger���I���̃R���C�_�[�����Q�[���I�u�W�F�N�g��ʉ߂����ꍇ�ɌĂяo����郁�\�b�h
    private void OnTriggerEnter(Collider col)
    {

        // �ʉ߂����Q�[���I�u�W�F�N�g��Tag�� Water �ł���A���AisWater �� false(������)�ł���Ȃ�
        if (col.gameObject.tag == "Water" && inWater == false)
        {

            // ������ԂɕύX����
            inWater = true;

            // �����Ԃ��̃G�t�F�N�g�𐶐�
            GameObject effect = Instantiate(waterEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);

            // �G�t�F�N�g���Q�b��ɔj��
            Destroy(effect, 2.0f);

            // �����Ԃ���SE���Đ�
            AudioSource.PlayClipAtPoint(splashSE, transform.position);

            // �R���[�`�����\�b�h�ł��� OutOfWater ���\�b�h���Ăяo��
            StartCoroutine(OutOfWater());
        }

        // �N�������Q�[���I�u�W�F�N�g�� Tag �� FlowerCircle �Ȃ�
        if (col.gameObject.tag == "FlowerCircle")
        {

            // �N������ FlowerCircle Tag �����Q�[���I�u�W�F�N�g(Collider)�̐e�I�u�W�F�N�g(FlowerCircle)�ɃA�^�b�`����Ă��� FlowerCircle �X�N���v�g���擾���āApoint �ϐ����Q�Ƃ��A���_�����Z����
            score += col.transform.parent.GetComponent<FlowerCircle>().point;

            // ��ʂɕ\������Ă��链�_�\�����X�V
            txtScore.text = score.ToString();
        }
    }

    /// <summary>
    /// ���ʂɊ���o��
    /// </summary>
    /// <returns></returns>
    private IEnumerator OutOfWater()
    {
        // �P�b�҂�
        yield return new WaitForSeconds(1.0f);   //  <= yield �ɂ�鏈���Byield return new WaitForSeconds���\�b�h�́A�����Ŏw�肵���b���������̏����ֈڂ炸�ɏ������ꎞ��~���鏈�� 

        // Rigidbody �R���|�[�l���g�� IsKinematic �ɃX�C�b�`�����ăL�����̑�����~����
        rb.isKinematic = true;

        // �L�����̎p���i��]�j��ύX����
        transform.eulerAngles = new Vector3(-30, 180, 0);

        // DOTween�𗘗p���āA�P�b�����Đ������琅�ʂւƃL�������ړ�������
        transform.DOMoveY(4.7f, 1.0f);
    }


    ////* �������烁�\�b�h���Q�ǉ� *////


    void Update()
    {

        // �X�y�[�X�L�[����������
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // �p���̕ύX
            ChangeAttitude();
        }
    }

    /// <summary>
    /// �p���̕ύX
    /// </summary>
    private void ChangeAttitude()
    {

        // ���݂̎p���ɉ����Ďp����ύX����
        switch (attitudeType)
        {

            // ���݂̎p�����u�����~�v��������
            case AttitudeType.Straight:

                // ���݂̎p�����u�����v�ɕύX
                attitudeType = AttitudeType.Prone;

                // �L��������]�����āu�����v�ɂ���
                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);

                // ��C��R�̒l���グ�ė������x��x������
                rb.drag = 25.0f;

                // �����𔲂���(���� case �ɂ͏���������Ȃ�)
                break;

            // ���݂̎p�����u�����v��������
            case AttitudeType.Prone:

                // ���݂̎p�����u�����~�v�ɕύX
                attitudeType = AttitudeType.Straight;

                // �L��������]�����āu�����~�v�ɂ���
                transform.DORotate(straightRotation, 0.25f);

                // ��C��R�̒l�����ɖ߂��ė������x��߂�
                rb.drag = 0f;

                // �����𔲂���
                break;
        }
    }


    ////* �����܂� *////

}