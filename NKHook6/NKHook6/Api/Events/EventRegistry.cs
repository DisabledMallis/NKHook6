﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace NKHook6.Api.Events
{
    public class EventRegistry
    {
        public static EventRegistry subscriber;
        internal EventRegistry()
        {
            subscriber = this;

            createEvent("UpdateEvent");
            createEvent("KeyPressEvent");
            createEvent("KeyHeldEvent");
            createEvent("KeyReleaseEvent");


            string preName = ".Pre";
            string postName = ".Post";
            List<string> CustomEvents = new List<string>()
            {
                "MainMenu.OnEnableEvent",
                "Bloon.InitialiseEvent",
                "Bloon.DamagedEvent",
                "Bloon.LeakedEvent",
                "Bloon.OnDestroyEvent",
                "Bloon.SetRotationEvent",
                "Bloon.UpdatedModelEvent",

                "Tower.InitialiseEvent",
                "Tower.OnDestroyEvent",
                "Tower.OnSoldEvent",
                "Tower.UpdatedModelEvent",
                "Tower.IsSelectableEvent",
                "Tower.IsUpgradeBlockedEvent",
                "Tower.AddPoppedCashEvent",
                "Tower.GetSaveDataEvent",
                

                "InGame.UpdateEvent",
                "Simulation.OnRoundStartEvent",
                "Simulation.OnRoundEndEvent",

                //autogenerated
                "Tower.HilightEvent",
                "Tower.OnUpgradeEvent",
                "Tower.UnHighlightEvent",

                "Weapon.InitialiseEvent",
                "Weapon.OnDestroyEvent",
                "Weapon.UpdatedModelEvent",
                
                "Simulation.OnDefeatEvent",
                "Simulation.TakeDamageEvent",

                "TimeManager.SetFastForwardEvent",
                "InGame.GetContinueCostEvent",
                "InGame.OnVictoryEvent",



                "Projectile.InitialiseEvent",
                "Projectile.OnDestroyEvent",
                "Projectile.UpdatedModelEvent",

                "Simulation.SetCashEvent",
                "Simulation.AddCashEvent",
                "Simulation.RemoveCashEvent"
            };

            foreach (var item in CustomEvents)
            {
                if (theRegistry.ContainsKey(item + preName) && theRegistry.ContainsKey(item + postName))
                    continue;

                createEvent(item + preName);
                createEvent(item + postName);
            }
        }

        void createEvent(string eventName)
        {
            //Logger.Log("Created event: " + eventName);
            theRegistry.Add(eventName, new List<MethodInfo>());
        }

        /// <summary>
        /// Dictionary of eventNames with their callbacks
        /// </summary>
        Dictionary<string, List<MethodInfo>> theRegistry = new Dictionary<string, List<MethodInfo>>();

        public void register(Type toSubscribe)
        {
            foreach(MethodInfo method in toSubscribe.GetMethods())
            {
                if (method.IsStatic)
                {
                    foreach (Attribute attrib in method.GetCustomAttributes())
                    {
                        if(attrib is EventAttribute)
                        {
                            bool registered = false;
                            EventAttribute eventAttrib = (EventAttribute)attrib;
                            foreach(string currentEventName in theRegistry.Keys)
                            {
                                if (currentEventName == eventAttrib.eventName)
                                {
                                    theRegistry[currentEventName].Add(method);
                                    //Logger.Log("Registered event \"" + eventAttrib.eventName + "\"");
                                    registered = true;
                                    continue;
                                }
                            }
                            if(!registered)
                                Logger.Log("Unknown event \"" + eventAttrib.eventName + "\"");
                        }
                    }
                }
            }
        }
        public void unregister(Type toUnSubscribe)
        {

        }
        public void dispatchEvent<T>(ref T e) where T : EventBase
        {
            foreach (string name in theRegistry.Keys)
            {
                List<MethodInfo> callbacks = theRegistry[name];
                if (callbacks == null)
                    continue;
                if (callbacks.Count == 0)
                    continue;
                foreach(MethodInfo callback in callbacks)
                {
                    foreach (Attribute attrib in callback.GetCustomAttributes())
                    {
                        if (attrib is EventAttribute)
                        {
                            EventAttribute eventAttrib = (EventAttribute)attrib;
                            if (eventAttrib.eventName == e.eventName)
                            {
                                callback.Invoke(null, new object[] { e });
                            }
                        }
                    }
                }
            }
        }
    }
}
