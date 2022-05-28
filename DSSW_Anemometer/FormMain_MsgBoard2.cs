
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO.Ports;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        public delegate void DataReceivedHandlerFunc_Msg2(byte[] RecvData);
        public DataReceivedHandlerFunc_Msg2 DataReceivedHandler_MsgBoard2;

        private SerialPort COM_MsgBoard2;    // 전광판 #2 연결을 위한 Serialport object
        private string str_PortMsgBoard2;

        ///=======================================================================================================
        #region IO.Serial Functions - Message Board #2

        //========================================================================================================//
        // Open - Serial Port
        private void Fn_Open_MsgBoard2()
        {
            try
            {
                //string portName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "COM3" : "/dev/serial0";
                str_PortMsgBoard2 = Combo_Port_MsgBoard2.SelectedItem.ToString();
                int i_baudRate = Convert.ToInt32(Combo_Baudrate_MsgBoard2.SelectedItem);

                // Intialize serial port
                COM_MsgBoard2 = new SerialPort()
                {
                    PortName = str_PortMsgBoard2,
                    BaudRate = i_baudRate,

                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,

                    ReadTimeout = 4000,
                    WriteTimeout = 100,

                    ReadBufferSize = 10
                };
                COM_MsgBoard2.DataReceived += MsgBoard2_DataReceived;

                // Open serial port
                COM_MsgBoard2.Open();

                b_Open_MsgBoard2 = true;
                DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard2} opened successfully");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Close - Serial Port
        private void Fn_Close_MsgBoard2()
        {
            try
            {
                if (COM_MsgBoard2 != null && COM_MsgBoard2.IsOpen)
                {
                    // Destroy serial port
                    COM_MsgBoard2.Close();
                    COM_MsgBoard2.Dispose();

                    b_Open_MsgBoard2 = false;
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard2} closed successfully");
                }
                else
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 0, $"{str_PortMsgBoard2} is already closed.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Serial Data Received

        // 시리얼 통신시 쓰레기 값 자르기
        private int i_READtail_MsgBoard2 = 0;
        private byte[] RecvBuff_MsgBoard2 = new byte[10];

        private void MsgBoard2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytesBuffer = ReadSerialByteData_MsgBoard2();
                int iLen = bytesBuffer.Length;
                //string strBuffer = Encoding.ASCII.GetString(bytesBuffer);

                DataReceivedHandler_MsgBoard2?.Invoke(bytesBuffer);

                // Display Data
                //DoUpdate_GUI(Lst_Log_ModBUS, 3, $"Position: {COM_READtail} / Length: {iLen}");
                //DataView.RecvDataLog(Txt_Log_MsgBoard, 3, $"Rx: {bytesBuffer.Length} >> {BitConverter.ToString(bytesBuffer).Replace("-", " ")}");

                Array.Copy(bytesBuffer, 0, RecvBuff_MsgBoard2, i_READtail_MsgBoard2, iLen);
                i_READtail_MsgBoard2 += iLen;

                if (i_READtail_MsgBoard2 > 6)
                {
                    // Display Data
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 3, $"Rx >> #2 : {BitConverter.ToString(RecvBuff_MsgBoard2).Replace("-", " ")}");

                    Array.Clear(RecvBuff_MsgBoard2, 0, 10);
                    i_READtail_MsgBoard2 = 0;
                }

                if (i_READtail_MsgBoard2 > 7) i_READtail_MsgBoard2 = 0;
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: {ex.Message}");
            }
        }

        private byte[] ReadSerialByteData_MsgBoard2()
        {
            byte[] bytesBuffer = new byte[COM_MsgBoard2.BytesToRead];
            int bufferOffset = 0;
            int bytesToRead = COM_MsgBoard2.BytesToRead;

            while (bytesToRead > 0)
            {
                try
                {
                    int readBytes = COM_MsgBoard2.Read(bytesBuffer, bufferOffset, bytesToRead - bufferOffset);
                    bytesToRead -= readBytes;
                    bufferOffset += readBytes;
                }
                catch (TimeoutException ex)
                {
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: {ex.Message}");
                }
            }

            return bytesBuffer;
        }

        //========================================================================================================//
        // Serial Data Send
        private void Fn_Send_MsgBoard2(string strCmd)
        {
            try
            {
                byte[] sendData = Common.StringToByte(strCmd);

                // Display Data
                DataView.RecvDataLog(Txt_Log_MsgBoard, 2, $"Tx >> #2 : {BitConverter.ToString(sendData).Replace("-", " ")}");

                // Send Packet
                if (COM_MsgBoard2 != null && COM_MsgBoard2.IsOpen)
                    COM_MsgBoard2.Write(sendData, 0, sendData.Length);
                else
                    DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: SerialPort not Open.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_MsgBoard, 1, $"Error - Message Board #2: {ex.Message}");
            }
        }
        #endregion


        private void Btn_Connect_MsgBoard2_Click(object sender, EventArgs e)
        {
            // Connect the Serial port for MsgBoard
            Fn_Open_MsgBoard2();

            Btn_Connect_MsgBoard2.Enabled = !b_Open_MsgBoard2;
            Btn_Close_MsgBoard2.Enabled = b_Open_MsgBoard2;
        }

        private void Btn_Close_MsgBoard2_Click(object sender, EventArgs e)
        {
            // Close the Serial port for MsgBoard
            Fn_Close_MsgBoard2();

            Btn_Connect_MsgBoard2.Enabled = !b_Open_MsgBoard2;
            Btn_Close_MsgBoard2.Enabled = b_Open_MsgBoard2;
        }
    }
}