using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1f;
    int missIndex = 0;

    public GameObject[] missilePrefabs;

    public Transform spPosition;

    [SerializeField]
    private float shootInterval = 0.05f;

    private float lastShotTime = 0f;

    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Debug.Log("Horizontal Input:" + horizontalInput);
        Vector3 moveTo = new Vector3(horizontalInput, 0, 0);
        transform.position += moveTo * moveSpeed * Time.deltaTime;
        if (horizontalInput < 0)
        {
            animator.Play("Left");
        }
        else if (horizontalInput > 0)
        {
            animator.Play("Right");
        }
        else
        {
            animator.Play("Idle");
        }
        Shoot();
    }
    void Shoot()
    {
        if (Time.time - lastShotTime > shootInterval)
        {
            Instantiate(missilePrefabs[missIndex], spPosition.position, Quaternion.identity);
            lastShotTime = Time.time;
        }
    }
    public void MissileUp()
    {
        missIndex++;
        shootInterval = shootInterval - 0.1f;
        if (shootInterval <= 0.1f)
        {
            shootInterval = 0.1f;
        }
        if(missIndex >=missilePrefabs.Length)
        {
            missIndex = missilePrefabs.Length - 1;
        }
    }
}
