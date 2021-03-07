using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehavior : MonoBehaviour
{
    public GameObject MonsterGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!MonsterGroup.GetComponentInChildren<MonsterBehavior>()) {
            Destroy(gameObject);
        }
    }
}
