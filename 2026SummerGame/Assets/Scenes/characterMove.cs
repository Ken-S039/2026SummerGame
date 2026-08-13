using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マリオ風2Dアクションゲーム用のキャラクター操作スクリプト
// Rigidbody2D と Collider2D をアタッチしたオブジェクトにセットしてください
[RequireComponent(typeof(Rigidbody2D))]
public class characterMove : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;      // 横移動の速さ

    [Header("ジャンプ設定")]
    [SerializeField] private float jumpForce = 12f;      // ジャンプの強さ
    [SerializeField] private float fallMultiplier = 2.5f; // 落下時の重力倍率(落下を速くして気持ちよくする)
    [SerializeField] private float lowJumpMultiplier = 2f; // ジャンプボタンを離した時の重力倍率

    [Header("地面判定")]
    [SerializeField] private Transform groundCheck;      // 足元に置く空オブジェクト
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;      // 地面のレイヤーを指定

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ---- 入力の取得 ----
        moveInput = Input.GetAxisRaw("Horizontal"); // ←→ or A/D

        // ---- 地面判定 ----
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // ---- ジャンプ入力(ジャンプボタンが押された瞬間) ----
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // ---- キャラクターの向きを入力に合わせて反転 ----
        FlipCharacter();
    }

    void FixedUpdate()
    {
        // ---- 横移動(物理演算はFixedUpdateで行う) ----
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ---- ジャンプの落下を調整してマリオらしい軽快な動きにする ----
        if (rb.velocity.y < 0)
        {
            // 落下中は重力を強めにかける
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // ジャンプボタンを離したら早めに落下開始(可変ジャンプの高さ)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // 移動方向に応じてスプライトを反転させる
    void FlipCharacter()
    {
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // シーンビューで地面判定の範囲を可視化する(デバッグ用)
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}