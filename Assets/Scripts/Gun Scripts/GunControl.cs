using UnityEngine;

public class GunControl : MonoBehaviour {

    public int damage = 10; //damage rate for gun
    public float range = 100f; //as far as gun should shoot
    public float fireRate; //fire Rate of Gun
    public bool isAutomatic; //automatic or no?

    public Camera m_Camera; //main camera (fps)
    public ParticleSystem muzzle_Flash; //muzzleFlash
    public GameObject impactFX;
    public AudioSource gunShot;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update() {
        if (!isAutomatic)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire) //left mouse button
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire) //left mouse button
            {
                Fire();
            }
        }
    }

    public void Shoot()
    {
        muzzle_Flash.Play();
        gunShot.Play();
        RaycastHit hit;
        if(Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, range)) { //range optional

            TargetScript target = hit.transform.GetComponent<TargetScript>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impact = Instantiate(impactFX, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }

    void Fire()
    {
        nextTimeToFire = Time.time + 1f / fireRate;
        Shoot();
    }
}
