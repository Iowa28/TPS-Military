using System.Collections;
using UnityEngine;

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

    private CameraSwitcher cameraSwitcher;

    [Header("Ammunition and shooting")]
    
    [SerializeField]
    private float reloadingTime = 1.3f;

    [SerializeField]
    private int maxAmmunition = 20;

    [SerializeField]
    private int magazines = 15;

    private int currentAmmunition;

    private bool reloading;

    private float nextTimeToShoot;

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
        cameraSwitcher = GetComponentInParent<CameraSwitcher>();
    }

    private void Update()
    {
        if (reloading)
            return;

        if ((Input.GetKey(KeyCode.R) && currentAmmunition < maxAmmunition) || currentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        
        if (Input.GetButton("Fire1") && cameraSwitcher.IsAiming() && Time.time >= nextTimeToShoot)
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
        reloading = true;
        playerController.SetStopped(true);
        animator.SetBool("Is Reloading", true);

        yield return new WaitForSeconds(reloadingTime);
        
        animator.SetBool("Is Reloading", false);
        currentAmmunition = maxAmmunition;
        playerController.SetStopped(false);
        reloading = false;
    }
}
