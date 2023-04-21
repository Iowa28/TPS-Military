using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RifleController : MonoBehaviour
{
    [Header("Common")] 
    
    [SerializeField] 
    private Camera camera;

    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private float shootingRange = 100f;

    [SerializeField]
    private float fireCharge = 15f;
    
    private Animator animator;

    private PlayerController playerController;

    [Header("Ammunition and shooting")]
    
    [SerializeField]
    private float reloadingTime = 1.3f;

    [SerializeField]
    private int maxAmmunition = 20;

    [SerializeField]
    private int magazines = 15;

    private int currentAmmunition;

    private bool isReloading;

    private float nextTimeToShoot;
    
    private int reloadingHash;

    private bool isShooting;

    private bool isAiming;

    [Header("Effects")]
    
    [SerializeField]
    private ParticleSystem muzzleSpark;

    [SerializeField]
    private GameObject impactEffect;

    private void Awake()
    {
        currentAmmunition = maxAmmunition;

        animator = GetComponentInParent<Animator>();
        playerController = GetComponentInParent<PlayerController>();

        reloadingHash = Animator.StringToHash("Is Reloading");
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (currentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (isShooting && isAiming && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (magazines == 0)
            return;
        
        currentAmmunition--;
        
        if (currentAmmunition == 0)
        {
            magazines--;
        }

        muzzleSpark.Play();
        
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, shootingRange))
        {
            ObjectController hitObject = hit.transform.GetComponent<ObjectController>();
            if (hitObject != null)
            {
                hitObject.takeDamage(damage);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            const float delay = 1f;
            Destroy(impact, delay);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        playerController.SetStopped(true);
        animator.SetBool(reloadingHash, true);

        yield return new WaitForSeconds(reloadingTime);
        
        animator.SetBool(reloadingHash, false);
        currentAmmunition = maxAmmunition;
        playerController.SetStopped(false);
        isReloading = false;
    }

    public void OnReload(InputValue value)
    {
        if (currentAmmunition < maxAmmunition)
        {
            StartCoroutine(Reload());
        }
    }
    
    public void OnFire(InputValue value) => isShooting = value.isPressed;
    
    public void OnAim(InputValue value) => isAiming = value.isPressed;
}
