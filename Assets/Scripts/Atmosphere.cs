using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atmosphere : MonoBehaviour
{
    public static Atmosphere Instance;

    void Awake()
    {
        if (Atmosphere.Instance == null) {
            Atmosphere.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }
}
