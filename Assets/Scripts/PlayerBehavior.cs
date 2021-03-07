using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CasterBehavior))]
public class PlayerBehavior : MonoBehaviour
{
    public SpellBehavior BasicAttack;
    public SpellBehavior Spell1;
    public SpellBehavior Spell2;
    public SpellBehavior Spell3;

    public GameObject WitchForm;
    public GameObject BreakForm;

    public bool Broken = false;
    private float _unbrokenHealth = 100.0f;

    public Camera FollowCamera;
    private Vector3 _cameraOffset;

    private NavMeshAgent _agent;
    private CasterBehavior _caster;
    private Animator[] _animators;

    // Start is called before the first frame update
    void Start() {
        if (FollowCamera) {
            _cameraOffset = FollowCamera.transform.position;
        }
        _agent = GetComponent<NavMeshAgent>();
        _caster = GetComponent<CasterBehavior>();
        _animators = GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        // Move the camera
        if (FollowCamera) {
            FollowCamera.transform.position = transform.position + _cameraOffset;
        }

        // Find the direction
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        // Move
        _agent.destination = transform.position + direction;

        // Hard set the facing
        //HardFace(direction);

        // Cast spell
        if (BasicAttack && Input.GetButtonDown("Jump") && !_caster.IsCasting() && !BasicAttack.OnCooldown()) {
            _caster.StartCast(BasicAttack);
            HardFace(direction);
        } else if (Spell1 && Input.GetButtonDown("Fire1") && !_caster.IsCasting() && !Spell1.OnCooldown()) {
            if (_caster.Mana <= 0) {
                Break();
            }
            _caster.StartCast(Spell1);
            HardFace(direction);
        } else if (Spell2 && Input.GetButtonDown("Fire2") && !_caster.IsCasting() && !Spell2.OnCooldown()) {
            if (_caster.Mana <= 0) {
                Break();
            }
            _caster.StartCast(Spell2);
            HardFace(direction);
        } else if (Spell3 && Input.GetButtonDown("Fire3") && !_caster.IsCasting() && !Spell3.OnCooldown()) {
            if (_caster.Mana <= 0) {
                Break();
            }
            _caster.StartCast(Spell3);
            HardFace(direction);
        }

        //Animations
        foreach (Animator animator in _animators) {
            animator.SetFloat("Motion", _agent.velocity.magnitude / _agent.speed);
            animator.SetBool("Attacking", _caster.IsCasting());
            animator.SetBool("Broken", Broken);
        }
    }

    public void HardFace(Vector3 direction) {
        if (direction.magnitude > 0)
            transform.forward = direction;
    }

    public void Break() {
        Debug.Log("BREAK!");
        Broken = true;
        _unbrokenHealth = _caster.Health;
        _caster.Health = 1.0f;
        WitchForm.SetActive(false);
        BreakForm.SetActive(true);
    }

    public void Unbreak() {
        Debug.Log("Unbreak");
        Broken = false;
        _caster.Health = _unbrokenHealth;
        WitchForm.SetActive(true);
        BreakForm.SetActive(false);
    }
}
