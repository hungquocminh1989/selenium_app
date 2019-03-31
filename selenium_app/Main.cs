using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using selenium_app.Library;
using System.Threading;

namespace selenium_app
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            DriverConnector abc = new DriverConnector();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //RemoteDriverConnector abc = new RemoteDriverConnector("http://192.168.99.100:4444/wd/hub/");
            //abc.GoToLink("https://api.ipify.org");
            //RemoteDriverConnector abc1 = new RemoteDriverConnector("http://192.168.99.100:4444/wd/hub/");
            //abc1.GoToLink("https://www.google.com");

            Dictionary<string, Thread> dic_thread = new Dictionary<string, Thread>();
            for (int i = 0; i < 10; i++)
            {
                dic_thread["thread_" + i] = new Thread(new ThreadStart(abc));
                dic_thread["thread_" + i].Start();
                //abc();
            }
            
        }
        public void abc()
        {
            RemoteDriverConnector abc1 = new RemoteDriverConnector("http://192.168.99.100:4444/wd/hub/");
            abc1.GoToLink("https://www.google.com");
        }
    }
}
