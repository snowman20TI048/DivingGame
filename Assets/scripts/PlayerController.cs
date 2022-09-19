using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;

    [Header("�������x")]
    public float fallSpeed;

    [Header("��������p�Btrue�Ȃ璅����")]
    public bool inWater;

    private Rigidbody rb;

    private float x;
    private float z;


    ////* ��������ǉ� *////


    [SerializeField, Header("�����Ԃ��̃G�t�F�N�g")]
    private GameObject splashEffectPrefab = null;


    ////* �����܂� *////


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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


            ////* ��������ǉ� *////


            // �����Ԃ��̃G�t�F�N�g�𐶐����āA�������ꂽ�����Ԃ��̃G�t�F�N�g�� effect �ϐ��ɑ��
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);

            // effect �ϐ��𗘗p���āA�G�t�F�N�g�̈ʒu�𒲐�����
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);

            // effect �ϐ��𗘗p���āA�G�t�F�N�g���Q�b��ɔj��
            Destroy(effect, 2.0f);


            // TODO�@�����Ԃ���SE���Đ�      //  <=�@���ꂩ��ǉ����鏈���ɂ��ẮATODO��t���ăR�����g�Ƃ��Ďc���Ă����܂��傤


            ////* �����܂� *////


            Debug.Log("���� :" + inWater);
        }
    }
}