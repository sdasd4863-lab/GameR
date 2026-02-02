using UnityEngine;

public class WallFix : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"Проверяю стену: {gameObject.name}");

        // 1. Проверяем есть ли коллайдер
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning($"У стены {gameObject.name} нет коллайдера! Добавляю...");
            col = gameObject.AddComponent<BoxCollider>();
        }

        // 2. Убеждаемся что это НЕ триггер
        if (col.isTrigger)
        {
            Debug.LogWarning($"Коллайдер стены {gameObject.name} - триггер! Исправляю...");
            col.isTrigger = false;
        }

        // 3. Добавляем тег если нужно
        if (gameObject.tag == "Untagged")
        {
            gameObject.tag = "Wall";
            Debug.Log($"Добавил тег Wall на {gameObject.name}");
        }

        Debug.Log($"Стена {gameObject.name} готова. Коллайдер: {col.GetType().Name}, IsTrigger: {col.isTrigger}");
    }
}