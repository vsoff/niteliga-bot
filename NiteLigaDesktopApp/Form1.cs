using NL.Core;
using NL.NiteLiga.Core;
using NL.NiteLiga.Core.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace NiteLigaDesktopApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var container = new UnityContainer())
            {
                NiteLigaCoreModule.Register(container);
                var teamsRepository = container.Resolve<ITeamsRepository>();
                var a = teamsRepository.GetTeam(1);
                var aa = new TeamsRepository(null);
            }
        }
    }
}
