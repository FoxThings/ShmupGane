using UnityEngine;

public enum BoundCheckActionType
{
    DeleteObject,
    KeepOnScreen,
    Nothing
}

public class BoundsCheck : MonoBehaviour
{
    public float radius = 1f;
    public BoundCheckActionType actionType = BoundCheckActionType.KeepOnScreen;

    [HideInInspector]
    public bool isOnScreen = true;

    [HideInInspector]
    public float camWidth;

    [HideInInspector]
    public float camHeight;

    
    public bool OffRight => transform.position.x > (camWidth - radius);
    public bool OffLeft => transform.position.x < (-camWidth + radius);
    public bool OffUp => transform.position.y > (camHeight - radius);
    public bool OffDown => transform.position.y < (-camHeight + radius);

    public float Height => camHeight;
    public float Width => camWidth;

    private void Awake()
    {
        var main = Camera.main;
        if (main == null) return;
        
        camHeight = main.orthographicSize;
        camWidth = camHeight * main.aspect;
    }

    private void LateUpdate()
    {
        var pos = transform.position;
        isOnScreen = true;

        if (OffRight)
        {
            pos.x = camWidth - radius;
        }
        if (OffLeft)
        {
            pos.x = -camWidth + radius;
        }
        if (OffUp)
        {
            pos.y = camHeight - radius;
        }
        if (OffDown)
        {
            pos.y = -camHeight + radius;
        }

        isOnScreen = !(OffDown || OffLeft || OffRight || OffUp);
        if (isOnScreen) return;
        
        switch (actionType)
        {
            case BoundCheckActionType.KeepOnScreen:
                transform.position = pos;
                isOnScreen = true;
                break;

            case BoundCheckActionType.DeleteObject:
                Destroy(gameObject);
                break;

            default:
            case BoundCheckActionType.Nothing:
                break;
        }

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        var boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}