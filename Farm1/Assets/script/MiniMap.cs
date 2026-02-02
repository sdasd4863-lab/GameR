using UnityEngine;
using UnityEngine.UI;

public class FullMiniMap : MonoBehaviour
{
    [Header("Основные настройки")]
    public Transform player;
    public float mapHeight = 100f;
    public Camera miniMapCamera;
    public RawImage miniMapImage; // ← СЮДА ПЕРЕТАЩИ UI Image

    [Header("Масштабирование")]
    public float zoomSpeed = 10f;
    public float minHeight = 30f;
    public float maxHeight = 200f;

    void Start()
    {
        if (miniMapCamera == null)
            miniMapCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Следим за игроком
            Vector3 newPos = player.position;
            newPos.y = player.position.y + mapHeight;
            transform.position = newPos;

            // Управление зумом колесиком мыши
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            mapHeight -= scroll * zoomSpeed;
            mapHeight = Mathf.Clamp(mapHeight, minHeight, maxHeight);
        }
    }

    // Опционально: Переключение между круглой и квадратной картой
    public void ToggleCircleMap(bool isCircle)
    {
        if (miniMapImage != null)
        {
            miniMapImage.maskable = isCircle;
            if (isCircle)
                miniMapImage.GetComponent<Mask>().enabled = true;
        }
    }
}