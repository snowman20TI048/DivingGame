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




    [SerializeField]
    private AudioClip flowerSE;�@�@// �ʉߎ��Ɗl�����łQ��ʁX��SE��炵�����ꍇ�ɂ͕ϐ����Q�p�ӂ���

    [SerializeField, Header("�ړ�������ꍇ�X�C�b�`�����")]
    private bool isMoveing;

    [SerializeField, Header("�ړ�����")]
    private float duration;

    [SerializeField, Header("�ړ�����")]
    private float moveDistance;



    [SerializeField, Header("�ړ����鎞�ԂƋ����������_���ɂ��銄��"), Range(0, 100)]
    private int randomMovingPercent;

    [SerializeField, Header("�ړ����Ԃ̃����_����")]
    private Vector2 durationRange;

    [SerializeField, Header("�ړ������̃����_����")]
    private Vector2 moveDistanceRange;

    [SerializeField, Header("�傫���̐ݒ�")]
    private float[] flowerSizes;

    [SerializeField, Header("�_���̔{��")]
    private float[] pointRate;


    private bool isMoving;



    void Start()
    {

        // �A�^�b�`�����Q�[���I�u�W�F�N�g(�ԗ�)����]������
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);



        // ���̉ԗւ��ړ�����ԗւ̐ݒ�Ȃ�
        if (isMoveing)
        {

            // �O��Ƀ��[�v�ړ�������
            transform.DOMoveZ(transform.position.z + moveDistance, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }




    }


    // �ԗւ���݂āA���̃Q�[���I�u�W�F�N�g���ԗւɐN�������ꍇ
    private void OnTriggerEnter(Collider other)
    {



        // ���ʂɐG��Ă��ʉߔ���͍s��Ȃ�
        if (other.gameObject.tag == "Water")
        {
            return;
        }



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



        // �ASE�Đ�
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        // �ԗւ�1�b��ɔj��
        Destroy(gameObject, 1.0f);

    }



    /// <summary>
    /// �ړ�����ԗւ̐ݒ�
    /// </summary>
    public void SetUpMovingFlowerCircle(bool isMoving, bool isScaleChanging)
    {

        // �ړ�����ԗւ��A�ʏ�̉ԗւ��̐ݒ�
        this.isMoving = isMoving;

        // �ړ�����ꍇ
        if (this.isMoving)
        {

            // �����_���Ȉړ����Ԃ⋗�����g�����A�߂�l�������\�b�h�𗘗p���Ĕ���
            if (DetectRandomMovingFromPercent())
            {

                // �����_���̏ꍇ�ɂ́A�ړ����ԂƋ����̃����_���ݒ���s��
                ChangeRandomMoveParameters();
            }
        }

        // �ԗւ̑傫����ύX����ꍇ
        if (isScaleChanging)
        {

            // �傫����ύX
            ChangeRandomScales();
        }
    }

    /// <summary>
    /// �ړ����ԂƋ����������_���ɂ��邩����Btrue �̏ꍇ�̓����_���Ƃ���
    /// </summary>
    /// <returns></returns>
    private bool DetectRandomMovingFromPercent()
    {

        // �������ʂ�  bool �l�Ŗ߂��BrandomMovingPercent �̒l�����傫����΁Afalse�A��������������� true
        return Random.Range(0, 100) <= randomMovingPercent;
    }

    /// <summary>
    /// �����_���l���擾���Ĉړ�
    /// </summary>
    private void ChangeRandomMoveParameters()
    {

        // �ړ����Ԃ������_���l�͈̔͂Őݒ�
        duration = Random.Range(durationRange.x, durationRange.y);

        // �ړ������������_���l�͈̔͂Őݒ�
        moveDistance = Random.Range(moveDistanceRange.x, moveDistanceRange.y);
    }

    /// <summary>
    /// �傫����ύX���ē_���ɔ��f
    /// </summary>
    private void ChangeRandomScales()
    {

        // �����_���l�͈͓̔��ő傫����ݒ�
        int index = Random.Range(0, flowerSizes.Length);

        // �傫����ύX
        transform.localScale *= flowerSizes[index];

        // �_����ύX
        point = Mathf.CeilToInt(point * pointRate[index]);   // Mathf.CeilToInt ���\�b�h�ɂ��Ē��ׂĂ݂܂��傤�B
    }





}