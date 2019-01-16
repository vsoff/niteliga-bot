using NL.Core;
using NL.NiteLiga.Core.DataAccess.DbContexts;
using NL.NiteLiga.Core.DataAccess.Repositories;
using NL.NiteLiga.Core.Game;
using NL.NiteLiga.Core.Game.Messengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace NL.NiteLiga.Core
{
    public static class NiteLigaCoreModule
    {
        public static void Register(UnityContainer container)
        {
            container.RegisterType<IGameManager, NiteLigaGameManager>(new TransientLifetimeManager());

            // Взаимодействие с БД.
            container.RegisterType<INiteLigaContextProvider, NiteLigaContextProvider>(new SingletonLifetimeManager());
            container.RegisterType<ITeamsRepository, TeamsRepository>(new SingletonLifetimeManager());
            container.RegisterType<IGamesRepository, GamesRepository>(new SingletonLifetimeManager());

            // Работа с сообщениями.
            container.RegisterType<IMessagePool, MessagePool>(new SingletonLifetimeManager());
            container.RegisterType<IMessenger, VkontakteMessenger>(new SingletonLifetimeManager());

        }
    }
}
