using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PAC_FruitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int maxFruits = 3;
    [SerializeField] private float fruitLifetime = 8f;

    private int fruitIndex;
    private List<GameObject> activeFruits = new List<GameObject>();

    public void TrySpawnFruit()
    {
        if (activeFruits.Count >= maxFruits) return;
        if (fruitIndex >= fruitPrefabs.Length) return;

        int index = Random.Range(0, spawnPoints.Length);
        Vector3 pos = spawnPoints[index].position;
        pos.z = 0;

        GameObject fruit = Instantiate(
            fruitPrefabs[fruitIndex],
            pos,
            Quaternion.identity
        );

        activeFruits.Add(fruit);
        fruitIndex++;

        StartCoroutine(FruitTimer(fruit));
    }

    private IEnumerator FruitTimer(GameObject fruit)
    {
        yield return new WaitForSeconds(fruitLifetime);

        if (fruit)
        {
            activeFruits.Remove(fruit);
            Destroy(fruit);
        }
    }

    public void ResetFruits()
    {
        StopAllCoroutines();

        for (int i = activeFruits.Count - 1; i >= 0; i--)
        {
            if (activeFruits[i])
                Destroy(activeFruits[i]);
        }

        activeFruits.Clear();
        fruitIndex = 0;
    }
}
