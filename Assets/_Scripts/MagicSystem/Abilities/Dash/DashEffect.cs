using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Player.Instance.transform.position + new Vector3(0, 0.3f, 0);
    }
}
