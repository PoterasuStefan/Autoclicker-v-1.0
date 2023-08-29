using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoclicker_v_1._0
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int LEFTUP = 0x0004;
        private const int LEFTDOWN = 0X0002;
        private const int RIGHTUP = 0x0010;
        private const int RIGHTDOWN = 0x0008;
        private int ACTIVEUP = 0x0004;
        private int ACTIVEDOWN = 0X0002;
        public int interval = 5;
        public bool Click = false;
        public int parsedValue;
        private Keys selectedStartHotkey = Keys.Up;
        private Keys selectedStopHotkey = Keys.Down;


        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread AC = new Thread(Autoclick);
            backgroundWorker1.RunWorkerAsync();
            AC.Start();
            SettingsPanel.Visible = false;
            checkBox1.Checked = true;
        }
        private void Autoclick()
        {
            while (true)
            {
                if (Click)
                {
                    mouse_event(dwFlags: ACTIVEDOWN, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                    Thread.Sleep(interval);  // Wait for the click interval
                    mouse_event(dwFlags: ACTIVEUP, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                }
                Thread.Sleep(5);  // A small delay before the next iteration
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (checkBox1.Checked)
                {
                    if (GetAsyncKeyState(selectedStopHotkey) < 0)
                    {
                        Click = false;
                    }
                    else if (GetAsyncKeyState(selectedStartHotkey) < 0)
                    {
                        Click = true;
                    }
                    Thread.Sleep(1);
                }
                Thread.Sleep(1);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out parsedValue))
            {
                MessageBox.Show("Add a number");
                return;
            }
            else
            {
                interval = int.Parse(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SettingsPanel.Visible == true)
            {
                MainPanel.Visible = true;
                SettingsPanel.Visible = false;
            }
            else 
            {
                SettingsPanel.Visible = true;
                MainPanel.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            // Subscribe to the KeyDown event of the form to capture the next key press
            this.KeyDown += Form1_KeyDownAfterButton3Click;

            // Focus the form to make sure it receives key events
            this.Focus();

        }

        private void Form1_KeyDownAfterButton3Click(object sender, KeyEventArgs e)
        {
            // Update the button text with the pressed key's representation
            button3.Text = $" {e.KeyCode}";
            selectedStartHotkey = e.KeyCode;

            // Unsubscribe from the KeyDown event
            this.KeyDown -= Form1_KeyDownAfterButton3Click;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Subscribe to the KeyDown event of the form to capture the next key press
            this.KeyDown += Form1_KeyDownAfterButton4Click;

            // Focus the form to make sure it receives key events
            this.Focus();
        }

        private void Form1_KeyDownAfterButton4Click(object sender, KeyEventArgs e)
        {
            // Update the button text with the pressed key's representation
            button4.Text = $" {e.KeyCode}";
            selectedStopHotkey = e.KeyCode;

            // Unsubscribe from the KeyDown event
            this.KeyDown -= Form1_KeyDownAfterButton4Click;
        }

        private void SettingsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LeftClickSwitch_Click(object sender, EventArgs e)
        {
            ACTIVEDOWN = LEFTDOWN;
            ACTIVEUP = LEFTDOWN;
            pictureBox1.Image = Properties.Resources.mouse_01_generated;
            Home_ClickMode_Label.Text = "Mode: Left Click";
        }

        private void RightClickSwitch_Click(object sender, EventArgs e)
        {
            ACTIVEDOWN = RIGHTDOWN;
            ACTIVEUP = RIGHTDOWN;
            pictureBox1.Image = Properties.Resources.mouse_02_generated;
            Home_ClickMode_Label.Text = "Mode: Right Click";
        }
    }
}
