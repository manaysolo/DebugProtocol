using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;

    void Start()
    {
        // Trouve le joueur automatiquement s'il a le Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Avance vers la position du joueur
            // MoveTowards est parfait pour ça (position actuelle, cible, vitesse * temps)
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Optionnel : Regarder le joueur
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Gestion des collisions (Mort de l'ennemi ou du joueur)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si l'ennemi touche le joueur
        if (other.CompareTag("Player"))
        {
            // Récupère le script du joueur et lui inflige 1 dégât
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
            }

            // L'ennemi meurt après avoir touché (Kamikaze)
            Destroy(gameObject);
        }
    }
}