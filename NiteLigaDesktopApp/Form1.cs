using NL.Core;
using NL.NiteLiga.Core;
using NL.NiteLiga.Core.DataAccess.Entites;
using NL.NiteLiga.Core.DataAccess.Repositories;
using NL.NiteLiga.Core.Game;
using NL.NiteLiga.Core.Game.Messengers;
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
        private const long _gameMatchId = 0;
        private GameTemplate _selectedTemplate;
        private Team[] _teams;
        private Team _selectedTeam;

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
            _selectedTemplate = gamesRepos.GetTemplateHard(_templates[templateIndex].Id);
            _gameManager = _container.Resolve<NiteLigaGameManager>();
            _gameManager.Start(new NiteLigaGameConfiguration
            {
                GameMatchId = _gameMatchId,
                Config = _selectedTemplate.Config,
                Settings = _selectedTemplate.Settings
            });

            var teamsRepos = _container.Resolve<ITeamsRepository>();
            _teams = teamsRepos.GetTeams(_selectedTemplate.Settings.TeamIds);
            comboBox2.Items.Clear();
            foreach (var team in _teams)
                comboBox2.Items.Add($"{team.Id}. {team.Name}");

            panel1.Enabled = false;
            panel2.Enabled = true;

            Task.Run(() =>
            {
                while (true)
                {
                    _gameManager.Iterate();
                    if (_gameManager.Status == GameStatusType.Ended || _gameManager.Status == GameStatusType.Aborted)
                    {
                        panel2.Enabled = false;
                        panel1.Enabled = true;
                        break;
                    }
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

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedPlayerIndex = comboBox3.SelectedIndex;
            if (selectedPlayerIndex == -1)
            {
                MessageBox.Show("Не выбран игрок!");
                return;
            }

            var messagePool = _container.Resolve<IMessagePool>();
            messagePool.AddMessage(new NL.NiteLiga.Core.Game.Messengers.Message
            {
                GameMatchId = _gameMatchId,
                Team = _selectedTeam,
                Player = _selectedTeam.Players[selectedPlayerIndex],
                Text = textBox1.Text
            });

            textBox1.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedTeam = _teams[comboBox2.SelectedIndex];

            comboBox3.Items.Clear();
            foreach (var player in _selectedTeam.Players)
                comboBox3.Items.Add($"{player.Id}. {player.GetFullName()}");
        }
    }
}
