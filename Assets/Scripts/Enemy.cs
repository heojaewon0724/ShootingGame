using System.Collections;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject damageTextPrefab;

    private SpriteRenderer spriteRenderer;

    public Color flashColor = Color.red;

    public float flashDuration = 0.1f;

    private Color originalColor;

    public float enemyHp = 1;

    [SerializeField]

    public float moveSpeed = 1f;

    public GameObject Coin;

    public GameObject Effect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.time;
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTr2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            Missile missile = collision.GetComponent<Missile>();
            StopAllCoroutines();
            StartCoroutine("HitColor");
            enemyHp = enemyHp - missile.missileDamege;
            if (enemyHp < 0)
            {
                Destroy(gameObject);
                Instantiate(Coin, transform.position, Quaternion.identity);
                Instantiate(Effect, transform.position, Quaternion.identity);

            }
            TakeDamage(missile.missileDamege);
        }
    }
    IEnumerator HitColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;


    }
    void TakeDamage(int damage)
    {
        DamagePopupManager.Instance.CreateDamageText(damage, transform.position);
    }
}
