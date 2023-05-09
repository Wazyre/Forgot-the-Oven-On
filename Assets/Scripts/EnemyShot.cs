using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [Header("Shot Locations")]
    [SerializeField] float power = 30f;
    [SerializeField] LineRenderer lr;
    [SerializeField] Vector3 landingPosition;

    void Start() {
        lr = GetComponent<LineRenderer>();
    }

    void Fire() {
        // GameObject projectile = Instantiate(projectilePrefab);
        // //Physics.IgnoreCollision(projectile.GetComponent<Collider>(), projectileSpawn.parent.GetComponent<Collider>());
        // projectile.transform.position = transform.position;
        // Vector3 rotation = projectile.transform.rotation.eulerAngles;
        // projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        // projectile.GetComponent<Rigidbody>().AddForce(projectileSpawn.forward * projectileSpeed, ForceMode.Impulse);
        Ray ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;
        
        line.SetPosition(0, ray.origin);
        
        Physics.Raycast(ray, hit, Mathf.Infinity);
        line.SetPosition(1, hit.point);

        Vector2 direction = (target.position - hit.point).normalized; 
        rigidigidy.AddForce(direction * power, ForceMode.Impulse);
    }
}