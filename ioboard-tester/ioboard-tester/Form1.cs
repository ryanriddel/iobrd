using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;

namespace ioboard_tester
{
    

    public partial class Form1 : Form
    {

        BadDogIOBoard baddog;

        public Form1()
        {
            InitializeComponent();
            baddog = new BadDogIOBoard();
            //baddog.SerialMessageReceived += Baddog_MessageReceived;
            baddog.InputChanged += Baddog_InputChanged;

        }

        private void Baddog_InputChanged(object sender, BadDogIOBoard.InputChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Input changed: " + BitConverter.ToString(e.inputStates));
        }

        private void Baddog_MessageReceived(object sender, BadDogIOBoard.SerialMessageReceivedEventArgs e)
        {
           AppendReceivedBox(BitConverter.ToString(e.messageBytes));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //connect button
            try
            {
                if (btnConnect.Text == "Connect")
                {
                    bool success = baddog.initializeBoard(comboBox1.Text, Convert.ToInt32(textBox1.Text));


                    if (success)
                    {
                        btnConnect.Text = "Disconnect";
                    }
                }
                else if (btnConnect.Text == "Disconnect")
                {
                    if (baddog._serialPort.IsOpen)
                    {
                        baddog._serialPort.Close();

                        baddog._serialPort.Dispose();
                    }
                    btnConnect.Text = "Connect";
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                Console.WriteLine(port);
                comboBox1.Items.Add(port);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            txtReceived.HideSelection = false;
            txtSent.HideSelection = false;

        }
        public void AppendSentBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendSentBox), new object[] { value });
                return;
            }
            txtSent.AppendText(value + Environment.NewLine);
        }

        public void AppendReceivedBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendReceivedBox), new object[] { value });
                return;
            }
            txtReceived.AppendText(value + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getCommTest();
            AppendSentBox(BitConverter.ToString(baddog.getCommTestCmdBytes));
            if (resp != null)
            {
                AppendReceivedBox(BitConverter.ToString(resp));
            }
            else AppendReceivedBox("NULL");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getSoftwareVer();

            AppendSentBox(BitConverter.ToString(baddog.getSoftwareVerCmdBytes));
            if (resp != null)
            {
                AppendReceivedBox(BitConverter.ToString(resp));
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtSent.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtReceived.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getHardwareVer();

            AppendSentBox(BitConverter.ToString(baddog.getHardwareVerCmdBytes)); 
            if (resp != null)
            {
                AppendReceivedBox(BitConverter.ToString(resp));
            }
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            bool resp = baddog.addPulsesToMeterOUT2(100);
            AppendReceivedBox(resp.ToString());
        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            bool resp = baddog.addPulsesToMeterOUT8(100);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getInputStates();

            AppendSentBox(BitConverter.ToString(baddog.getAllInputsCmdBytes));
            if (resp != null)
            {
                AppendReceivedBox(BitConverter.ToString(resp));
            }
        }


        private void button11_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getSwitchStates();
        }

        

        private void button12_Click(object sender, EventArgs e)
        {
            byte[] resp = baddog.getOutputStates();
            AppendSentBox(BitConverter.ToString(baddog.getOutputStatesCmdBytes));
            if (resp != null)
            {
                AppendReceivedBox(BitConverter.ToString(resp));
            }
        }

        private void txtIONumInput_TextChanged(object sender, EventArgs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {
            //get one input

            byte pinNum = (byte)Int16.Parse(txtIONumInput.Text);
            byte[] inputs = baddog.getInputStates();

            if ((inputs[(int)((pinNum + 1) / 8)] & (byte)(2 ^ (pinNum % 8))) == 1)
            {
                textBox2.Text = "HIGH";
            }
            else
                textBox2.Text = "LOW";

            if (inputs != null)
            {
                for (int i = 0; i < 3; i++)
                    System.Diagnostics.Debug.WriteLine(inputs[i]);
            }
            else
                System.Diagnostics.Debug.WriteLine("ERROR");
            //requestStateOfAllInputs();
        }


        private void button9_Click(object sender, EventArgs e)
        {
            bool resp = baddog.setOutputState(Convert.ToByte(txtIONumOutput.Text),(byte)(checkBoxOutput.Checked ? 1 : 0));
        }

       

        private void button13_Click(object sender, EventArgs e)
        {
            baddog.clearErrors();
        }

        
    }
    
}
