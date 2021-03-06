﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace NKHook6.Api.Events
{
    public class EventRegistry : Registry<List<MethodInfo>>
    {
        //Compat for old mods
        public static EventRegistry subscriber
        {
            get
            {
                return instance;
            }
        }
        public static EventRegistry instance;
        internal EventRegistry() : base()
        {
            instance = this;

            //Unity
            createEvent("UpdateEvent"); //Updated
            createEvent("KeyPressEvent"); //Updated
            createEvent("KeyHeldEvent"); //Updated
            createEvent("KeyReleaseEvent"); //Updated

            //Game
            createEvent("MainMenuLoadedEvent"); //Updated

            //Bloons
            createEvent("BloonCreatedEvent"); //Updated
            createEvent("BloonDamagedEvent"); //Updated
            createEvent("BloonLeakedEvent"); //Updated
            createEvent("BloonDeletedEvent"); //Updated
            //createEvent("BloonMoveEvent"); //Not possible yet, gotta do more investigation
            createEvent("BloonRotateEvent"); //Updated
            //createEvent("BloonModelChangedEvent"); //Kinda useless

            //Towers
            createEvent("TowerCreatedEvent"); //Updated
            createEvent("TowerDeletedEvent"); //Updated
            createEvent("TowerSoldEvent"); //Updated
            //createEvent("TowerModelChangedEvent"); //Kinda useless
            createEvent("TowerSelectedEvent"); //Updated
            createEvent("TowerDeselectedEvent"); //Updated
            createEvent("TowerUpgradeEvent"); //Updated

            //Simulation
            createEvent("RoundStartEvent"); //Updated
            createEvent("RoundEndEvent"); //Updated
            createEvent("DefeatedEvent"); //Updated
            createEvent("CashChangedEvent"); //Updated
            createEvent("CashLostEvent"); //Updated
            createEvent("CashGainedEvent"); //Updated

            //Weapons
            createEvent("WeaponCreatedEvent"); //Updated
            createEvent("WeaponDeletedEvent"); //Updated
            //createEvent("WeaponModelChangedEvent"); //Kinda useless

            //InGame
            createEvent("VictoryEvent"); //Updated
            createEvent("StartMatchEvent"); // new

            //Projectiles
            createEvent("ProjectileCreatedEvent");
            createEvent("ProjectileDeletedEvent");
            //createEvent("ProjectileModelChangedEvent");

            //TODO:
            /*createEvent("HealthChangedEvent");
            createEvent("HealthLostEvent");
            createEvent("HealthGainedEvent");
            createEvent("FastForwardToggleEvent");*/
        }

        public void createEvent(string eventName)
        {
            try
            {
                register(eventName, new List<MethodInfo>());
                //Logger.Log("Created event: " + eventName);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to create event: " + eventName);
                Logger.Log(ex.Message);
                Logger.Log(ex.StackTrace);
            }
        }

        /*/// <summary>
        /// Dictionary of eventNames with their callbacks
        /// </summary>
        Dictionary<string, List<MethodInfo>> theRegistry = new Dictionary<string, List<MethodInfo>>();*/

        public void listen(Type toSubscribe)
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
                            foreach(string currentEventName in getIDs())
                            {
                                if (currentEventName == eventAttrib.eventName)
                                {
                                    getItem(currentEventName).Add(method);
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
        public void dispatchEvent<T>(ref T e) where T : EventBase
        {
            foreach (string name in getIDs())
            {
                List<MethodInfo> callbacks = getItem(name);
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
