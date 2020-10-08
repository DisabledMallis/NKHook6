﻿using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Backend;
using MelonLoader;
using NKHook6.Api.Events;
using NKHook6.Api.Events._Bloons;
using NKHook6.Api.Events._MainMenu;
using NKHook6.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHook6.Examples.LHCM
{
    public class Entry : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            EventRegistry.subscriber.register(typeof(Entry));
        }

        [EventAttribute("BloonLeakedEvent")]
        public static void onLeaked(ref BloonEvents.LeakedEvent e)
        {
            float damage = e.bloon.getDamage();
            InGame inst = InGame.instance;
            if (inst != null)
            {
                double cash = inst.getCash();
                inst.setCash(cash - damage);
                cash = inst.getCash();
                if(cash > 0)
                {
                    inst.setHealth(inst.getHealth() + damage);
                }
                else
                {
                    inst.setHealth(0);
                }
            }
            return;
        }
        
        [EventAttribute("BloonCreatedEvent")]
        public static void onBloonInit(BloonEvents.CreatedEvent e)
        {
            e.bloon.setCamo(true);
        }
    }
}
