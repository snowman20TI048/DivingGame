using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;     // キャラの位置情報

    [SerializeField]
    private Transform goal;              // ゴール地点(水面)の位置情報

    [SerializeField]
    private Text txtDistance;            // 距離の値を受け取って更新するためのコンポーネントを代入する

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private ResultPopUp resultPopUp;

    [SerializeField]
    private AudioManager audioManager;




    [SerializeField, Header("ステージをランダム生成する場合にはチェックする")]
    private bool isRandomStaging;

    [SerializeField, Header("移動する花輪の割合"), Range(0, 100)]
    private int movingFlowerCirclePercent;

    [SerializeField, Header("大きさが変化する花輪の割合"), Range(0, 100)]
    private int scalingFlowerCirclePercent;

    [SerializeField]
    private FlowerCircle flowerCirclePrefab;　　　　// 花輪のプレファブ・ゲームオブジェクトにアタッチされている FlowerCircle スクリプトをアサインする(同じプレファブ)

    [SerializeField]
    private Transform limitLeftBottom;　　　　　　　// キャラの移動制限用のオブジェクトを生成位置の制限にも利用する

    [SerializeField]
    private Transform limitRightTop;        // キャラの移動制限用のオブジェクトを生成位置の制限にも利用する



    [SerializeField]
    private Slider sliderAltimeter;                 // Slider コンポーネントの操作を行うための変数


    [SerializeField]
    private SkyboxChanger skyboxChanger;            // SkyboxChanger スクリプトの操作を行うための変数


    [SerializeField, Header("障害物とアイテムをランダムに生成する場合にはチェックする")]
    private bool isRandomObjects;

    [SerializeField, Header("障害物とアイテムのプレファブ登録")]
    private GameObject[] randomObjPrefabs;




    private float startPos;                         // ゲーム開始時のキャラの位置情報を代入するための変数





    private float distance;              // キャラと水面までの距離の計測用

    private bool isGoal;                 // ゴール判定用。距離が 0 以下になったらゴールと判定して true にする。false の間はゴールしていない状態(着水判定と同じ bool 型の利用方法)



    void Awake()
    {

        // Skyboxの変更
        skyboxChanger.ChangeSkybox();
    }






    void Update()
    {
        // 距離が 0 以下になったらゴールしたと判定して距離の計算は行わないようにする
        if (isGoal == true)
        {

            // return があると、この処理よりも下の処理は処理されない
            return;
        }

        // Y軸が高さの情報なので、双方の高さの値を減算して差分値を距離とする
        distance = player.transform.position.y - goal.position.y;

        txtDistance.text = distance.ToString("F2");



        // 高度計用のキャラのアイコンの位置を更新
        sliderAltimeter.DOValue(distance / startPos, 0.1f);





        // 距離が 0 以下になったら
        if (distance <= 0)
        {

            // 距離が 0 以下になったので、ゴールと判定する
            isGoal = true;

            txtDistance.text = 0.ToString("F2");

            // カメラを初期のカメラに戻す
            cameraController.SetDefaultCamera();

            // リザルト表示
            resultPopUp.DisplayResult();


            // ゲームクリアのBGMを再生する
            audioManager.PlayBGM(AudioManager.BgmType.GameClear);


        }
    }




    IEnumerator Start()　　　　　　　　　// 戻り値が void ではないので注意
    {
        // スタート地点取得
        startPos = player.transform.position.y;


        // Startメソッドの処理を別のメソッド化して、外部から実行する
        player.SetUpPlayer();


        // キャラの移動を一時停止(キー入力も受け付けない)
        player.StopMove();

        // Updateを止める
        isGoal = true;

        // 花輪をランダムで配置する場合
        if (isRandomStaging)
        {

            // 花輪の生成処理を行う。この処理が終了するまで、次の処理を中断する
            yield return StartCoroutine(CreateRandomStage());
        }



        // 障害物とアイテムをランダムで配置する場合
        if (isRandomObjects)
        {

            // 障害物とアイテムをランダムに生成して設置する
            yield return StartCoroutine(CreateRandomObjects());
        }




        // Updateを再開
        isGoal = false;


        // キャラの移動を再開(キー入力受付開始)
        player.ResumeMove();


        Debug.Log(isGoal);
    }








    /// <summary>
    /// ランダムで花輪を生成してステージ作成
    /// </summary>
    private IEnumerator CreateRandomStage()
    {

        // 花輪の高さのスタート位置
        float flowerHeight = goal.position.y;

        // 花輪を生成した数
        int count = 0;
        Debug.Log("初期の花輪のスタート位置 : " + flowerHeight);

        // 花輪の高さがキャラの位置に到達するまで、ループ処理を行って花輪を生成する。キャラの位置に到達したらループを終了する
        while (flowerHeight <= player.transform.position.y)
        {

            // 花輪の高さを加算(float 型の Random.Range メソッドは 10.0f を含む)
            flowerHeight += Random.Range(5.0f, 10.0f);

            Debug.Log("現在の花輪の生成位置 : " + flowerHeight);

            // 花輪の位置を設定して生成
            FlowerCircle flowerCircle = Instantiate(flowerCirclePrefab, new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), flowerHeight, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)), Quaternion.identity);

            // 花輪の初期設定を呼び出す。引数には評価後の戻り値を利用する。このとき、移動するかどうか、大きさを変えるかどうかの情報を引数として渡す
            flowerCircle.SetUpMovingFlowerCircle(Random.Range(0, 100) <= movingFlowerCirclePercent, Random.Range(0, 100) <= scalingFlowerCirclePercent);

            // 花輪の生成数を加算
            count++;

            Debug.Log("花輪の合計生成数 : " + count);

            // 1フレームだけ中断。　　※　この処理を入れないと無限ループしてUnityがフリーズします。
            yield return null;
        }

        Debug.Log("ランダムステージ完成");
    }


    /// <summary>
    /// // 障害物とアイテムをランダムに生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateRandomObjects()
    {

        // ステージの長さ
        int height = (int)(goal.position.y);
        int count = 0;
        Debug.Log("初期のスタート位置 : " + height);

        while (height <= player.transform.position.y)
        {
            height += Random.Range(10, 15);

            Debug.Log("現在の生成位置 : " + height);

            // 位置を設定して生成
            Instantiate(randomObjPrefabs[Random.Range(0, randomObjPrefabs.Length)], new Vector3(Random.Range(limitLeftBottom.position.x, limitRightTop.position.x), height, Random.Range(limitLeftBottom.position.z, limitRightTop.position.z)), Quaternion.identity);

            count++;

            Debug.Log("障害物とアイテムの合計生成数 : " + count);

            yield return null;
        }

        Debug.Log("障害物とアイテムのランダム配置完成");
    }








}