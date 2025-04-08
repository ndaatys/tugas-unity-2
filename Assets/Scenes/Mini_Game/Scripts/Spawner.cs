using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mini_Game.Scripts
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] GameObject entity;
        [SerializeField] float spawnAreaRange = 1;
        [CustomInspector.AsRange(0.0001f, 100)]
        [SerializeField] Vector2 spawnRate;
        [SerializeField] int maxPool = 100;
        [SerializeField] float entityMoveSpeed = 1;

        List<GameObject> pools;

        IEnumerator Engine() 
        {
            pools = new();
            yield return new WaitForSeconds(1/Random.Range(spawnRate.x, spawnRate.y));
            while (true)
            {
                var instance = Instantiate(entity, transform.position + Vector3.right * Random.Range(-spawnAreaRange/2, spawnAreaRange/2), transform.rotation, transform);
                if (pools.Count >= maxPool)
                {
                    var item = pools[0];
                    pools.RemoveAt(0);
                    Destroy(item);
                }
                pools.Add(instance);
                var delay = 1/Random.Range(spawnRate.x, spawnRate.y);
                yield return new WaitForSeconds(delay);
            }
        }

        void OnEnable()
        {
            StartCoroutine(Engine());
        }

        void Update()
        {
            var delta = Time.deltaTime;
            for (int i = 0; i < pools.Count; i++)
            {
                var item = pools[i];
                item.transform.localPosition += Vector3.up * entityMoveSpeed * delta;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Vector3.zero, new Vector3(spawnAreaRange, 0.3f, 0));
            Gizmos.DrawCube(Vector3.right * spawnAreaRange/2 - Vector3.right * 0.15f + Vector3.up * 0.3f, new Vector3(0.3f, 0.3f, 0));
            Gizmos.DrawCube(Vector3.left * spawnAreaRange/2 - Vector3.left * 0.15f + Vector3.up * 0.3f, new Vector3(0.3f, 0.3f, 0));
        }
    }
}