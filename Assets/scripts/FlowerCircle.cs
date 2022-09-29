using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircle : MonoBehaviour
{
    [Header("�ԗ֒ʉߎ��̓��_")]
    public int point;

    [SerializeField]
    private BoxCollider boxCollider;


    ////* ��������ǉ� *////


    [SerializeField]
    private GameObject effectPrefab;


    ////* �����܂� *////


    void Start()
    {

        // �A�^�b�`�����Q�[���I�u�W�F�N�g(�ԗ�)����]������
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }


    // �ԗւ���݂āA���̃Q�[���I�u�W�F�N�g���ԗւɐN�������ꍇ
    private void OnTriggerEnter(Collider other)
    {

        // �ԗւ� BoxCollider �̃X�C�b�`���I�t�ɂ��ďd�������h�~
        boxCollider.enabled = false;

        // �ԗւ��L�����̎q�I�u�W�F�N�g�ɂ���
        transform.SetParent(other.transform);


        ////* ��������C�� *////


        // �ԗւ����������ۂ̉��o
        StartCoroutine(PlayGetEffect());      //�@<=�@���@�R���[�`�����\�b�h�̌Ăяo�����߂ɕύX����


        ////* �����܂� *////

    }


    /// <summary>
    /// �ԗւ����������ۂ̉��o
    /// </summary>
    private IEnumerator PlayGetEffect()
    {�@�@�@//�@<=�@���@�߂�l�� void ���� IEnumerator �ɕύX���āA�R���[�`�����\�b�h�ɕύX����

        // DOTween �� Sequence ��錾���ė��p�ł���悤�ɂ���
        Sequence sequence = DOTween.Sequence();

        // Append �����s����ƁA������DOTween�̏��������s�ł���B�ԗւ� Scale �� 1�b������ 0 �ɂ��Č����Ȃ�����
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));

        // Join �����s���邱�ƂŁAAppend �ƈꏏ��DOTween�̏������s����B�ԗւ� Scale ��1�b������ 0 �ɂȂ�̂ƈꏏ�ɁA�v���C���[�̈ʒu�ɉԗւ��ړ�������
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));


        ////* ��������ǉ� *////


        // 1�b�����𒆒f(�ҋ@����)
        yield return new WaitForSeconds(1.0f);

        // �G�t�F�N�g�𐶐����āAInstantiate ���\�b�h�̖߂�l�� effect �ϐ��ɑ��
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // �G�t�F�N�g�̈ʒu(����)�𒲐�����@<=�@���@���̍����̒������K�v�ȗ��R�͂Ȃ񂾂Ǝv���܂����H�@��x�A���̏������R�����g�A�E�g���āA�ǂ̂悤�ȈႢ���o�邩�m�F���Ă��������B
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y - 1.5f, effect.transform.position.z);

        // 1�b��ɃG�t�F�N�g��j��(�����ɔj������ƃG�t�F�N�g�����ׂčĐ�����Ȃ�����)
        Destroy(effect, 1.0f);


        ////* �����܂� *////


        // �ԗւ�1�b��ɔj��
        Destroy(gameObject, 1.0f);

    }
}