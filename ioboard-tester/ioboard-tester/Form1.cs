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
        SerialPort sPort;

        //This synchronizes the serial processing thread with the datareceived event on the SerialPort.
        //This is redundant, and one could just check for BytesToRead in the serial processing thread
        ManualResetEvent serialDataReceivedEvent;


        //These two dictionaries are the foundation of this API's synchronization of the asynchronous request-response serial comms
        //The dictionary key is the command byte, which connects responses to the requests that caused them
        Dictionary<byte, ManualResetEvent> synchronizationEventDictionary = new Dictionary<byte, ManualResetEvent>();
        Dictionary<byte, byte[]> synchronizationReturnValues = new Dictionary<byte, byte[]>();

        //raised when a falling edge is detected on an input
        //the eventargs contain a 3 byte array, which is a bitmap of the state of the 24 inputs (0=LOW, 1=HIGH)
        public event EventHandler<InputChangedEventArgs> InputChanged;

        const int serialResponseTimeoutMilliseconds = 1000;

        byte[] getCommTestCmdBytes = { 0x58, 0x59, 0xF0, 0x01, 0xF1 };
        byte[] getSoftwareVerCmdBytes = { 0x58, 0x59, 0xF3, 0x01, 0xF2 };
        byte[] getHardwareVerCmdBytes = { 0x58, 0x59, 0xF2, 0x01, 0xF3 };

        byte[] getAllInputsCmdBytes = { 0x58, 0x59, 0xA0, 0x01, 0xA1 };
        byte[] getOutputStatesCmdBytes = { 0x58, 0x59, 0xA3, 0x01, 0xA2 };
        byte[] getSwitchStatesCmdBytes = { 0x58, 0x59, 0xA2, 0x01, 0xA3 };

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
                                  lowByteNumPulses, 0x06, 0x06 }; //corresponds to 60mS pulse period

            for (int i = 3; i < cmdBytes.Length; i++)
                cmdBytes[2] ^= cmdBytes[i];


            return cmdBytes;
        }

        byte[] setMeter2PulsesCmdBytes(int numPulses)  //Meter2 corresponds to OUT8
        {

            byte lowByteNumPulses = (byte)(numPulses & 0xFF);
            byte highByteNumPulses = (byte)(numPulses << 8);
            byte[] cmdBytes = {0x58, 0x59, 0, 0x05, 0xB7, highByteNumPulses,
                                  lowByteNumPulses, 0x06, 0x06 }; //corresponds to 60mS pulse period

            for (int i = 3; i < cmdBytes.Length; i++)
                cmdBytes[2] ^= cmdBytes[i];

            
            return cmdBytes;
        }


        

        


        void receivedMessageDispatcher(byte[] responseBytes)
        {
            byte commandByte = responseBytes[4];
            AppendReceivedBox(BitConverter.ToString(responseBytes));

            if (synchronizationEventDictionary.ContainsKey(commandByte))
            {
                //this synchronizes the method that initiated this response with the return value
                synchronizationReturnValues[commandByte] = responseBytes;
                synchronizationEventDictionary[commandByte].Set();
            }

            if(commandByte == 0xA6)
            {
                //this is an io-board initiated message which indicates a falling edge on an input
                byte[] inputMap = { responseBytes[5], responseBytes[6], responseBytes[7] };

                InputChanged?.Invoke(this, new InputChangedEventArgs(inputMap));
            }

        }



        private ConcurrentQueue<byte> serialByteQueue = new ConcurrentQueue<byte>();
        //this is intended to run in its own thread
        private void serialReader()
        {
            while(serialDataReceivedEvent.WaitOne())
            {
                serialDataReceivedEvent.Reset(); //reset immediately in case new data is received while processing
                
                if(serialByteQueue.Count > 2)
                {
                    //wait until we've received a header to do any processing

                    if(serialByteQueue.ElementAt<byte>(0) == 0x58 && serialByteQueue.ElementAt<byte>(1) == 0x59)
                    {
                        //we have received the beginning of a response

                        if(serialByteQueue.Count >= 5)
                        {
                            //we have possibly received a complete response, check length byte to confirm

                            byte lengthByteVal = serialByteQueue.ElementAt<byte>(3);
                            int totalMessageLength = 4 + lengthByteVal;

                            if(totalMessageLength <= serialByteQueue.Count)
                            {
                                //a complete response has been received. take the message out of the serialbytequeue
                                byte[] responseBytes = new byte[totalMessageLength];
                                for (int i = 0; i < totalMessageLength; i++)
                                {
                                    bool result = serialByteQueue.TryDequeue(out responseBytes[i]);
                                    
                                    if (!result)
                                        throw new Exception("THIS SHOULD NOT HAPPEN. YOU MADE A MISTAKE.");
                                }

                                //confirm validity by comparing the check byte
                                byte checksum = responseBytes[3];
                                for (int i = 4; i < responseBytes.Length; i++)
                                    checksum ^= responseBytes[i];

                                if(checksum == responseBytes[2])
                                {
                                    //valid message confirmed

                                    receivedMessageDispatcher(responseBytes);
                                }
                                else
                                {
                                    //uh oh...checksum was incorrect.  discard message.
                                }
                            }
                            else
                            {
                                //we have not received the entire message. do nothing...
                            }
                        }
                        else
                        {
                            //we have received a valid header but not an entire message. do nothing...
                        }
                    }
                    else
                    {
                        //the serial buffer is in a bad state or the io board sent bad data.  
                        //empty bytes until we hit 0x58 
                        byte val;
                        do
                        {
                            if (!serialByteQueue.TryPeek(out val))
                            {
                                val = 0x58; //the queue is empty. exit the loop 
                            }
                        } while (val != 0x58);
                    }
                }
                else
                {
                    //not enough data in the serial buffer. do nothing...
                }
            }
        }

        
        

        

        //note that this function will return null if there is a timeout
        byte[] doSerialTransaction(byte[] msg)
        {
            sPort.Write(msg, 0, msg.Length);
            byte cmdByte = msg[4];

            AppendSentBox(BitConverter.ToString(msg));

            if (!synchronizationEventDictionary.ContainsKey(cmdByte))
                synchronizationEventDictionary[cmdByte] = new ManualResetEvent(false);

            if (synchronizationEventDictionary[cmdByte].WaitOne(serialResponseTimeoutMilliseconds))
            {
                AppendReceivedBox(BitConverter.ToString(synchronizationReturnValues[cmdByte]));
                return synchronizationReturnValues[cmdByte];
            }
            else
            {
                //timeout, no response received
                System.Diagnostics.Debug.WriteLine("Uh oh.");
                return null;
            }
        }

        byte[] getCommTest()
        {
            byte[] msg = getCommTestCmdBytes;

            return doSerialTransaction(msg);
        }

        byte[] getHardwareVer()
        {
            byte[] msg = getHardwareVerCmdBytes;

            return doSerialTransaction(msg);
        }

        byte[] getSoftwareVer()
        {
            byte[] msg = getSoftwareVerCmdBytes;

            return doSerialTransaction(msg);
        }


        byte[] getAllInputs()
        {
            byte[] msg = getAllInputsCmdBytes;

            return doSerialTransaction(msg);
        }

        byte[] getOutputStates()
        {
            byte[] msg = getOutputStatesCmdBytes;

            return doSerialTransaction(msg);
        }

        byte[] getSwitchStates()
        {
            byte[] msg = getSwitchStatesCmdBytes;

            return doSerialTransaction(msg);
        }

        bool setOutputState(byte outputNum, byte state)
        {
            byte[] msg = setOutputStateCmdBytes(outputNum, state);

            return (doSerialTransaction(msg)[4] == 0x0B2 ? true : false);
        }

        bool addPulsesToMeterOUT2(int numPulses)
        {
            byte[] msg = setMeter1PulsesCmdBytes(numPulses);

            return (doSerialTransaction(msg)[4] == 0xB3 ? true : false);
        }

        bool addPulsesToMeterOUT8(int numPulses)
        {
            byte[] msg = setMeter2PulsesCmdBytes(numPulses);

            return (doSerialTransaction(msg)[4] == 0xB7 ? true : false);
        }

        void clearErrors()
        {
            byte[] msg = { 0x58, 0x59, 0xC0, 0x01, 0xC1 };

            sPort.Write(msg, 0, msg.Length);
            AppendSentBox(BitConverter.ToString(msg));
        }

        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (sPort.BytesToRead > 0)
            {
                int temp = sPort.ReadByte();
                if (temp != -1)
                {
                    serialByteQueue.Enqueue((byte)temp);
                }
            }


            serialDataReceivedEvent.Set();

        }

        public Form1()
        {
            InitializeComponent();

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
                    if (sPort.IsOpen)
                    {
                        sPort.Close();

                        sPort.Dispose();
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


        System.Threading.Thread serialParserThread = null;
        private void Form1_Load(object sender, EventArgs e)
        {

            txtReceived.HideSelection = false;
            txtSent.HideSelection = false;



            serialDataReceivedEvent = new ManualResetEvent(false);
            if (serialParserThread == null)
            {
                serialParserThread = new Thread(serialReader);
                serialParserThread.Start();
            }
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
            getCommTest();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            getSoftwareVer();
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
            getHardwareVer();
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            addPulsesToMeterOUT2(100);
        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            addPulsesToMeterOUT8(100);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            getAllInputs();
        }


        private void button11_Click(object sender, EventArgs e)
        {
            getSwitchStates();
        }

        

        private void button12_Click(object sender, EventArgs e)
        {
            getOutputStates();
        }

        private void txtIONumInput_TextChanged(object sender, EventArgs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {
            //get one input

            byte pinNum = (byte)Int16.Parse(txtIONumInput.Text);
            byte[] inputs = doSerialTransaction(getAllInputsCmdBytes);

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
            setOutputState((byte)(checkBoxOutput.Checked ? 1 : 0), Convert.ToByte(txtIONumOutput.Text));
        }

       

        private void button13_Click(object sender, EventArgs e)
        {
            clearErrors();
        }

        
    }
    public class InputChangedEventArgs : EventArgs
    {
        byte[] inputStates;
        public InputChangedEventArgs(byte[] state)
        {
            inputStates = new byte[] { state[0], state[1], state[2] };
        }
    }
}
