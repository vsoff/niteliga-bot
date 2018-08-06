using NiteLigaLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using NiteLigaLibrary.Classes;
using Newtonsoft.Json;
using NiteLigaLibrary.Database.Models;
using NiteLigaLibrary.Database;

namespace TestingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Enable-Migrations -Force   -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose
            //Add-Migration Version_Name -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose
            //Update-Database            -ConnectionString "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" -ConnectionProviderName "System.Data.SqlClient" -Verbose

            NiteLigaContext.ConnectionString = "data source=DESKTOP-10P39AB;initial catalog=NiteLiga;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        }

        private GameManager gm;

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> teams = null;
            List<long> playerVkIds = null;
            using (var db = new NiteLigaContext())
            {
                teams = db.Teams.Take(3).Select(x => x.Id).ToList();
                playerVkIds = db.Players.Where(x => x.VkId != null).Select(x => (long)x.VkId).ToList();
            }

            comboBox1.Items.Clear();
            foreach (var i in playerVkIds)
                comboBox1.Items.Add(i);

            GameConfig config = new GameConfig("{\"GameDate\":\"2018-08-02T23:04:54.2585947+03:00\",\"Address\":\"Место старта\",\"Description\":\"Описание игры\",\"Tasks\":[{\"Task\":\"Задание1\",\"Hint1\":\"Задание1\",\"Hint2\":\"Задание1\",\"Address\":\"Задание1\",\"Code\":\"Задание1\",\"Lat\":\"1.0000\",\"Lon\":\"1.0000\"},{\"Task\":\"Задание2\",\"Hint1\":\"Задание2\",\"Hint2\":\"Задание2\",\"Address\":\"Задание2\",\"Code\":\"Задание2\",\"Lat\":\"2.0000\",\"Lon\":\"2.0000\"},{\"Task\":\"Задание3\",\"Hint1\":\"Задание3\",\"Hint2\":\"Задание3\",\"Address\":\"Задание3\",\"Code\":\"Задание3\",\"Lat\":\"3.0000\",\"Lon\":\"3.0000\"}],\"TaskGrid\":[[\"0\",\"1\"],[\"2\",\"0\"],[\"1\",\"0\"]]}");
            GameSetting setting = new GameSetting()
            {
                GameDurationMin = 1,
                GameClosingDurationMin = 1,
                SecondsDelayStart = 30,
                Hint1DelaySec = 20,
                Hint2DelaySec = 20,
                TaskDropDelaySec = 10,
                TeamIds = teams
            };
            gm = new GameManager(config, setting);
            gm.Start();
            
            //GameConfig gc = JsonConvert.DeserializeObject<GameConfig>(JSONCONFIG);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gm == null) return;

            gm.Iterate();

            listBox1.Items.Clear();
            foreach (var a in gm.StoredEvents)
                listBox1.Items.Add(JsonConvert.SerializeObject(a));

            foreach (var a in gm.Noticer.PullOutput())
                textBox_Messages.Text += $"{a.Player.FirstName} {a.Player.LastName} << {a.Text}\r\n";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gm == null)
            {
                label1.Text = $"Game isn't launched.";
                return;
            }

            label1.Text = $"Last update: {DateTime.Now.ToLongTimeString()} [{gm.GameStatus.ToString()}]";
            button2.PerformClick();
        }

        private void button_SendMessage_Click(object sender, EventArgs e)
        {
            if (gm == null || comboBox1.SelectedIndex == -1) return;
            var message = new NiteLigaLibrary.Classes.Message((long)comboBox1.SelectedItem, textBox1.Text);
            gm.Noticer.AddInputMessage(message);
        }
    }
}
