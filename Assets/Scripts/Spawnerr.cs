using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Réglages")]
    public GameObject enemyPrefab;
    public float spawnInterval = 2f; // Temps entre chaque ennemi au début

    // Limites de l'écran (à ajuster selon ta caméra, ici environ 10 unités)
    private float xLimit = 10f;
    private float yLimit = 6f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // Boucle infinie
        {
            SpawnEnemy();

            // Attendre avant le prochain spawn
            yield return new WaitForSeconds(spawnInterval);

            // Augmenter la difficulté : on réduit le temps d'attente petit à petit
            if (spawnInterval > 0.5f)
            {
                spawnInterval -= 0.05f;
            }
        }
    }

    void SpawnEnemy()
    {
        // Choisir un côté au hasard (0=Haut, 1=Bas, 2=Gauche, 3=Droite)
        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: spawnPos = new Vector2(Random.Range(-xLimit, xLimit), yLimit); break; // Haut
            case 1: spawnPos = new Vector2(Random.Range(-xLimit, xLimit), -yLimit); break; // Bas
            case 2: spawnPos = new Vector2(-xLimit, Random.Range(-yLimit, yLimit)); break; // Gauche
            case 3: spawnPos = new Vector2(xLimit, Random.Range(-yLimit, yLimit)); break; // Droite
        }

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}