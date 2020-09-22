﻿using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Unity;
using NKHook6.Api.Events.Bloons;
using NKHook6.Api.Enums;
using System;
using Boo.Lang;
using Assets.Scripts.Simulation.Bloons;

namespace NKHook6.Api.Utilities
{
    public class BloonUtils
    {
        public static List<Bloon> BloonsOnMap = new List<Bloon>();


        public static BloonModel GetBloon(DefaultBloonIds bloonId) => GetBloon(bloonId.ToString());

        public static BloonModel GetBloon(string bloonId)
        {
            BloonModel result = null;

            if (Game.instance == null)
            {
                Logger.Log("Can't get BloonModel for BloonId: \"" + bloonId + "\". The Game instance is null");
                return result;
            }
            else if (Game.instance.model == null)
            {
                Logger.Log("Can't get BloonModel for BloonId: \"" + bloonId + "\". The Game instance model is null");
                return result;
            }

            try
            {
                result = Game.instance.model.GetBloon(bloonId);
            }
            catch (Exception e)
            {
                Logger.Log("Exception occured when trying to use GetBloon from NKHook6." +
                    " Tried Getting a bloon with this non-existant BloonId: \"" + bloonId + "\"." +
                    "\nMore exception details: " + e.Message);
            }

            return result;
        }
    }
}
