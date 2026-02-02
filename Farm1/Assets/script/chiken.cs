using UnityEngine;

public class FinalChicken : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 1f;           // Скорость ходьбы
    public float rotateSpeed = 3f;     // Скорость поворота

    [Header("Поведение")]
    public float directionChangeTime = 3f; // Смена направления каждые N секунд

    // Приватные переменные
    private Vector3 moveDirection;     // Направление движения
    private float timer;               // Таймер до смены направления

    void Start()
    {
        // УДАЛЯЕМ ВСЕ ЛИШНИЕ КОМПОНЕНТЫ
        RemoveBadComponents();

        // Добавляем нормальный Collider если нет
        AddProperCollider();

        // Начальное направление
        GetNewDirection();
        timer = directionChangeTime;
    }

    void Update()
    {
        // Обновляем таймер
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GetNewDirection();
            timer = directionChangeTime;
        }

        // Проверяем столкновение ВПЕРЕДИ
        if (CheckCollisionAhead())
        {
            AvoidCollision();
        }

        // Поворачиваем к цели
        RotateChicken();

        // Двигаемся
        MoveChicken();
    }

    void RemoveBadComponents()
    {
        // Удаляем Rigidbody если есть (он вызывает падение)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

        // Удаляем CharacterController если есть (он тоже может мешать)
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) Destroy(cc);
    }

    void AddProperCollider()
    {
        // Если нет коллайдера - добавляем
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
            col.height = 0.5f;
            col.radius = 0.2f;
            col.center = new Vector3(0, 0.25f, 0);
        }

        // Убеждаемся что коллайдер НЕ триггер
        GetComponent<Collider>().isTrigger = false;
    }

    bool CheckCollisionAhead()
    {
        // Проверяем лучами в трех направлениях
        float checkDistance = 0.3f;
        Vector3 pos = transform.position + Vector3.up * 0.1f; // Чуть выше земли

        // 1. Прямо
        if (Physics.Raycast(pos, transform.forward, checkDistance))
            return true;

        // 2. Слева-вперед
        Vector3 leftDir = (transform.forward - transform.right * 0.3f).normalized;
        if (Physics.Raycast(pos, leftDir, checkDistance))
            return true;

        // 3. Справа-вперед
        Vector3 rightDir = (transform.forward + transform.right * 0.3f).normalized;
        if (Physics.Raycast(pos, rightDir, checkDistance))
            return true;

        return false;
    }

    void AvoidCollision()
    {
        // Резко поворачиваем направо или налево
        if (Random.value > 0.5f)
        {
            moveDirection = Quaternion.Euler(0, 90, 0) * moveDirection;
        }
        else
        {
            moveDirection = Quaternion.Euler(0, -90, 0) * moveDirection;
        }

        // Немного отступаем назад
        transform.position -= transform.forward * 0.1f;
    }

    void RotateChicken()
    {
        // Плавно поворачиваем к направлению движения
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }

    void MoveChicken()
    {
        // Просто двигаем вперед
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void GetNewDirection()
    {
        // Случайное направление
        moveDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;
    }

    // Рисуем лучи для отладки (видно только в редакторе)
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 pos = transform.position + Vector3.up * 0.1f;
        float checkDistance = 0.3f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + transform.forward * checkDistance);
        Gizmos.DrawLine(pos, pos + (transform.forward - transform.right * 0.3f).normalized * checkDistance);
        Gizmos.DrawLine(pos, pos + (transform.forward + transform.right * 0.3f).normalized * checkDistance);
    }
}