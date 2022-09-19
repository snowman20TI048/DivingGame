using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    [Header("落下速度")]
    public float fallSpeed;

    [Header("着水判定用。trueなら着水済")]
    public bool inWater;

    private Rigidbody rb;

    private float x;
    private float z;


    ////* ここから追加 *////


    [SerializeField, Header("水しぶきのエフェクト")]
    private GameObject splashEffectPrefab = null;


    ////* ここまで *////


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        // キー入力の受付
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // velocity(速度)に新しい値を代入して移動
        rb.velocity = new Vector3(x * moveSpeed, -fallSpeed, z * moveSpeed);
    }


    // IsTriggerがオンのコライダーを持つゲームオブジェクトを通過した場合に呼び出されるメソッド
    private void OnTriggerEnter(Collider col)
    {

        // 通過したゲームオブジェクトのTagが Water であり、かつ、isWater が false(未着水)であるなら
        if (col.gameObject.tag == "Water" && inWater == false)
        {

            // 着水状態に変更する
            inWater = true;


            ////* ここから追加 *////


            // 水しぶきのエフェクトを生成して、生成された水しぶきのエフェクトを effect 変数に代入
            GameObject effect = Instantiate(splashEffectPrefab, transform.position, Quaternion.identity);

            // effect 変数を利用して、エフェクトの位置を調整する
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);

            // effect 変数を利用して、エフェクトを２秒後に破壊
            Destroy(effect, 2.0f);


            // TODO　水しぶきのSEを再生      //  <=　これから追加する処理については、TODOを付けてコメントとして残しておきましょう


            ////* ここまで *////


            Debug.Log("着水 :" + inWater);
        }
    }
}