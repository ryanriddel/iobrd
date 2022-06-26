using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



using System.IO.Ports;
using System.Threading;

namespace ioboard_tester
{
    public partial class Form1 : Form
    {
        SerialPort sPort;

        byte[] getCommTestCmdBytes = { 0x58, 0x59, 0xF0, 0x01, 0xF1 };
        byte[] getSoftwareVerCmdBytes = { 0x58, 0x59, 0xF3, 0x01, 0xF2 };
        byte[] getHardwareVerCmdBytes = { 0x58, 0x59, 0xF2, 0x01, 0xF3 };

        byte[] getAllInputsCmdBytes = { 0x58, 0x59, 0xA0, 0x01, 0xA1 };
        byte[] getOutputStatesCmdBytes = { 0x58, 0x59, 0xA3, 0x01, 0xA2 };
        byte[] getSwitchStatesCmdBytes = { 0x58, 0x59, 0xA2, 0x01, 0xA3 };

        ManualResetEvent getInputResponse;
        ManualResetEvent getOutputStateResponse;
        ManualResetEvent getSwitchStatesResponse;

        
        byte[] setOutputStateCmdBytes(byte outputNum, byte state) //state: 0=high impedance, 1=enabled
        {

            byte[] cmdBytes = { 0x58, 0x59, 0, 0x03, 0xB2, outputNum, state };

            for (int i = 3; i < cmdBytes.Length; i++)
                cmdBytes[2] ^= cmdBytes[i];


            return cmdBytes;
        }
        byte[] setMeter1PulsesCmdBytes(int numPulses)  //Meter1 corresponds to OUT2
        {
            
            byte lowByteNumPulses = (byte) (numPulses & 0xFF);
            byte highByteNumPulses = (byte)(numPulses << 8);
            byte[] cmdBytes = {0x58, 0x59, 0, 0x05, 0xB3, highByteNumPulses,
                                  lowByteNumPulses, 0x06, 0x06 };

            for (int i = 3; i < cmdBytes.Length; i++)
                cmdBytes[2] ^= cmdBytes[i];


            return cmdBytes;
        }

        byte[] setMeter2PulsesCmdBytes(int numPulses)  //Meter2 corresponds to OUT8
        {

            byte lowByteNumPulses = (byte)(numPulses & 0xFF);
            byte highByteNumPulses = (byte)(numPulses << 8);
            byte[] cmdBytes = {0x58, 0x59, 0, 0x05, 0xB7, highByteNumPulses,
                                  lowByteNumPulses, 0x06, 0x06 };

            for (int i = 3; i < cmdBytes.Length; i++)
                cmdBytes[2] ^= cmdBytes[i];


            return cmdBytes;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach(string port in SerialPort.GetPortNames())
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

        private void button2_Click(object sender, EventArgs e)
        {
            //connect button
            try
            {
                if (btnConnect.Text == "Connect")
                {
                    sPort = new SerialPort(comboBox1.Text, Convert.ToInt32(textBox1.Text));
                    
                    sPort.Open();
                    sPort.DataReceived += SPort_DataReceived;

                    if (sPort.IsOpen)
                    {
                        btnConnect.Text = "Disconnect";
                    }
                }
                else if (btnConnect.Text == "Disconnect")
                {
                    if(sPort.IsOpen)
                    {
                        sPort.Close();
                        
                        sPort.Dispose();
                    }
                    btnConnect.Text = "Connect";
                }
            }
            catch(Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }

        void serialReader()
        {

        }

        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            
            
            
            
            
            
            
            
            /*List<byte> byteList = new List<byte>();

            
            while (sPort.BytesToRead > 0 || byteList.Count < 4 )
            {
                int temp = sPort.ReadByte();
                if(temp != -1)
                    byteList.Add((byte) temp);
            }

            byte[] messageBytes = byteList.ToArray();
            string inputString = BitConverter.ToString(messageBytes);

            AppendReceivedBox(inputString);*/



        }

        public void AppendSentBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendSentBox), new object[] { value });
                return;
            }
            //txtSent.Text += value + Environment.NewLine;
            txtSent.AppendText(value + Environment.NewLine);
        }

        public void AppendReceivedBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendReceivedBox), new object[] { value });
                return;
            }
            //txtReceived.Text += value + Environment.NewLine;
            txtReceived.AppendText(value + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //comm test
            byte[] msg = new byte[] { 0x58, 0x59, 0xF0, 0x01, 0xF1 };

            sPort.Write(msg, 0, 5);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //software version
            byte[] msg = new byte[] { 0x58, 0x59, 0xF3, 0x01, 0xF2 };

            sPort.Write(msg, 0, 5);
            AppendSentBox(BitConverter.ToString(msg));
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
            byte[] msg = getHardwareVerCmdBytes;


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            byte[] msg = setMeter1PulsesCmdBytes(100);


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));

        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            byte[] msg = setMeter2PulsesCmdBytes(100);


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            requestStateOfAllInputs();
        }

        void requestStateOfAllInputs()
        {
            //get all inputs
            byte[] msg = getAllInputsCmdBytes;


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            requestSwitchStates();
        }

        void requestSwitchStates()
        {
            //get switch states
            byte[] msg = getSwitchStatesCmdBytes;


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            requestStatesOfOutputs();
        }

        void requestStatesOfOutputs()
        {
            //get state of outputs
            byte[] msg = getOutputStatesCmdBytes;


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void txtIONumInput_TextChanged(object sender, EventArgs e)
        {

        }
        System.Threading.ManualResetEvent mResEv;
        private void button10_Click(object sender, EventArgs e)
        {
            //get one input
            

            requestStateOfAllInputs();
        }

        void requestInputState(byte inputNum)
        {
            //get state of outputs
            byte[] msg = getOutputStatesCmdBytes;


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            requestSetOutputState();
        }

        void requestSetOutputState()
        {
            //set output state
            byte state = (byte)(checkBoxOutput.Checked ? 1 : 0);
            byte outputNum = Convert.ToByte(txtIONumOutput.Text);

            byte[] msg = setOutputStateCmdBytes(outputNum, state);


            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            clearErrors();
        }

        void clearErrors()
        {
            byte[] msg = { 0x58, 0x59, 0xC0, 0x01, 0xC1 };

            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }
    }
}
