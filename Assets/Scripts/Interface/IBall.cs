using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public interface IBall
    {
        BallShowQueue BallSQ { get; }

        GameObject GetGameObject();
    }
}
