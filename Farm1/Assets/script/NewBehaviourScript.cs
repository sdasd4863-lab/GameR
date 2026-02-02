using UnityEngine;

public class WindmillBlades : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float rotationSpeed = 100f; // Скорость вращения
    [SerializeField] private Vector3 rotationAxis = Vector3.forward; // Ось вращения

    void Update()
    {
        // Вращаем лопасти с постоянной скоростью
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}