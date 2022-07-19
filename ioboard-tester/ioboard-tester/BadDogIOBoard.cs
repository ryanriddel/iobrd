using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;

class BadDogIOBoard
{
    public class InputChangedEventArgs : EventArgs
    {
        public byte[] inputStates;
        public InputChangedEventArgs(byte[] state)
        {
            inputStates = state;
        }
    }

    public class SerialMessageReceivedEventArgs : EventArgs
    {
        public byte[] messageBytes;
        public SerialMessageReceivedEventArgs(byte[] state)
        {
            messageBytes = state;
        }
    }

    /*public class CoinInOutPulsesReceivedEventArgs : EventArgs
    {
        public byte numPulses;
            
        public CoinInOutPulsesReceivedEventArgs(byte pulses)
        {
            numPulses = pulses;
        }
    }*/

    //raised when a falling edge is detected on an input
    //the eventargs contain a 3 byte array, which is a bitmap of the state of the 24 inputs (0=LOW, 1=HIGH)
    public event EventHandler<InputChangedEventArgs> InputChanged;

    //raised when a complete message is received from the io board
    public event EventHandler<SerialMessageReceivedEventArgs> SerialMessageReceived;

    //raised when a series of pulses are raised on IN18
    //public event EventHandler<CoinInOutPulsesReceivedEventArgs> CoinInOutPulsesReceived;

    public SerialPort _serialPort;
    private Thread serialParserThread;

    byte lastSentCommandByte = 0;

    public BadDogIOBoard()
    {
            
    }

    ~BadDogIOBoard()
    {
        if(_serialPort != null)
            if(_serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();

            }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="serialPortName">e.x. "COM7"</param>
    /// <param name="baudRate"></param>
    /// <returns>True if board is successfully initialized, otherwise false</returns>
    public bool initializeBoard(string serialPortName, int baudRate=115200)
    {
        _serialPort = new SerialPort(serialPortName, Convert.ToInt32(baudRate));

        _serialPort.Open();

        if(_serialPort.IsOpen)
        {
            serialDataReceivedEvent = new ManualResetEvent(false);
            if (serialParserThread == null)
            {
                serialParserThread = new Thread(doSerialParser);
                serialParserThread.Start();
            }

            return true;
        }
        else
        {
            return false;
        }
    }


       

    public byte[] getCommTest()
    {
        byte[] msg = getCommTestCmdBytes;

        return doSerialTransaction(msg);
    }

    public byte[] getHardwareVer()
    {
        byte[] msg = getHardwareVerCmdBytes;

        return doSerialTransaction(msg);
    }

    public byte[] getSoftwareVer()
    {
        byte[] msg = getSoftwareVerCmdBytes;

        return doSerialTransaction(msg);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A 3-byte bitmap of the state of all 24 inputs.  The lowest bit is on the last byte</returns>
    public byte[] getInputStates()
    {
        byte[] msg = getAllInputsCmdBytes;
        byte[] response = doSerialTransaction(msg);

        if (response != null)
            return new byte[] { response[5], response[6], response[7] };
        else
            return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A 3-byte bitmap of the state of all 24 outputs.  The lowest bit is on the last byte</returns>
    public byte[] getOutputStates()
    {
        byte[] msg = getOutputStatesCmdBytes;
        byte[] response = doSerialTransaction(msg);

        if (response != null)
            return new byte[] { response[5], response[6], response[7] };
        else
            return null;
    }

    /*public byte[] getSwitchStates()
    {
        byte[] msg = getSwitchStatesCmdBytes;
        byte[] response = doSerialTransaction(msg);

        if (response != null)
            return new byte[] { response[5], response[6], response[7] };
        else
            return null;
    }*/

    public bool setOutputState(byte outputNum, byte state)
    {
        byte[] msg = setOutputStateCmdBytes(outputNum, state);
        byte[] response = doSerialTransaction(msg);

        if (response != null)
            return response[4] == 0xB2 ? true : false;
        else
            return false;
    }

    public bool addPulsesToMeterOUT2(int numPulses)
    {
        byte[] msg = setMeter1PulsesCmdBytes(numPulses);
        byte[] resp = doSerialTransaction(msg);

        if (resp == null)
            return false;
        else
            return resp[4] == 0xB3 ? true : false;
    }

    public bool addPulsesToMeterOUT8(int numPulses)
    {
        byte[] msg = setMeter2PulsesCmdBytes(numPulses);

        byte[] resp = doSerialTransaction(msg);

        if (resp == null)
            return false;
        else
            return resp[4] == 0xB7 ? true : false;
    }

    public void clearErrors()
    {
        byte[] msg = { 0x58, 0x59, 0xC0, 0x01, 0xC1 };

        _serialPort.Write(msg, 0, msg.Length);
    }



    private void receivedMessageDispatcher(byte[] responseBytes)
    {
        byte commandByte = responseBytes[4];

        byte[] newArray = new byte[responseBytes.Length];
        for (int i = 0; i < responseBytes.Length; i++) newArray[i] = responseBytes[i];
            

        SerialMessageReceived?.Invoke(this, new SerialMessageReceivedEventArgs(responseBytes));

        if (commandByte == 0xA6)
        {
            //we received an io-board initiated message which indicates a falling edge on an input
            byte[] inputMap = { responseBytes[5], responseBytes[6], responseBytes[7] };

            InputChanged?.Invoke(this, new InputChangedEventArgs(inputMap));
        }
        else if(commandByte == 0xA5)
        {
            //A number of pulses have been received on input 18 (this functionality is usually used to detected coin in/out)
            //this is an annoying message that i have not figured out how to disable.  for now, do not use this functionality and ignore this message

            //byte numPulses = responseBytes[5];
            //CoinInOutPulsesReceived?.Invoke(this, new CoinInOutPulsesReceivedEventArgs(numPulses));
            System.Diagnostics.Debug.WriteLine("Not implemented...someone changed IN18!");

        }
        else if(commandByte == 0xD1)
        {
            // throw new Exception("The board didnt understand the command...");
            //the board wants us to retransmit the message that initiated this response
            //for now we'll just return null and let the caller handle the situation (e.x. by retrying the command)
            synchronizationReturnValues[lastSentCommandByte] = null;
            synchronizationEventDictionary[lastSentCommandByte].Set();
        }
        else if (synchronizationEventDictionary.ContainsKey(commandByte))
        {
            //this synchronizes the method that initiated this response with the return value
            synchronizationReturnValues[commandByte] = responseBytes;
            synchronizationEventDictionary[commandByte].Set();
        }
        else
        {
            //a valid message has been received but its handling has not been implemented yet
            System.Diagnostics.Debug.WriteLine("An undefined message has been received.");
        }

    }

    private ConcurrentQueue<byte> serialByteQueue = new ConcurrentQueue<byte>();

    //this should be run in its own thread
    private void doSerialParser()
    {
        while (true)
        {
            Thread.Sleep(25);

            while (_serialPort.BytesToRead > 0)
            {
                int temp = _serialPort.ReadByte();
                if (temp != -1)
                {
                    serialByteQueue.Enqueue((byte)temp);
                }
            }


            if (serialByteQueue.Count > 2)
            {
                //wait until we've received a header to do any processing

                if (serialByteQueue.ElementAt<byte>(0) == 0x58 && serialByteQueue.ElementAt<byte>(1) == 0x59)
                {
                    //we have received the beginning of a response

                    if (serialByteQueue.Count >= 5)
                    {
                        //we have possibly received a complete response, check length byte to confirm

                        byte lengthByteVal = serialByteQueue.ElementAt<byte>(3);
                        int totalMessageLength = 4 + lengthByteVal;

                        if (totalMessageLength <= serialByteQueue.Count)
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

                            if (checksum == responseBytes[2])
                            {
                                //valid message confirmed

                                receivedMessageDispatcher(responseBytes);
                            }
                            else
                            {
                                //uh oh...checksum was incorrect.  discard message.

                                System.Diagnostics.Debug.WriteLine("BAD CHECKSUM: " + BitConverter.ToString(responseBytes));
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
                        else
                        {
                            if(val != 0x58)
                                serialByteQueue.TryDequeue(out val);
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

    //note that this function will return null if there is a timeout or error
    private byte[] doSerialTransaction(byte[] msg)
    {
        byte cmdByte = msg[4];

        if (!synchronizationEventDictionary.ContainsKey(cmdByte))
            synchronizationEventDictionary[cmdByte] = new ManualResetEvent(false);
            
        synchronizationReturnValues[cmdByte] = null;

        _serialPort.Write(msg, 0, msg.Length);
        lastSentCommandByte = cmdByte;

        if (synchronizationEventDictionary[cmdByte].WaitOne(serialResponseTimeoutMilliseconds))
        {
            byte[] res = synchronizationReturnValues[cmdByte];
            synchronizationEventDictionary[cmdByte].Reset();

            return res;
        }
        else
        {
            //timeout, no response received
            System.Diagnostics.Debug.WriteLine("Command timed out: " + cmdByte);
            return null;
        }
    }


    //This synchronizes the serial processing thread with the datareceived event on the SerialPort.
    //It is redundant, and one could just check for BytesToRead in the serial processing thread
    private ManualResetEvent serialDataReceivedEvent;


    //These two dictionaries are this API's method of making the asynchronous request-response serial comms work with synchronous methods
    //The dictionary key is the command byte, which connects responses to the requests that caused them
    private Dictionary<byte, ManualResetEvent> synchronizationEventDictionary = new Dictionary<byte, ManualResetEvent>();
    private Dictionary<byte, byte[]> synchronizationReturnValues = new Dictionary<byte, byte[]>();

        

    private const int serialResponseTimeoutMilliseconds = 1000;

    public readonly byte[] getCommTestCmdBytes = { 0x58, 0x59, 0xF0, 0x01, 0xF1 };
    public readonly byte[] getSoftwareVerCmdBytes = { 0x58, 0x59, 0xF3, 0x01, 0xF2 };
    public readonly byte[] getHardwareVerCmdBytes = { 0x58, 0x59, 0xF2, 0x01, 0xF3 };

    public readonly byte[] getAllInputsCmdBytes = { 0x58, 0x59, 0xA0, 0x01, 0xA1 };
    public readonly byte[] getOutputStatesCmdBytes = { 0x58, 0x59, 0xA3, 0x01, 0xA2 };
    //public readonly byte[] getSwitchStatesCmdBytes = { 0x58, 0x59, 0xA3, 0x01, 0xA3 };

    public byte[] setOutputStateCmdBytes(byte outputNum, byte state) //state: 0=high impedance, 1=enabled
    {

        byte[] cmdBytes = { 0x58, 0x59, 0, 0x03, 0xB2, outputNum, state };

        for (int i = 3; i < cmdBytes.Length; i++)
            cmdBytes[2] ^= cmdBytes[i];


        return cmdBytes;
    }
    public byte[] setMeter1PulsesCmdBytes(int numPulses)  //Meter1 corresponds to OUT2
    {

        byte lowByteNumPulses = (byte)(numPulses & 0xFF);
        byte highByteNumPulses = (byte)(numPulses << 8);
        byte[] cmdBytes = {0x58, 0x59, 0, 0x05, 0xB3, highByteNumPulses,
                                lowByteNumPulses, 0x06, 0x06 }; //corresponds to 60mS pulse period

        for (int i = 3; i < cmdBytes.Length; i++)
            cmdBytes[2] ^= cmdBytes[i];


        return cmdBytes;
    }

    public byte[] setMeter2PulsesCmdBytes(int numPulses)  //Meter2 corresponds to OUT8
    {

        byte lowByteNumPulses = (byte)(numPulses & 0xFF);
        byte highByteNumPulses = (byte)(numPulses << 8);
        byte[] cmdBytes = {0x58, 0x59, 0, 0x05, 0xB7, highByteNumPulses,
                                lowByteNumPulses, 0x06, 0x06 }; //corresponds to 60mS pulse period

        for (int i = 3; i < cmdBytes.Length; i++)
            cmdBytes[2] ^= cmdBytes[i];


        return cmdBytes;
    }

}

