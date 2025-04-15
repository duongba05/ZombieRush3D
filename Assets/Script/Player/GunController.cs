using UnityEngine;

public class GunController : MonoBehaviour
{
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float cooldownTime = 1f;          // Thời gian hồi mỗi lần bắn
    public float holdTimeToPause = 0.5f;     // Thời điểm dừng animation (sau khi giơ súng)

    private float lastShotTime = 0f;
    private bool isFiring = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Khi nhấn chuột trái
        {
            animator.Play("PlayerShoot", 0, 0f);  // Chạy animation bắn từ đầu
            isFiring = true;
        }

        if (Input.GetMouseButton(0) && isFiring)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("PlayerShoot") && state.normalizedTime >= holdTimeToPause && state.normalizedTime < 0.9f)
            {
                animator.speed = 0f;  // Dừng animation tại điểm giơ súng
            }

            if (Time.time - lastShotTime >= cooldownTime)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0))  // Khi thả chuột trái
        {
            animator.speed = 1f;  // Cho animation tiếp tục
            animator.Play("PlayerIdle", 0, 0f);  // Quay lại animation Idle
            isFiring = false;
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Thêm âm thanh hoặc hiệu ứng nếu cần
    }
}
