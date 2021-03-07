using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CasterBehavior))]
public class MonsterBehavior : MonoBehaviour
{
    public enum State {
        Idling,
        Wandering,
        Approaching,
        Attacking,
        Stunned,
        Dead
    }

    public float IdleTimer = 1.0f;
    public float AggroRadius = 5.0f;
    public float AttackRadius = 2.5f;

    public SpellBehavior Spell;

    private State _state = State.Idling;
    private float _timer = 0.0f;
    private Vector3 _wanderDirection = new Vector3();

    private NavMeshAgent _agent;
    private CasterBehavior _caster;
    private PlayerBehavior _player;

    // Start is called before the first frame update
    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _caster = GetComponent<CasterBehavior>();
        _player = FindObjectOfType<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update() {
        switch (_state) {
            case State.Idling:
                Idle();
                break;
            case State.Wandering:
                Wander();
                break;
            case State.Approaching:
                Approach();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Stunned:
                Stunned();
                break;
            case State.Dead:
                Dead();
                break;
        }
        _timer += Time.deltaTime;
    }

    public float DistanceToPlayer() {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void EnterIdle() {
        // Stop
        _agent.destination = transform.position;
        Debug.Log("Idling");
        _timer = 0.0f;
        _state = State.Idling;
    }

    private void Idle() {

        // Check state change
        if (!_caster.IsAlive()) {
            EnterDead();
        } else if (_caster.IsStunned()) {
            EnterStunned();
        } else if (DistanceToPlayer() <= AggroRadius) {
            EnterApproach();
        } else if (_timer >= IdleTimer) {
            EnterWander();
        }
    }

    private void EnterWander() {
        Debug.Log("Wandering");
        _wanderDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        _timer = 0.0f;
        _state = State.Wandering;
    }

    private void Wander() {
        // Move
        _agent.destination = transform.position + _wanderDirection;

        // Check state change
        if (!_caster.IsAlive()) {
            EnterDead();
        } else if (_caster.IsStunned()) {
            EnterStunned();
        } else if (DistanceToPlayer() <= AggroRadius) {
            EnterApproach();
        } else if (_timer >= IdleTimer / 2) {
            EnterIdle();
        }
    }

    private void EnterApproach() {
        Debug.Log("Approaching");
        _state = State.Approaching;
    }

    private void Approach() {
        // Move
        _agent.destination = _player.transform.position;

        // Check state change
        if (!_caster.IsAlive()) {
            EnterDead();
        } else if (_caster.IsStunned()) {
            EnterStunned();
        } else if (DistanceToPlayer() <= AttackRadius) {
            EnterAttack();
        } else if (DistanceToPlayer() >= AggroRadius * 2.0f) {
            EnterIdle();
        }
    }

    private void EnterAttack() {
        Debug.Log("Attacking");
        _agent.destination = transform.position;
        _state = State.Attacking;
    }

    private void Attack() {
        // Check state change
        if (!_caster.IsAlive()) {
            EnterDead();
        } else if (_caster.IsStunned()) {
            EnterStunned();
        } else if (DistanceToPlayer() >= AttackRadius) {
            EnterApproach();
        } else if (!_caster.IsCasting()) {
            transform.LookAt(_player.transform);
            _caster.StartCast(Spell);
        }
    }

    private void EnterStunned() {
        // Stop
        _agent.destination = transform.position;
        Debug.Log("Stunned!");
        _state = State.Stunned;
    }

    private void Stunned() {
        if (!_caster.IsAlive()) {
            EnterDead();
        } else if (!_caster.IsStunned()) {
            EnterApproach();
        }
    }

    private void EnterDead() {
        // Stop
        _agent.destination = transform.position;
        Debug.Log("Dead!");
        _state = State.Dead;
    }

    private void Dead() {
        Destroy(gameObject);
    }
}
