using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{



    [SerializeField]
    private PlayerController playerController;



    private Vector3 offset;


    void Start()
    {
        // カメラと追従対象のゲームオブジェクトとの距離を補正値として取得
        offset = transform.position - playerController.transform.position;   //  <=  ☆　処理を書き換えます
    }


    void Update()
    {



        // 着水状態になったら
        if (playerController.inWater == true)
        {
            // ここで処理を停止する => ここから下の処理は動かなくなるので、カメラの追従が止まる
            return;
        }



        // 追従対象がいる場合
        if (playerController != null)
        {　　　　　　　　　　　　　　　　　　　　　//  <=  条件を書き換えます

            // カメラの位置を追従対象の位置 + 補正値にする
            transform.position = playerController.transform.position + offset;  //   <=  処理を書き換えます
        }


    }
}
