
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO.Ports;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        public delegate void DataReceivedHandlerFunc_Msg1(byte[] RecvData);
        public DataReceivedHandlerFunc_Msg1 DataReceivedHandler_MsgBoard1;

        private SerialPort COM_MsgBoard1;    // 전광판 #1 연결을 위한 Serialport object
        private string str_PortMsgBoard1;

        ///=======================================================================================================
        #region IO.Serial Functions - Message Board #1

        //========================================================================================================//
        // Open - Serial Port
        private void Fn_Open_MsgBoard1()
        {
            try
            {
                //string portName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "COM3" : "/dev/serial0";
                str_PortMsgBoard1 = Combo_Port_MsgBoard1.SelectedItem.ToString();
                int i_baudRate = Convert.ToInt32(Combo_Baudrate_MsgBoard1.SelectedItem);

                // Intialize serial port
                COM_MsgBoard1 = new SerialPort()
                {
                    PortName = str_PortMsgBoard1,
                    BaudRate = i_baudRate,

                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,

                    ReadTimeout = 4000,
                    WriteTimeout = 100,

                    ReadBufferSize = 10
                };
                COM_MsgBoard1.DataReceived += MsgBoard1_DataReceived;

                // Open serial port
                COM_MsgBoard1.Open();

                b_Open_MsgBoard1 = true;
                DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard1} opened successfully");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Close - Serial Port
        private void Fn_Close_MsgBoard1()
        {
            try
            {
                if (COM_MsgBoard1 != null && COM_MsgBoard1.IsOpen)
                {
                    // Destroy serial port
                    COM_MsgBoard1.Close();
                    COM_MsgBoard1.Dispose();

                    b_Open_MsgBoard1 = false;
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard1} closed successfully");
                }
                else
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard1} is already closed.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Serial Data Received

        // 시리얼 통신시 쓰레기 값 자르기
        private int i_READtail_MsgBoard1 = 0;
        private byte[] RecvBuff_MsgBoard1 = new byte[10];

        private void MsgBoard1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytesBuffer = ReadSerialByteData_MsgBoard1();
                int iLen = bytesBuffer.Length;
                //string strBuffer = Encoding.ASCII.GetString(bytesBuffer);

                DataReceivedHandler_MsgBoard1?.Invoke(bytesBuffer);

                // Display Data
                //DoUpdate_GUI(Lst_Log_ModBUS, 2, $"Position: {COM_READtail} / Length: {iLen}");
                //DataView.RecvDataLog(Txt_Log_MsgBoard, 3, $"Rx: {bytesBuffer.Length} >> {BitConverter.ToString(bytesBuffer).Replace("-", " ")}");

                Array.Copy(bytesBuffer, 0, RecvBuff_MsgBoard1, i_READtail_MsgBoard1, iLen);
                i_READtail_MsgBoard1 += iLen;

                if (i_READtail_MsgBoard1 > 6)
                {
                    // Display Data
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 3, $"Rx >> #1 : {BitConverter.ToString(RecvBuff_MsgBoard1).Replace("-", " ")}");

                    Array.Clear(RecvBuff_MsgBoard1, 0, 10);
                    i_READtail_MsgBoard1 = 0;
                }

                if (i_READtail_MsgBoard1 > 7) i_READtail_MsgBoard1 = 0;
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: {ex.Message}");
            }
        }

        private byte[] ReadSerialByteData_MsgBoard1()
        {
            byte[] bytesBuffer = new byte[COM_MsgBoard1.BytesToRead];
            int bufferOffset = 0;
            int bytesToRead = COM_MsgBoard1.BytesToRead;

            while (bytesToRead > 0)
            {
                try
                {
                    int readBytes = COM_MsgBoard1.Read(bytesBuffer, bufferOffset, bytesToRead - bufferOffset);
                    bytesToRead -= readBytes;
                    bufferOffset += readBytes;
                }
                catch (TimeoutException ex)
                {
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: {ex.Message}");
                }
            }

            return bytesBuffer;
        }

        //========================================================================================================//
        // Serial Data Send
        private void Fn_Send_MsgBoard1(string strCmd)
        {
            try
            {
                byte[] sendData = Common.StringToByte(strCmd);

                // Display Data
                DataView.RecvDataLog(Txt_Log_MsgBoard, 2, $"Tx >> #1 : {BitConverter.ToString(sendData).Replace("-", " ")}");

                // Send Packet
                if (COM_MsgBoard1 != null && COM_MsgBoard1.IsOpen)
                    COM_MsgBoard1.Write(sendData, 0, sendData.Length);
                else
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: SerialPort not Open.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #1: {ex.Message}");
            }
        }

        #endregion


        private void Btn_Connect_MsgBoard1_Click(object sender, EventArgs e)
        {
            // Connect the Serial port for MsgBoard
            Fn_Open_MsgBoard1();

            Btn_Connect_MsgBoard1.Enabled = !b_Open_MsgBoard1;
            Btn_Close_MsgBoard1.Enabled = b_Open_MsgBoard1;
        }

        private void Btn_Close_MsgBoard1_Click(object sender, EventArgs e)
        {
            // Close the Serial port for MsgBoard
            Fn_Close_MsgBoard1();

            Btn_Connect_MsgBoard1.Enabled = !b_Open_MsgBoard1;
            Btn_Close_MsgBoard1.Enabled = b_Open_MsgBoard1;
        }
    }
}