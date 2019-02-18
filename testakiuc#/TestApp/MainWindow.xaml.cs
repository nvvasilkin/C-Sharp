using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Diagnostics;

using AkiuLib;
using System.Windows.Threading;

namespace TestApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = null;
        string agentSysName;
        IntPtr agent;

        public MainWindow()
        {
        	
        	InitializeComponent();
            this.comboBox.Items.Add(Properties.Settings.Default.server_ip);
            this.comboBox.SelectedIndex = 0;

            agentSysName = Properties.Settings.Default.sysName;
            this.label1.Content = agentSysName;
            this.textBox12.Text = Properties.Settings.Default.db_server;
            this.textBox13.Text = Properties.Settings.Default.db_name;
            this.textBox14.Text = Properties.Settings.Default.db_user_name;
            this.textBox15.Text = Properties.Settings.Default.db_pass;

            agent = Akiu.createAgentInstance();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(DisplayTimeEvent);
            timer.Interval = new TimeSpan(0,0,0,1);
            timer.Start();

        }

        string doABarrelRoll(string x)
        {
           // this.lbDis.Content = x;
            return "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            bool st = Akiu.authorizationAgent(agent, textBox.Text);
            if (st == true)
            {
                agentSysName = textBox.Text;
                this.label1.Content = agentSysName;

            }

            this.label1.Content = Marshal.PtrToStringAuto(Akiu.getAgentSysName(agent));
        }

        private void DisplayTimeEvent( object source, EventArgs e)
        {

            bool m = Akiu.getStatusRegisterAgent(agent,textBox.Text);

            if (m==true)
            {
                this.label5.Content = "Зарегистрирован";
            }
            else
            {
                this.label5.Content = "Не зарегистрирован";
            }

            this.label25.Content = "Ожидает передачи";

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.registerAgent(agent, textBox1.Text,textBox18.Text, textBox2.Text);

            if (m == true)
            {
                this.label10.Content = "Зарегистрирован";
                this.label1.Content = textBox2.Text;
                agentSysName = textBox2.Text;
            }
            else
            {
                this.label10.Content = "Не зарегистрирован";
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.registerParametr(agent, textBox4.Text, textBox3.Text, textBox19.Text, textBox5.Text,textBox20.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);
            if (m == true)
            {
                this.label20.Content = "Зарегистрирован";
            }
            else
            {
                this.label20.Content = "Не зарегистрирован";
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.setParameterValue(agent, textBox10.Text, textBox11.Text);
            if (m == true)
            {
                this.label25.Content = "Передано";
            }
            else
            {
                this.label25.Content = "Не передано";
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.checkCurrentConnectionAgentToDB(agent, agentSysName);
            if (m == true)
            {
                label33.Content = "Существует";
            }
            else
            {
                label33.Content = "Не существует";
            }

            m = Akiu.checkCurrentReconnectAgentToDB(agent, agentSysName);
            if (m == true)
            {
                label35.Content = "Существует";
            }
            else
            {
                label35.Content = "Не существует";
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.deleteAgent(agent, textBox16.Text, textBox17.Text);
            if (m==true)
            {
                label40.Content = "Агент удален";
            }
            else
            {
                label40.Content = "Ошибка удаления агента";
            }
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            bool m = Akiu.registerParametr(agent, textBox4.Text, textBox3.Text, textBox19.Text, textBox5.Text, textBox20.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);
            if (m == true)
            {
                this.label20.Content = "Зарегистрирован";
            }
            else
            {
                this.label20.Content = "Не зарегистрирован";
            }
        }
    }
}
