using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCastBehavior : MonoBehaviour
{
    public SpellBehavior Spell1;
    public SpellBehavior Spell2;
    public SpellBehavior Spell3;

    private NavMeshAgent _agent;
    private float _agentSpeed;

    // Start is called before the first frame update
    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agentSpeed = _agent.speed;
    }

    // Update is called once per frame
    void Update() {
        if (Spell1 && Input.GetButtonDown("Fire1") && _agent.speed != 0.0f) {
            StartCast(Spell1);
        }
    }

    public void StartCast(SpellBehavior spell) {
        _agentSpeed = _agent.speed;
        _agent.speed = 0.0f;
        spell.Cast(this);
    }

    public void EndCast(SpellBehavior spell) {
        _agent.speed = _agentSpeed;
    }
}
