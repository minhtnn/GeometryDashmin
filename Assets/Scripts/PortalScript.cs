using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PortalScript : MonoBehaviour
    {
        public Gamemodes Gamemode;
        public Speeds Speed;
        public bool gravity;
        public int State;

        public void initiatePortal(PlayerMove movement)
        {
            movement.ChangeThroughPortal(Gamemode, Speed, gravity ? 1 : -1, State);
        }
    }
}
