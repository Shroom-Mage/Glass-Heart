using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CasterBehavior : MonoBehaviour
{
    public float Health = 100.0f;
    public float Mana = 100.0f;

    private float _maxHealth;
    private float _maxMana;
    private float _stun;

    private SpellBehavior _currentSpell;

    private NavMeshAgent _agent;
    private float _agentSpeed;

    // Start is called before the first frame update
    private void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agentSpeed = _agent.speed;
        _maxHealth = Health;
        _maxMana = Mana;
    }

    private void Update() {
        _stun -= Time.deltaTime;
        Mathf.Min(Mana + Time.deltaTime, _maxMana);
    }

    public void StartCast(SpellBehavior spell) {
        Mana -= spell.Cost;
        _agentSpeed = _agent.speed;
        _agent.speed = 0.0f;
        _agent.velocity = transform.forward * spell.SelfVelocity;
        spell.tag = tag;
        spell.Cast(this);
        _currentSpell = spell;
    }

    public void EndCast() {
        _agent.speed = _agentSpeed;
        _currentSpell = null;
    }

    public bool IsCasting() {
        return _agent.speed == 0.0f;
    }

    public void TakeDamage(float damage, float stun, Vector3 knockback) {
        Debug.Log(name + " took " + damage);
        // Apply effects
        Health -= damage;
        _stun = stun;
        transform.Translate(knockback, Space.World);
        // Cancel current spell
        if (_currentSpell) {
            _currentSpell.Cancel();
            _currentSpell = null;
        }
    }

    public bool IsStunned() {
        return _stun > 0.0f;
    }

    public bool IsAlive() {
        return Health > 0.0f;
    }
}
