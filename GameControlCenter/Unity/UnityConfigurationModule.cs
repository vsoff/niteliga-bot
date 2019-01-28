using GameControlCenter.NiteLiga.Game;
using GameControlCenter.Services;
using NL.NiteLiga.Core.DataAccess.DbContexts;
using NL.NiteLiga.Core.DataAccess.Repositories;
using NL.NiteLiga.Core.Serialization;
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
            // Репозитории и работа с БД.
            container.RegisterType<IGamesRepository, GamesRepository>(new SingletonLifetimeManager());
            container.RegisterType<ITeamsRepository, TeamsRepository>(new SingletonLifetimeManager());
            container.RegisterType<INiteLigaContextProvider, NiteLigaContextProvider>(new SingletonLifetimeManager());

            container.RegisterType<INiteLigaDeserializator, NiteLigaDeserializator>(new SingletonLifetimeManager());

            // Остальное.
            container.RegisterType<IEmailSender, EmailSender>(new SingletonLifetimeManager());
            container.RegisterType<IGameTemplateBuilder, GameTemplateBuilder>(new SingletonLifetimeManager());
        }
    }
}
