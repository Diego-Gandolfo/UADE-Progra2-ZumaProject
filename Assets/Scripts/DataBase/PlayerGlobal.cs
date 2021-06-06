using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : MonoBehaviour
{
    static public PlayerGlobal instance;

    public string Name { get; private set; }
    public int Id { get; private set; }

    public void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void SetName(string name)
    {
        this.Name = name;
    }

    public void SetId(int id)
    {
        this.Id = id;
    }
}
