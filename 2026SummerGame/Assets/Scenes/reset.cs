using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rキーを押すと指定したオブジェクトを初期位置に戻すスクリプト
// 空のGameObjectなどにアタッチし、リセットしたいオブジェクトをリストに登録してください
public class reset : MonoBehaviour
{
    [Header("リセット対象")]
    [SerializeField] private List<Transform> resetTargets = new List<Transform>(); // リセットしたいオブジェクトを登録

    [Header("キー設定")]
    [SerializeField] private KeyCode resetKey = KeyCode.R; // リセットに使うキー

    [Header("リスポーン連動設定")]
    [SerializeField] private bool resetOnRespawn = true; // キャラクターのリスポーン時にもリセットを行うかどうか(Unity上でON/OFF切り替え可能)

    // 各オブジェクトの初期位置・回転を記録しておくための辞書
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();

    void Start()
    {
        // ゲーム開始時点の位置と回転をすべて記録しておく
        foreach (Transform target in resetTargets)
        {
            if (target == null) continue;

            initialPositions[target] = target.position;
            initialRotations[target] = target.rotation;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetAllTargets();
        }
    }

    // respownCharacter.cs など、他のスクリプトから呼び出すための入り口
    // resetOnRespawn がOFFの場合は何もしない
    public void OnPlayerRespawn()
    {
        if (!resetOnRespawn) return;

        ResetAllTargets();
    }

    public void ResetAllTargets()
    {
        foreach (Transform target in resetTargets)
        {
            if (target == null) continue;

            // 位置と回転を初期状態に戻す
            target.position = initialPositions[target];
            target.rotation = initialRotations[target];

            // Rigidbody2Dがあれば速度もリセットする(慣性が残らないように)
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}