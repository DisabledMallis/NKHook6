﻿using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Objects;
using NKHook6.Api.Gamemodes;

namespace NKHook6.Api.Events._Bloons
{
    public partial class BloonEvents
    {
        public class CreatedEvent : EventBase
        {
            public Bloon bloon;
            public Entity entity { get; set; }
            public Model model { get; set; }

            public CreatedEvent(ref Bloon bloon, ref Entity target, ref Model model) : base("BloonCreatedEvent")
            {
                //Loader.Start();
                this.bloon = bloon;
                this.model = model;
                this.entity = target;
            }
        }
    }
}
