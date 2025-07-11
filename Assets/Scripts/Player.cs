using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField]
    float moveSpeed = 1f;

    // 현재 미사일 프리팹 인덱스
    int missIndex = 0;

    // 미사일 프리팹 배열
    public GameObject[] missilePrefab;

    // 미사일 생성 위치
    public Transform spPostion;

    // 미사일 발사 간격(초)
    [SerializeField]
    private float shootInverval = 0.05f;
    

    // 마지막 발사 시간
    private float lastshotTime = 0f;

    [SerializeField]
    private float SpecialshootInverval = 5f;

    public static float SpecialCool = 0f;
    // 마지막 발사 시간
    private float SpeciallastshotTime = 0f;

    // 애니메이터 컴포넌트 참조
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기

    }

    // 매 프레임마다 이동 및 발사 처리
    void Update()
    {
        float Jump = Input.GetAxisRaw("Jump");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Debug.Log("Horizontal Input: " + horizontalInput); // 디버그용 로그 출력
        Vector3 moveTo = new Vector3(horizontalInput, 0, 0);
        transform.position += moveTo * moveSpeed * Time.deltaTime; // 좌우 이동


        // 애니메이션 상태 변경
        if (horizontalInput < 0)
        {
            animator.Play("Left"); // 왼쪽 이동 애니메이션
        }
        else if (horizontalInput > 0)
        {
            animator.Play("Right"); // 오른쪽 이동 애니메이션
        }
        else
        {
            animator.Play("Idle"); // 가운데(정지) 애니메이션
        }
        Shoot(); // 미사일 발사
        if (Jump == 1) // 1: 우클릭, 0: 좌클릭, 2: 휠클릭
        {
            Debug.Log("스페이스바 감지!");
            SpecialShoot();
        }
        if ((Time.time - SpeciallastshotTime) / SpecialshootInverval >= 1)
        {
            SpecialCool = 1;
        }
        else
        {
           SpecialCool = (Time.time - SpeciallastshotTime) / SpecialshootInverval;
        }
        GameManager.Instance.ShowCool(SpecialCool);

    }

    // 미사일 발사 함수
    void Shoot()
    {
        if (Time.time - lastshotTime>shootInverval)
        {
            Instantiate(missilePrefab[missIndex], spPostion.position, Quaternion.identity);
            lastshotTime = Time.time; // 미사일 발사 시간 갱신
        }
    }
    void SpecialShoot()
    {
            Vector3 SpecialspPos1 = spPostion.position;
            Vector3 SpecialspPos2 = spPostion.position;
            SpecialspPos1.x -= 0.5f;
            SpecialspPos2.x += 0.5f;

        if (Time.time - SpeciallastshotTime > SpecialshootInverval)
        {
            Instantiate(missilePrefab[missIndex], SpecialspPos1, Quaternion.identity);
            Instantiate(missilePrefab[missIndex], SpecialspPos2, Quaternion.identity);
            Instantiate(missilePrefab[missIndex], spPostion.position, Quaternion.identity);
            SpeciallastshotTime = Time.time; // 미사일 발사 시간 갱신

        }
    }

    // 미사일 업그레이드 함수
    public void MissileUp()
    {
        missIndex++; // 미사일 종류 업그레이드
        shootInverval = shootInverval - 0.1f; // 발사 간격 감소(더 빠르게)
        if (shootInverval <= 0.1f)
        {
            shootInverval = 0.1f; // 최소 발사 간격 제한
        }
        if (missIndex >= missilePrefab.Length)
        {
            missIndex = missilePrefab.Length - 1; // 인덱스 범위 제한
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 몬스터 태그로 비교
        {
            Destroy(gameObject); // 플레이어 제거        
            GameManager.Instance.GameOver();
        }
        if (GameManager.Instance.coin >= 100)
        {
            GameManager.Instance.Clear();
        }
    }
}

