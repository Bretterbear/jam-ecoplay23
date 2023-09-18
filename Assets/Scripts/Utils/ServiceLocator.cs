using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Creates a simple globally accessible service locator
    /// </summary>
    public class ServiceLocator
    {
        // --- Private Variable Declarations --- //
        private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

        private ServiceLocator() { }                                // Private constructor for protection
        public static ServiceLocator Instance { get; private set; } // Runtime singleton

        /// <summary>
        /// Sets up a new ServiceLocator singleton for the runtime. Needs to be run at game initialization
        /// </summary>
        public static void Initialize()
        {
            Instance = new ServiceLocator();
        }

        /// <summary>
        /// Returns the instance of the service specified by type
        /// </summary>
        /// <typeparam name="T">the service to be retreived (MUST implement the IService interface)</typeparam>
        /// <returns>The instance of the service as a strongly-typed object of type T</returns>
        /// <exception cref="InvalidOperationException">Thrown if no service of type T has been registered</exception>
        public T Get<T>() where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                return (T)_services[key];
            }
            else
            {
                Debug.LogError($"<color=red>{key} not registered with {GetType().Name}</color>");
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Registers a service instance with the ServiceLocator
        /// </summary>
        /// <typeparam name="T">the type of service to be registered (MUST implement the IService interface)</typeparam>
        /// <param name="service">the service instance to be registered</param>
        /// <exception cref="ArgumentException">thrown if you try to double register a service already registered</exception>"
        public void Register<T>(T service) where T : IService
        {
            string key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                _services.Add(key, service);
                Debug.Log($"<color=green> Successfully registered {key} </color>");
            }
            else
            {
                Debug.LogError($"<color=red> Attempted to register service of type " +
                    $"{key} which is already registered with the {GetType().Name}</color>");
                return;
            }
        }

        /// <summary>
        /// Unregister a service instance from the ServiceLocator
        /// </summary>
        /// <typeparam name="T">the type of service to unregister (MUST implement the IService interface)</typeparam>
        /// <exception cref="ArgumentException">thrown if service isn't registered in the first place</exception>"
        public void Unregister<T>() where T : IService
        {
            string key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                _services.Remove(key);
            }
            else
            {
                Debug.LogError($"<color=red>Attempted to unregister service of type " +
                    $"{key} which is not registered with the {GetType().Name}</color>");
                return;
            }
        }
    }
}