using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField, Header("BGM�p�I�[�f�B�I�t�@�C��")]
    private AudioClip[] bgms = null;

    private AudioSource audioSource;         // BGM�Đ��p�R���|�[�l���g

    // BGM�̎�ނ�o�^
    public enum BgmType
    {
        Main,          // �����I�� 0 ������U���Ă���
        GameClear      // ����������l�ɁA1 ������U���Ă���
    }

    void Start()
    {

        // �R���|�[�l���g���擾���đ������
        audioSource = GetComponent<AudioSource>();

        // �Q�[������BGM���Đ�
        PlayBGM(BgmType.Main);
    }

    /// <summary>
    /// �w�肵����ނ�BGM���Đ�
    /// </summary>
    /// <param name="bgmType"></param>
    public void PlayBGM(BgmType bgmType)
    {
        // BGM��~
        audioSource.Stop();

        // �Đ�����BGM��ݒ肷��
        audioSource.clip = bgms[(int)bgmType];   //(int)bgmtype��enum�̔ԍ��ɕς��

        // BGM�Đ�
        audioSource.Play();

        Debug.Log("�Đ�����BGM : " + bgmType);
    }
}
