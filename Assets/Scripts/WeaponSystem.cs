using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
    public enum WeaponType { MachineGun, Shotgun, Patator }

    [Header("Réglages")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Rigidbody2D playerRb;

    [Header("Info Debug")]
    public WeaponType currentWeapon;

    private float nextFireTime = 0f;
    private PlayerController playerController; // Référence au script du joueur

    // Stats internes
    private float fireRate;
    private float recoilForce;
    private float bulletSpeed;
    private float bulletSize;
    private int bulletCount;
    private float spreadAngle;

    void Start()
    {
        if (playerRb == null) playerRb = GetComponent<Rigidbody2D>();

        // On récupère automatiquement le script PlayerController sur le même objet
        playerController = GetComponent<PlayerController>();

        StartCoroutine(SwitchWeaponRoutine());
        SetWeapon(WeaponType.MachineGun);
    }

    void Update()
    {
        // VERIFICATION DE SÉCURITÉ
        // Si le joueur défend, on INTERDIT le tir
        if (playerController != null && playerController.isDefending)
        {
            return; // On arrête tout, on ne tire pas.
        }

        // Gestion du Tir (Clic gauche maintenu)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        playerRb.AddForce(-firePoint.up * recoilForce, ForceMode2D.Impulse);

        for (int i = 0; i < bulletCount; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, randomSpread);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<Bullet>().Setup(bulletSpeed, bulletSize);
        }
    }

    IEnumerator SwitchWeaponRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            WeaponType randomType = (WeaponType)Random.Range(0, 3);
            SetWeapon(randomType);
            Debug.Log("ARME CHANGÉE : " + randomType);
        }
    }

    void SetWeapon(WeaponType type)
    {
        currentWeapon = type;
        switch (type)
        {
            case WeaponType.MachineGun:
                fireRate = 0.1f; recoilForce = 0.2f; bulletSpeed = 15f; bulletSize = 0.3f; bulletCount = 1; spreadAngle = 5f;
                break;
            case WeaponType.Shotgun:
                fireRate = 0.8f; recoilForce = 2f; bulletSpeed = 12f; bulletSize = 0.4f; bulletCount = 4; spreadAngle = 20f;
                break;
            case WeaponType.Patator:
                fireRate = 1.2f; recoilForce = 5f; bulletSpeed = 8f; bulletSize = 1.2f; bulletCount = 1; spreadAngle = 0f;
                break;
        }
    }
}