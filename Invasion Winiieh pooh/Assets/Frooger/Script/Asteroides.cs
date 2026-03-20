using UnityEngine;
using System.Collections;

public class Asteroides : MonoBehaviour
{
    [SerializeField] private GameObject asteroid;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private float maximumScale = 10f;
    [SerializeField] private float minimumScale = 5f;

    private Vector3 screenCenter;

    private float minimumZ;
    private float maximumZ;
    private float minimumX;
    private float maximumX;

    private void Start()
    {
        var mainCamera = Camera.main;
        var camPos = mainCamera.transform.position;

        screenCenter = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width / 2, Screen.height / 2, -camPos.y));

        minimumZ = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -camPos.y)).z;
        maximumZ = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, -camPos.y)).z;

        minimumX = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -camPos.y)).x;
        maximumX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, -camPos.y)).x;

        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            InstantiateRandomAsteroid();
        }
    }

    private void InstantiateRandomAsteroid()
    {
        bool asteroidsOverlap;

        float spawnX = 0;
        float spawnZ = 0;

        var scale = UnityEngine.Random.Range(minimumScale, maximumScale);

        do
        {
            var randomValue = UnityEngine.Random.value;

            if (randomValue > 0.75f)
            {
                spawnX = UnityEngine.Random.Range(minimumX - maximumScale - scale, minimumX - minimumScale - scale);
                spawnZ = UnityEngine.Random.Range(minimumZ, maximumZ);
            }
            else if (randomValue > 0.5f)
            {
                spawnX = UnityEngine.Random.Range(maximumX + minimumScale + scale, maximumX + maximumScale + scale);
                spawnZ = UnityEngine.Random.Range(minimumZ, maximumZ);
            }
            else if (randomValue > 0.25f)
            {
                spawnX = UnityEngine.Random.Range(minimumX, maximumX);
                spawnZ = UnityEngine.Random.Range(minimumZ - maximumScale - scale, minimumZ - minimumScale - scale);
            }
            else
            {
                spawnX = UnityEngine.Random.Range(minimumX, maximumX);
                spawnZ = UnityEngine.Random.Range(maximumZ + minimumScale + scale, maximumZ + maximumScale + scale);
            }

            var collidersBuffer = new Collider[16];
            var size = Physics.OverlapBoxNonAlloc(
                new Vector3(spawnX, 0, spawnZ), new Vector3(1, 1, 1), collidersBuffer);

            asteroidsOverlap = size > 0;

        } while (asteroidsOverlap);

        var asteroidObject = Instantiate(asteroid, new Vector3(spawnX, 0, spawnZ),Quaternion.identity);

        asteroidObject.transform.LookAt(new Vector3(screenCenter.x, 0, screenCenter.z));
        asteroidObject.transform.localScale = new Vector3(scale, scale, scale);
    }
}

