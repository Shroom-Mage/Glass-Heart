using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpellEffectBehavior : MonoBehaviour
{
    public float Damage = 0.0f;
    public float Stun = 0.0f;
    public float Knockback = 0.0f;
    public float Duration = 1.0f;
    public bool EndOnCollision = false;
    public Vector3 Velocity = new Vector3();

    private float _timer = 0.0f;
    private Collider _collider;

    // Start is called before the first frame update
    void Start() {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update() {
        // Move forward
        transform.Translate(Velocity * Time.deltaTime, Space.Self);

        // Destroy self if duration has expired
        if (_timer >= Duration) {
            // Play particle effect here
            Destroy(gameObject);
        }
        _timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != tag) {
            Debug.Log("Hit");
            CasterBehavior target = other.GetComponentInParent<CasterBehavior>();
            if (target) {
                // Damage the target
                target.TakeDamage(Damage, Stun, transform.forward * Knockback);
                // Destroy self if needed
                if (EndOnCollision) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
