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

    [SerializeField]
    private GameObject effectPrefab;


    ////* ��������ǉ� *////


    [SerializeField]
    private AudioClip flowerSE;�@�@// �ʉߎ��Ɗl�����łQ��ʁX��SE��炵�����ꍇ�ɂ͕ϐ����Q�p�ӂ���

    [SerializeField, Header("�ړ�������ꍇ�X�C�b�`�����")]
    private bool isMoveing;

    [SerializeField, Header("�ړ�����")]
    private float duration;

    [SerializeField, Header("�ړ�����")]
    private float moveDistance;


    ////* �����܂� *////


    void Start()
    {

        // �A�^�b�`�����Q�[���I�u�W�F�N�g(�ԗ�)����]������
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);


        ////* ��������ǉ� *////


        // ���̉ԗւ��ړ�����ԗւ̐ݒ�Ȃ�
        if (isMoveing)
        {

            // �O��Ƀ��[�v�ړ�������
            transform.DOMoveZ(transform.position.z + moveDistance, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }


        ////* �����܂� *////


    }


    // �ԗւ���݂āA���̃Q�[���I�u�W�F�N�g���ԗւɐN�������ꍇ
    private void OnTriggerEnter(Collider other)
    {


        ////* ��������ǉ� *////


        // ���ʂɐG��Ă��ʉߔ���͍s��Ȃ�
        if (other.gameObject.tag == "Water")
        {
            return;
        }


        ////* �����܂� *////


        // �ԗւ� BoxCollider �̃X�C�b�`���I�t�ɂ��ďd�������h�~
        boxCollider.enabled = false;

        // �ԗւ��L�����̎q�I�u�W�F�N�g�ɂ���
        transform.SetParent(other.transform);

        // �ԗւ����������ۂ̉��o
        StartCoroutine(PlayGetEffect());
    }


    /// <summary>
    /// �ԗւ����������ۂ̉��o
    /// </summary>
    private IEnumerator PlayGetEffect()
    {


        ////* ��������ǉ� *////

        // �@SE�Đ�
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        ////* �����܂� *////


        // DOTween �� Sequence ��錾���ė��p�ł���悤�ɂ���
        Sequence sequence = DOTween.Sequence();

        // Append �����s����ƁA������DOTween�̏��������s�ł���B�ԗւ� Scale �� 1�b������ 0 �ɂ��Č����Ȃ�����
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));

        // Join �����s���邱�ƂŁAAppend �ƈꏏ��DOTween�̏������s����B�ԗւ� Scale ��1�b������ 0 �ɂȂ�̂ƈꏏ�ɁA�v���C���[�̈ʒu�ɉԗւ��ړ�������
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));

        // 1�b�����𒆒f(�ҋ@����)
        yield return new WaitForSeconds(1.0f);

        // �G�t�F�N�g�𐶐����āAInstantiate ���\�b�h�̖߂�l�� effect �ϐ��ɑ��
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // �G�t�F�N�g�̈ʒu(����)�𒲐�����@<=�@���@���̍����̒������K�v�ȗ��R�͂Ȃ񂾂Ǝv���܂����H�@��x�A���̏������R�����g�A�E�g���āA�ǂ̂悤�ȈႢ���o�邩�m�F���Ă��������B
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y - 1.5f, effect.transform.position.z);

        // 1�b��ɃG�t�F�N�g��j��(�����ɔj������ƃG�t�F�N�g�����ׂčĐ�����Ȃ�����)
        Destroy(effect, 1.0f);


        ////* ��������ǉ� *////

        // �ASE�Đ�
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        ////* �����܂� *////


        // �ԗւ�1�b��ɔj��
        Destroy(gameObject, 1.0f);

    }
}