using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定範囲にキャラクターが侵入したらリスポーン位置に戻すスクリプト
// 落下死ゾーンやトゲなど、危険エリアのオブジェクトにアタッチしてください
// このオブジェクト自身のCollider2Dが「指定範囲」の役割を果たします(Is Trigger を必ずONにする)
[RequireComponent(typeof(Collider2D))]
public class respownCharacter : MonoBehaviour
{
    [Header("リスポーン設定")]
    [SerializeField] private Transform respawnPoint;    // リスポーンさせる位置(空のGameObjectをシーン上に置いて指定)
    [SerializeField] private string playerTag = "Player"; // 判定対象のタグ

    [Header("リセット連動(任意)")]
    [SerializeField] private reset resetScript; // reset.cs がついたオブジェクトを指定すると、リスポーン時に連動してリセットされる

    [Header("画面暗転演出")]
    [SerializeField] private CanvasGroup fadeCanvasGroup; // 画面全体を覆う黒いImageのCanvasGroup
    [SerializeField] private float fadeDuration = 0.5f;   // 暗転・明転にかかる時間(秒)

    private bool isRespawning = false; // 演出中に多重でリスポーンが発生しないようにするフラグ

    void Start()
    {
        // 範囲の役割を果たすコライダーは必ずTriggerにしておく
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning(gameObject.name + " のColliderがTriggerになっていません。Is Trigger をONにしてください。");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (isRespawning) return; // 演出中は多重発生を無視する

        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn Point が設定されていません。");
            return;
        }

        StartCoroutine(RespawnPlayerWithFade(other.gameObject));
    }

    IEnumerator RespawnPlayerWithFade(GameObject player)
    {
        isRespawning = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // 演出中にプレイヤーが落下・移動し続けないよう、物理演算を一時停止する
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        // ---- 暗転(フェードアウト) ----
        yield return StartCoroutine(Fade(0f, 1f));

        // 画面が暗くなった状態で位置をリスポーン地点に移動
        player.transform.position = respawnPoint.position;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // reset.cs が設定されていれば、リスポーンと連動してリセットを行う
        // (reset.cs 側の「Reset On Respawn」がOFFの場合は内部で何もしない)
        if (resetScript != null)
        {
            resetScript.OnPlayerRespawn();
        }

        // ---- 明転(フェードイン) ----
        yield return StartCoroutine(Fade(1f, 0f));

        // 物理演算を再開してプレイヤーの操作を戻す
        if (rb != null)
        {
            rb.simulated = true;
        }

        isRespawning = false;
    }

    // fadeCanvasGroup の透明度を from から to へ fadeDuration 秒かけて変化させる
    IEnumerator Fade(float from, float to)
    {
        if (fadeCanvasGroup == null)
        {
            // フェード用のCanvasGroupが未設定の場合は演出をスキップする
            yield break;
        }

        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;

        // alphaが完全にtoの値になった状態を最低1フレームは確実に描画させてから
        // 呼び出し元(位置移動処理など)へ処理を戻す
        yield return null;
    }

    // シーンビューでリスポーン位置を可視化する(デバッグ用)
    void OnDrawGizmosSelected()
    {
        if (respawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(respawnPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, respawnPoint.position);
        }
    }
}