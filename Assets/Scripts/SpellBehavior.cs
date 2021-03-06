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
    public Vector3 SelfShift = new Vector3(0.0f, 0.0f, 0.0f);

    private bool _activated = false;
    private float _timer = 0.0f;
    private CasterBehavior _caster = null;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        _timer += Time.deltaTime;

        //The effect has activated
        if (_activated && _timer >= Delay) {
            foreach (Vector3 target in Targets) {
                //Create a spell effect at the caster's position
                //offset by the target, at the caster's rotation
                transform.localPosition += target;
                Instantiate(Effect, transform.position, _caster.transform.rotation);
            }
            transform.localPosition = new Vector3();
            _activated = false;
        }
        //The caster has finished casting
        if (_caster && _timer >= CastTime) {
            _caster.transform.Translate(SelfShift, Space.Self);
            _caster.EndCast(this);
            _caster = null;
        }
    }

    public void Cast(CasterBehavior caster) {
        _activated = true;
        _caster = caster;
        _timer = 0.0f;
    }
}
