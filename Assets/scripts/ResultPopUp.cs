using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPopUp : MonoBehaviour
{
    [SerializeField]
    private Button btnRetry;                      // �����̕ϐ����Ȃ��K�v�Ȃ̂����l���܂��傤

    [SerializeField]
    private CanvasGroup canvasGroupTxt;

    [SerializeField]
    private CanvasGroup canvasGroupPopUp;

    [SerializeField]
    private Image imgTitle;

    private bool isClickable;


    void Start()
    {
        // �e�A���t�@�l�� 0 �ɂ��āA���U���g�\�����\���ɂ��Ă���
        canvasGroupPopUp.alpha = 0;
        canvasGroupTxt.alpha = 0;

        // �{�^���� OnClick �C�x���g�Ƀ��\�b�h��o�^
        btnRetry.onClick.AddListener(OnClickRetry);       //btnRetry���N���b�N������AOnClickRetry�����s�����悤�ɂ���

        // �{�^����񊈐������ĉ����Ȃ���Ԃɂ���
        btnRetry.interactable = false;

        // �{�^���̘A�Ŗh�~�p�̔���l�𖢃^�b�v��Ԃɂ���
        isClickable = false;
    }

    /// <summary>
    /// ���U���g�\�����s��
    /// </summary>
    public void DisplayResult()
    {
        // CanvasGroup �̃A���t�@��ύX���ă��U���g�\���BOnComplete ���\�b�h�𗘗p����ƁADOFade���\�b�h�̏������I�������ɓo�^���Ă��鏈���������I�Ɏ��s���Ă����
        canvasGroupPopUp.DOFade(1.0f, 1.0f)    //DoFade(alpha�l,���b�����āj
            .OnComplete(() =>  /*Dofade���I�������*/
             {
                 // �{�^�������������ĉ�����悤�ɂ���
                  btnRetry.interactable = true;

                // ���g���C�̕�����_��
                 canvasGroupTxt.DOFade(1.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);   //setloop(�J��Ԃ���,�����j
             });

        // �^�C�g���̑傫����ύX���邽�߁A��U�A���݂̑傫����ϐ��ɑ�����ĕێ�����
        Vector3 scale = imgTitle.transform.localScale;

        // �^�C�g���̑傫���� 0 �ɂ��Ĕ�\����Ԃɂ���
        imgTitle.transform.localScale = Vector3.zero;

        // �V�[�P���X�̐錾
        Sequence sequence = DOTween.Sequence();    //�����ɓ������Ȃ��悤�ɂ��邽�߂�sequence.Append���K�v

        // 1�b�҂�
        sequence.AppendInterval(1.0f);

        // �^�C�g���̑傫���� 0.25 �b������ 1.5 �{�ɂ���
        sequence.Append(imgTitle.transform.DOScale(1.5f, 0.25f));

        // �O�� Append ���\�b�h���I�����Ă���A���̃��\�b�h�����������
        // �^�C�g���̑傫���� 0.15 �b�����Č��̑傫���ɖ߂��@=>�@��u�傫���Ȃ鉉�o�ɂȂ�
        sequence.Append(imgTitle.transform.DOScale(scale, 0.15f));
    }

    /// <summary>
    /// ���U���g���^�b�v�����ۂ̏���
    /// </summary>
    private void OnClickRetry()
    {

        // ���łɃ^�b�v�ς̏ꍇ
        if (isClickable == true)
        {
            // �d���h�~�̂��߃��g���C�������s��Ȃ�
            return;
        }

        // �^�b�v�ςɂ���B�ȍ~�́A��ɂ��� if ���̐���ɂ��A�^�b�v���Ă�������̏����ɂ͂��Ȃ��Ȃ�
        isClickable = true;

        // ���g���C����
        StartCoroutine(Retry());
    }

    /// <summary>
    /// ���g���C
    /// </summary>
    /// <returns></returns>
    private IEnumerator Retry()
    {

        // ���U���g�����X�ɔ�\���ɂ���
        canvasGroupPopUp.DOFade(0, 1.0f);

        // ���U���g����\���ɂȂ�܂őҋ@
        yield return new WaitForSeconds(1.0f);

        // ���݂Ɠ����V�[����ǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
