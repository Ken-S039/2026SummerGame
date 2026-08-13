using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーが押すことで動かせるオブジェクト用スクリプト
// Rigidbody2D と Collider2D をアタッチしたオブジェクトにセットしてください
[RequireComponent(typeof(Rigidbody2D))]
public class mobileObject : MonoBehaviour
{
    [Header("押す設定")]
    [SerializeField] private float pushSpeed = 2f;       // 押された時に動く速さ
    [SerializeField] private string playerTag = "Player"; // プレイヤーを判別するタグ

    [Header("押せる角度の判定")]
    [Range(0f, 90f)]
    [SerializeField] private float maxPushAngle = 45f;    // 横からの押しとみなす角度の許容範囲(上や下からの接触で動かないようにする)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // 押された時に回転してしまわないようにする
    }

    void FixedUpdate()
    {
        // 何にも押されていない間は横方向の速度を止める(慣性で滑り続けないように)
        // ※ただし重力による落下(Y方向)には影響を与えない
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    // プレイヤーが接触し続けている間、接触方向にオブジェクトを押す
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 接触面の法線(オブジェクト側から見た向き)を使って押す方向を判定
            Vector2 normal = contact.normal;

            // 法線が横方向に近い(=真横から押されている)場合のみ反応する
            float angle = Vector2.Angle(normal, Vector2.up);
            bool isSideHit = Mathf.Abs(angle - 90f) <= maxPushAngle;

            if (isSideHit)
            {
                // 法線と逆方向(プレイヤーが進んできた方向)にオブジェクトを動かす
                float pushDirection = -Mathf.Sign(normal.x);
                rb.velocity = new Vector2(pushDirection * pushSpeed, rb.velocity.y);
                break;
            }
        }
    }
}