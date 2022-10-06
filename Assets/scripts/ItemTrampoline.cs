using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemTrampoline : MonoBehaviour
{
    private BoxCollider boxCol;

    [SerializeField, Header("���˂��Ƃ��̋�C��R�l")]
    private float airResistance;

    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        // �w�肳�ꂽ�^�O�̃Q�[���I�u�W�F�N�g���ڐG�����ꍇ�ɂ́A������s��Ȃ�
        if (col.gameObject.tag == "Water" || col.gameObject.tag == "FlowerCircle")
        {
            return;
        }

        // �N�����Ă����Q�[���I�u�W�F�N�g�� PlayerController �X�N���v�g�������Ă�����擾
        if (col.gameObject.TryGetComponent(out PlayerController player))
        {

            // �o�E���h������
            Bound(player);
        }
    }

    /// <summary>
    /// �o�E���h������
    /// </summary>
    /// <param name="player"></param>
    private void Bound(PlayerController player)
    {

        // �R���C�_�[���I�t�ɂ��ďd�������h�~����
        boxCol.enabled = false;

        // �L���������Ƀo�E���h������(����͉\)
        player.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(800, 1000), ForceMode.Impulse);

        // �L��������]������(��]�����������F�X�ƕς��Ă݂܂��傤�I)
        player.transform.DORotate(new Vector3(90, 1080, 0), 1.5f, RotateMode.FastBeyond360)
            .OnComplete(() => {
                // ���΂炭�̊ԁA�������x���������ɂ���
                player.DampingDrag(airResistance);
            });

        Destroy(gameObject);
    }
}

