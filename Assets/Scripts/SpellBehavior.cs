using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    public float Cost = 0.0f;
    public float Delay = 0.0f;
    public float CastTime = 0.0f;
    public SpellEffectBehavior Effect;
    public List<Vector3> Targets;
    public Vector3 SelfVelocity = new Vector3();
    public Vector3 SelfShift = new Vector3();

    private bool _activated = false;
    private float _timer = 0.0f;
    private CasterBehavior _caster = null;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //The effect has activated
        if (_activated && _timer >= Delay) {
            foreach (Vector3 target in Targets) {
                //Create a spell effect at the caster's position
                //offset by the target, at the caster's rotation
                transform.localPosition += target;
                SpellEffectBehavior spellEffect = Instantiate(Effect, transform.position, _caster.transform.rotation);
                spellEffect.tag = tag;
            }
            transform.localPosition = new Vector3();
            _activated = false;
        }
        //The caster has finished casting
        if (_caster && _timer >= CastTime) {
            _caster.transform.Translate(SelfShift, Space.Self);
            _caster.EndCast();
            _caster = null;
        }

        _timer += Time.deltaTime;
    }

    public void Cast(CasterBehavior caster) {
        _activated = true;
        _caster = caster;
        _timer = 0.0f;
    }

    public void Cancel() {
        Debug.Log("Spell cancelled!");
        _activated = false;
        _caster.EndCast();
        _caster = null;
    }
}
