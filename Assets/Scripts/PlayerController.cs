using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float moveSpeed = 10f;

    [Header("Santé")]
    public int maxHealth = 3;
    public Image healthBarFill;

    // Cette variable est publique pour que le WeaponSystem puisse la lire
    [HideInInspector] public bool isDefending = false;

    private int currentHealth;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera cam;
    private SpriteRenderer spriteRenderer; // Pour changer la couleur

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Récupère le visuel du joueur
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 1. GESTION DU MODE DÉFENSE (Clic Droit maintenu)
        if (Input.GetMouseButton(1))
        {
            isDefending = true;
            spriteRenderer.color = Color.cyan; // Devient Bleu Cyan (Bouclier)
        }
        else
        {
            isDefending = false;
            spriteRenderer.color = Color.white; // Revient à la normale
        }

        // 2. Mouvements habituels
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // 3. Rotation
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void FixedUpdate()
    {
        // On peut toujours bouger, même en défendant
        rb.AddForce(movement * moveSpeed);
    }

    public void TakeDamage(int damage)
    {
        // SI ON DÉFEND : ON NE PREND PAS DE DÉGÂTS !
        if (isDefending)
        {
            return; // On sort de la fonction immédiatement
        }

        currentHealth -= damage;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}