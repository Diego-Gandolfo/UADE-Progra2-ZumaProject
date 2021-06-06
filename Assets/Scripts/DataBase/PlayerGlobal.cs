using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : MonoBehaviour
{
    static public PlayerGlobal instance;

    public string Name { get; set; }
    public int Id { get; set; }
    public int Level { get; set; }
    public int Score { get; set; }
    public float Time { get; set; }

    public void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
