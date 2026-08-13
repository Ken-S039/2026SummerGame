using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーに追従する2Dカメラ用スクリプト
// メインカメラにアタッチしてください
public class cameraMove : MonoBehaviour
{
    [Header("追従対象")]
    [SerializeField] private Transform target;         // 追従するプレイヤーのTransform

    [Header("追従設定")]
    [SerializeField] private float smoothTime = 0.15f;  // 追従の滑らかさ(小さいほど速く追いつく)
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f); // プレイヤーからのオフセット(Zは必ず負の値)

    [Header("マップ範囲の制限(任意)")]
    [SerializeField] private bool useBounds = false;    // ステージ端でカメラを止めるか
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Vector3 velocity = Vector3.zero;            // SmoothDamp用の内部速度(参照渡しで自動更新される)

    void Start()
    {
        if (target == null)
        {
            // タグ"Player"がついたオブジェクトを自動で探す
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    // カメラの移動はキャラクターの移動後(LateUpdate)に行うことでカクつきを防ぐ
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        // 滑らかに追従(SmoothDampで急なカメラワークを避ける)
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // マップ範囲の制限をかける場合
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        transform.position = smoothedPosition;
    }
}