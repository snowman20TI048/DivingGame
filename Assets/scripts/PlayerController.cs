using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Coffee.UIExtensions;     //  <=  ���@ShinyEffectForUGUI �𗘗p���邽�߂ɕK�v�Ȑ錾�ł��B

public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;

    [Header("�������x")]
    public float fallSpeed;

    [Header("��������p�Btrue�Ȃ璅����")]
    public bool inWater;

    // �L�����̏�Ԃ̎��
    public enum AttitudeType
    {
        Straight,        // �����~(�ʏ펞)
        Prone,           // ����
    }

    [Header("���݂̃L�����̎p��")]
    public AttitudeType attitudeType;


    private Rigidbody rb;

    private float x;
    private float z;

    private Vector3 straightRotation = new Vector3(180, 0, 0);     // ������(���ʕ���)�Ɍ�����ۂ̉�]�p�x�̒l

    private int score;                                             // �ԗւ�ʉ߂����ۂ̓��_�̍��v�l�Ǘ��p

    private Vector3 proneRotation = new Vector3(-90, 0, 0);        // �����̎p���̉�]�p�x�̒l

    private float attitudeTimer;                                   // �p���ύX���\�ɂȂ�܂ł̌v���p�^�C�}�[
    private float chargeTime = 2.0f;                               // �p���ύX���\�ɂȂ�܂ł̃`���[�W(�ҋ@)����

    private bool isCharge;                                         // �`���[�W��������p�Bfalse �͖�����(�`���[�W��)�Atrue �̓`���[�W����

    [SerializeField, Header("�����Ԃ��̃G�t�F�N�g")]
    private GameObject waterEffectPrefab = null;

    [SerializeField, Header("�����Ԃ���SE")]
    private AudioClip splashSE = null;

    [SerializeField]
    private Text txtScore;

    [SerializeField]
    private Button btnChangeAttitude;

    [SerializeField]
    private Image imgGauge;


    ////* ��������ǉ� *////


    [SerializeField]
    private ShinyEffectForUGUI shinyEffect;


    ////* �����܂� *////


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // �����̎p����ݒ�(���𐅖ʕ����Ɍ�����)
        transform.eulerAngles = straightRotation;

        // ���݂̎p�����u�����~�v�ɕύX(���܂܂ł̎p��)
        attitudeType = AttitudeType.Straight;

        // �{�^����OnClick�C�x���g�� ChangeAttitude ���\�b�h��ǉ�����
        btnChangeAttitude.onClick.AddListener(ChangeAttitude);

        // �{�^����񊈐���(�������ŉ����Ȃ����)
        btnChangeAttitude.interactable = false;
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

    void Update()
    {

        // �X�y�[�X�L�[����������
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // �p���̕ύX
            ChangeAttitude();
        }

        // �`���[�W������Ԃł͂Ȃ��A�p�������ʂ̏��
        if (isCharge == false && attitudeType == AttitudeType.Straight)
        {

            // �^�C�}�[�����Z���� = �`���[�W���s��
            attitudeTimer += Time.deltaTime;

            // �Q�[�W�\�����X�V
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            // �{�^����񊈐���(�������ŉ����Ȃ����)
            btnChangeAttitude.interactable = false;

            // �^�C�}�[���`���[�W����(���^��)�ɂȂ�����
            if (attitudeTimer >= chargeTime)
            {

                // �^�C�}�[�̒l���`���[�W�̎��ԂŎ~�߂�悤�ɂ���
                attitudeTimer = chargeTime;

                // �`���[�W��Ԃɂ���
                isCharge = true;

                // �{�^����������(��������)
                btnChangeAttitude.interactable = true;


                ////* ��������ǉ� *////


                // ���^�����̃G�t�F�N�g
                shinyEffect.Play(0.5f);


                ////* �����܂� *////

            }
        }

        // �p���������̏��
        if (attitudeType == AttitudeType.Prone)
        {

            // �^�C�}�[�����Z���� = �`���[�W�����炷
            attitudeTimer -= Time.deltaTime;

            // �Q�[�W�\�����X�V
            imgGauge.DOFillAmount(attitudeTimer / chargeTime, 0.1f);

            // �^�C�}�[(�`���[�W)�� 0 �ȉ��ɂȂ�����
            if (attitudeTimer <= 0)
            {

                // �^�C�}�[�����Z�b�g���āA�ēx�v���ł����Ԃɂ���
                attitudeTimer = 0;

                // �{�^����񊈐���(�������ŉ����Ȃ����)
                btnChangeAttitude.interactable = false;

                // �����I�Ɏp���𒼊��~�ɖ߂�
                ChangeAttitude();
            }
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

                // ���`���[�W���(�`���[�W��)�Ȃ�
                if (isCharge == false)
                {

                    // �ȍ~�̏������s��Ȃ� = ���`���[�W��ԂȂ̂ŁA�`���[�W���̏������s���Ȃ��悤�ɂ���
                    return;
                }

                // �`���[�W��Ԃ𖢃`���[�W��Ԃɂ���
                isCharge = false;

                // ���݂̎p�����u�����v�ɕύX
                attitudeType = AttitudeType.Prone;

                // �L��������]�����āu�����v�ɂ���
                transform.DORotate(proneRotation, 0.25f, RotateMode.WorldAxisAdd);

                // ��C��R�̒l���グ�ė������x��x������
                rb.drag = 25.0f;

                // �{�^���̎q�I�u�W�F�N�g�̉摜����]������
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 180), 0.25f);

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

                // �{�^���̎q�I�u�W�F�N�g�̉摜����]������
                btnChangeAttitude.transform.GetChild(0).DORotate(new Vector3(0, 0, 90), 0.25f);

                // �����𔲂���
                break;
        }
    }
}
