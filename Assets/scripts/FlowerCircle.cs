using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlowerCircle : MonoBehaviour
{
    [Header("花輪通過時の得点")]
    public int point;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private GameObject effectPrefab;


    ////* ここから追加 *////


    [SerializeField]
    private AudioClip flowerSE;　　// 通過時と獲得時で２回別々のSEを鳴らしたい場合には変数を２つ用意する

    [SerializeField, Header("移動させる場合スイッチ入れる")]
    private bool isMoveing;

    [SerializeField, Header("移動時間")]
    private float duration;

    [SerializeField, Header("移動距離")]
    private float moveDistance;


    ////* ここまで *////


    void Start()
    {

        // アタッチしたゲームオブジェクト(花輪)を回転させる
        transform.DORotate(new Vector3(0, 360, 0), 5.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);


        ////* ここから追加 *////


        // この花輪が移動する花輪の設定なら
        if (isMoveing)
        {

            // 前後にループ移動させる
            transform.DOMoveZ(transform.position.z + moveDistance, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }


        ////* ここまで *////


    }


    // 花輪からみて、他のゲームオブジェクトが花輪に侵入した場合
    private void OnTriggerEnter(Collider other)
    {


        ////* ここから追加 *////


        // 水面に触れても通過判定は行わない
        if (other.gameObject.tag == "Water")
        {
            return;
        }


        ////* ここまで *////


        // 花輪の BoxCollider のスイッチをオフにして重複判定を防止
        boxCollider.enabled = false;

        // 花輪をキャラの子オブジェクトにする
        transform.SetParent(other.transform);

        // 花輪をくぐった際の演出
        StartCoroutine(PlayGetEffect());
    }


    /// <summary>
    /// 花輪をくぐった際の演出
    /// </summary>
    private IEnumerator PlayGetEffect()
    {


        ////* ここから追加 *////

        // �@SE再生
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        ////* ここまで *////


        // DOTween の Sequence を宣言して利用できるようにする
        Sequence sequence = DOTween.Sequence();

        // Append を実行すると、引数でDOTweenの処理を実行できる。花輪の Scale を 1秒かけて 0 にして見えなくする
        sequence.Append(transform.DOScale(Vector3.zero, 1.0f));

        // Join を実行することで、Append と一緒にDOTweenの処理を行える。花輪の Scale が1秒かけて 0 になるのと一緒に、プレイヤーの位置に花輪を移動させる
        sequence.Join(transform.DOLocalMove(Vector3.zero, 1.0f));

        // 1秒処理を中断(待機する)
        yield return new WaitForSeconds(1.0f);

        // エフェクトを生成して、Instantiate メソッドの戻り値を effect 変数に代入
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        // エフェクトの位置(高さ)を調整する　<=　☆　この高さの調整が必要な理由はなんだと思いますか？　一度、この処理をコメントアウトして、どのような違いが出るか確認してください。
        effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y - 1.5f, effect.transform.position.z);

        // 1秒後にエフェクトを破棄(すぐに破棄するとエフェクトがすべて再生されないため)
        Destroy(effect, 1.0f);


        ////* ここから追加 *////

        // �ASE再生
        AudioSource.PlayClipAtPoint(flowerSE, transform.position);


        ////* ここまで *////


        // 花輪を1秒後に破棄
        Destroy(gameObject, 1.0f);

    }
}