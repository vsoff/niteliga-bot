using NL.Core;
using NL.NiteLiga.Core;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Repositories;
using NL.NiteLiga.Core.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace NiteLigaDesktopApp
{
    public partial class Form1 : Form
    {
        private readonly IUnityContainer _container;
        private NiteLigaGameManager _gameManager;
        private GameTemplate[] _templates;

        public Form1()
        {
            InitializeComponent();

            _container = new UnityContainer();
            NiteLigaCoreModule.Register(_container);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int templateIndex = comboBox1.SelectedIndex;
            if (templateIndex == -1)
            {
                MessageBox.Show("Не выбрано ни одной игры");
                return;
            }

            var gamesRepos = _container.Resolve<IGamesRepository>();
            var template = gamesRepos.GetTemplateHard(_templates[templateIndex].Id);
            _gameManager = _container.Resolve<NiteLigaGameManager>();
            _gameManager.Start(new NiteLigaGameConfiguration
            {
                GameMatchId = 0,
                Config = template.Config,
                Settings = template.Settings
            });

            Task.Run(() =>
            {
                while (true)
                {
                    _gameManager.Iterate();
                    if (_gameManager.Status == GameStatusType.Ended || _gameManager.Status == GameStatusType.Aborted)
                        break;
                    Thread.Sleep(1000);
                }
            });
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            var gamesRepository = _container.Resolve<IGamesRepository>();
            _templates = gamesRepository.GetAllTemplatesLight();

            comboBox1.Items.Clear();
            foreach (var template in _templates)
                comboBox1.Items.Add($"{template.Id}. {template.Caption}");
        }
    }
}
