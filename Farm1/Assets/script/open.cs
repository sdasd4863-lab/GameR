using UnityEngine;
using System.Collections;

public class AdvancedGateController : MonoBehaviour
{
    [Header("Настройки калитки")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float closeDelay = 2f; // Задержка перед закрытием

    [Header("Ссылки")]
    [SerializeField] private Transform gatePivot; // Опционально: отдельный объект для вращения
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Триггерная зона")]
    [SerializeField] private Collider triggerZone;
    [SerializeField] private GameObject interactionUI; // UI подсказка

    private bool isOpen = false;
    private bool isMoving = false;
    private Quaternion closedRotation;
    private AudioSource audioSource;
    private Coroutine closeCoroutine;

    void Start()
    {
        // Используем gatePivot если задан, иначе текущий объект
        Transform targetTransform = gatePivot != null ? gatePivot : transform;
        closedRotation = targetTransform.rotation;

        // Получаем или добавляем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Скрываем UI подсказку
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        // Проверка ввода, только если игрок в триггере
        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            ToggleGate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Показываем UI подсказку
            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Скрываем UI подсказку
            if (interactionUI != null)
                interactionUI.SetActive(false);

            // Автоматическое закрытие при выходе из зоны
            if (isOpen && !isMoving)
            {
                StartCloseDelay();
            }
        }
    }

    public void ToggleGate()
    {
        if (isMoving) return;

        if (!isOpen)
        {
            OpenGate();
        }
        else
        {
            CloseGate();
        }
    }

    void OpenGate()
    {
        StartCoroutine(RotateGate(true));

        // Воспроизводим звук
        if (openSound != null)
            audioSource.PlayOneShot(openSound);

        // Отменяем отложенное закрытие если оно было
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }
    }

    void CloseGate()
    {
        StartCoroutine(RotateGate(false));

        // Воспроизводим звук
        if (closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }

    IEnumerator RotateGate(bool opening)
    {
        isMoving = true;
        isOpen = opening;

        Transform targetTransform = gatePivot != null ? gatePivot : transform;
        Quaternion startRotation = targetTransform.rotation;
        Quaternion endRotation = opening
            ? closedRotation * Quaternion.Euler(0, openAngle, 0)
            : closedRotation;

        float elapsedTime = 0f;
        float duration = 1f / openSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Плавная интерполяция с ease-in-out
            t = t * t * (3f - 2f * t);

            targetTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        targetTransform.rotation = endRotation;
        isMoving = false;
    }

    void StartCloseDelay()
    {
        if (closeCoroutine != null)
            StopCoroutine(closeCoroutine);

        closeCoroutine = StartCoroutine(CloseAfterDelay());
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        if (isOpen && !isMoving)
        {
            CloseGate();
        }
    }
}