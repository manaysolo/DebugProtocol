using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Vitesse par défaut, sera écrasée par le WeaponSystem

    public void Setup(float newSpeed, float newSize)
    {
        speed = newSpeed;
        transform.localScale = Vector3.one * newSize;
        Destroy(gameObject, 3f); // Autodestruction après 3s
    }

    void Update()
    {
        // Avance vers le HAUT local (là où la balle regarde)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si l'objet touché a le Tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // <-- TUE L'ENNEMI INSTANTANÉMENT
            Destroy(gameObject);       // <-- DÉTRUIT LA BALLE
        }
    }
}