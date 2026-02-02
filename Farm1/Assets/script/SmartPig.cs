using UnityEngine;

public class SimpleWorkingPig : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 dir;
    private float changeTime = 2f;
    private float timer;

    void Start()
    {
        // Проверяем компоненты
        CheckComponents();

        dir = Vector3.forward;
        timer = changeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            dir = Quaternion.Euler(0, Random.Range(-45f, 45f), 0) * dir;
            timer = changeTime;
        }

        // Поворот
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            2f * Time.deltaTime
        );

        // Пробуем двигаться
        TryMove();
    }

    void TryMove()
    {
        // Пробуем сдвинуться
        Vector3 attemptMove = transform.forward * speed * Time.deltaTime;

        // Проверяем можно ли пройти
        if (CanPass(attemptMove))
        {
            // Двигаемся
            transform.position += attemptMove;
        }
        else
        {
            // Не можем - поворачиваем
            TurnAround();
        }
    }

    bool CanPass(Vector3 move)
    {
        // Проверяем весь путь
        RaycastHit hit;
        if (Physics.Raycast(transform.position, move.normalized, out hit, move.magnitude + 0.1f))
        {
            // Проверяем что это не земля
            if (!hit.collider.CompareTag("Ground"))
            {
                return false;
            }
        }

        return true;
    }

    void TurnAround()
    {
        // Простой разворот
        dir = -dir;

        // Добавляем случайность
        dir = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * dir;

        // Ждем перед следующим поворотом
        timer = 1f;

        Debug.Log("Не могу пройти! Поворачиваю.");
    }

    void CheckComponents()
    {
        // Автоматическая настройка если что-то не так
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationZ |
                            RigidbodyConstraints.FreezePositionY;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // На всякий случай
        if (!collision.gameObject.CompareTag("Ground"))
        {
            TurnAround();
        }
    }
}