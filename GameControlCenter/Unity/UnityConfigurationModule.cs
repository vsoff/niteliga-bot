using GameControlCenter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace GameControlCenter.Unity
{
    public static class UnityConfigurationModule
    {
        public static void Register(IUnityContainer container)
        {
            container.RegisterType<IEmailSender, EmailSender>(new SingletonLifetimeManager());
        }
    }
}
