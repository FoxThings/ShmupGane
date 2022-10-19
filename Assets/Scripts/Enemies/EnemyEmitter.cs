using UnityEditor;
using UnityEngine;

public class EnemyEmitter : MonoBehaviour
{
    public float Radius = 1f;

    private float spawnRange;
    private Vector2 spawnPoint;
    
    private void Start()
    {
        var main = Camera.main;
        if (main == null) return;
        var orthographicSize = main.orthographicSize;

        spawnPoint = main.transform.position;
        spawnPoint.y += orthographicSize;
            
        spawnRange = (orthographicSize * main.aspect);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            spawnPoint + new Vector2(-spawnRange, Radius), 
            spawnPoint + new Vector2(spawnRange, Radius)
            );
    }

    public GameObject Emit(GameObject enemy)
    {
        var point = spawnPoint + new Vector2(Random.Range(-spawnRange + Radius, spawnRange - Radius), Radius);
        return Instantiate(enemy, point, Quaternion.identity);
    }
    
}
