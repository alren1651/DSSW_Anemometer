
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO.Ports;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        public delegate void DataReceivedHandlerFunc_Serial(byte[] RecvData);
        public DataReceivedHandlerFunc_Serial DataReceivedHandler_Serial;

        private SerialPort COM_Serial;    // 풍속계 #1 연결을 위한 Serialport object
        private string str_PortSerial;

        ///=======================================================================================================
        #region IO.Serial Functions - WindMeter #1
        //========================================================================================================//
        // Open - Serial Port
        private void Fn_Open_Serial()
        {
            try
            {
                //string portName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "COM3" : "/dev/serial0";
                str_PortSerial = Combo_Port_Serial.SelectedItem.ToString();
                int i_baudRate = Convert.ToInt32(Combo_Baudrate_Serial.SelectedItem);

                // Intialize serial port
                COM_Serial = new SerialPort()
                {
                    PortName = str_PortSerial,
                    BaudRate = i_baudRate,

                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,

                    ReadTimeout = 4000,
                    WriteTimeout = 100,

                    ReadBufferSize = 10
                };
                COM_Serial.DataReceived += Serial_DataReceived;

                // Open serial port
                COM_Serial.Open();

                b_Open_Serial = true;
                DataView.RecvDataLog(Txt_Log_Serial, 0, $"{str_PortSerial} opened successfully");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_Serial, 1, $"Error: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Close - Serial Port
        private void Fn_Close_Serial()
        {
            try
            {
                if (COM_Serial != null && COM_Serial.IsOpen)
                {
                    // Destroy serial port
                    COM_Serial.Close();
                    COM_Serial.Dispose();

                    b_Open_Serial = false;
                    DataView.RecvDataLog(Txt_Log_Serial, 0, $"{str_PortSerial} closed successfully");
                }
                else
                    DataView.RecvDataLog(Txt_Log_Serial, 0, $"{str_PortSerial} is already closed.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_Serial, 1, $"Error: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Serial Data Received

        // 시리얼 통신시 쓰레기 값 자르기
        private int i_READtail_Serial = 0;
        private byte[] RecvBuff_Serial = new byte[10];

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytesBuffer = ReadSerialByteData_Serial();
                int iLen = bytesBuffer.Length;
                //string strBuffer = Encoding.ASCII.GetString(bytesBuffer);

                DataReceivedHandler_Serial?.Invoke(bytesBuffer);

                // Display Data
                //DoUpdate_GUI(Lst_Log_ModBUS, 2, $"Position: {COM_READtail} / Length: {iLen}");
                //DataView.RecvDataLog(Txt_Log_Serial, 2, $"Rx: {bytesBuffer.Length} >> {BitConverter.ToString(bytesBuffer).Replace("-", " ")}");

                Array.Copy(bytesBuffer, 0, RecvBuff_Serial, i_READtail_Serial, iLen);
                i_READtail_Serial += iLen;

                if (i_READtail_Serial > 6)
                {
                    // Display Data
                    DataView.RecvDataLog(Txt_Log_Serial, 3, $"Rx >> {BitConverter.ToString(RecvBuff_Serial).Replace("-", " ")}");

                    //--------------------------------------------------------------------------------------------------------//
                    // PostProcessing
                    Fn_PostProcessing_Serial(RecvBuff_Serial);

                    //--------------------------------------------------------------------------------------------------------//
                    Array.Clear(RecvBuff_Serial, 0, 10);
                    i_READtail_Serial = 0;
                }

                if (i_READtail_Serial > 7) i_READtail_Serial = 0;
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_Serial, 1, $"Error: {ex.Message}");
            }
        }

        private byte[] ReadSerialByteData_Serial()
        {
            byte[] bytesBuffer = new byte[COM_Serial.BytesToRead];
            int bufferOffset = 0;
            int bytesToRead = COM_Serial.BytesToRead;

            while (bytesToRead > 0)
            {
                try
                {
                    int readBytes = COM_Serial.Read(bytesBuffer, bufferOffset, bytesToRead - bufferOffset);
                    bytesToRead -= readBytes;
                    bufferOffset += readBytes;
                }
                catch (TimeoutException ex)
                {
                    DataView.RecvDataLog(Txt_Log_Serial, 1, $"Error: {ex.Message}");
                }
            }

            return bytesBuffer;
        }

        //========================================================================================================//
        // Display Data & Send to MsgBoard, Insert DB
        private void Fn_PostProcessing_Serial(byte[] RecvBuff)
        {
            //--------------------------------------------------------------------------------------------------------//
            // Parsing Data - Wind Speed
            byte[] b_Spd = new byte[2];
            b_Spd[0] = RecvBuff[3];
            b_Spd[1] = RecvBuff[4];
            // If the system architecture is little-endian (that is, little end first), reverse the byte array.
            if (BitConverter.IsLittleEndian) Array.Reverse(b_Spd);
            int i_WindSpd = BitConverter.ToInt16(b_Spd, 0) / 2;

            //--------------------------------------------------------------------------------------------------------//
            // Get Wind Speed
            string str_WindSpd = (Convert.ToDouble(i_WindSpd) / 10).ToString("F1");

            //--------------------------------------------------------------------------------------------------------//
            // Display MsgBoard
            string CurTime = DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss");
            DataView_Refer.Set_State = "Ready";

            DataView_Refer.Set_Value = str_WindSpd;
            DataView_Refer.Set_Dir = "-";
            DataView_Refer.Set_CurTime = CurTime;
            DataView_Refer.Set_State = "Run";

            //--------------------------------------------------------------------------------------------------------//
            // Insert Database
            InsertDB(4, str_WindSpd);
        }

        //========================================================================================================//
        // Serial Data Send
        private void Fn_Send_Serial(byte[] sendData)
        {
            try
            {
                // Display Data
                DataView.RecvDataLog(Txt_Log_Serial, 2, $"Tx >> {BitConverter.ToString(sendData).Replace("-", " ")}");

                // Send Packet
                if (COM_Serial != null && COM_Serial.IsOpen)
                    COM_Serial.Write(sendData, 0, sendData.Length);
                else
                    DataView.RecvDataLog(Txt_Log_Serial, 1, $"SerialPort: not Open.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_Serial, 1, $"Error: {ex.Message}");
            }
        }
        #endregion


        ///=====================================================================================================
        #region Serial Commands

        //========================================================================================================//
        // 풍속계 센서 풍속 & 풍향 확인
        private void Fn_Serial_Read(byte DeviceId)
        {
            // WTF-B500 풍속계 데이터 요청 ModBUS 명령 : 01 03 00 22 00 01 24 00

            // Make Send Packet
            byte[] b_Cmd = new byte[8];
            b_Cmd[0] = DeviceId;    // ID
            b_Cmd[1] = 0x03;        // Function Code
            b_Cmd[2] = 0x00;        // Register = 0x0022
            b_Cmd[3] = 0x22;
            b_Cmd[4] = 0x00;        // Data Length = 0x0001
            b_Cmd[5] = 0x01;
            b_Cmd[6] = 0x24;        // CRC
            b_Cmd[7] = 0x00;

            // Send Data
            Fn_Send_Serial(b_Cmd);
        }

        #endregion


        private void Btn_Connect_Serial_Click(object sender, EventArgs e)
        {
            // Connect the Serial port
            Fn_Open_Serial();

            Btn_Connect_Serial.Enabled = !b_Open_Serial;
            Btn_Close_Serial.Enabled = b_Open_Serial;
        }

        private void Btn_Close_Serial_Click(object sender, EventArgs e)
        {
            // Close the Serial port
            Fn_Close_Serial();

            Btn_Connect_Serial.Enabled = !b_Open_Serial;
            Btn_Close_Serial.Enabled = b_Open_Serial;
        }

        private void Btn_Read_Serial_Click(object sender, EventArgs e)
        {
            byte DeviceId = Convert.ToByte(Txt_DeviceID_Serial.Text);

            // 풍속계 센서 풍속 & 풍향 확인
            Fn_Serial_Read(DeviceId);
        }
    }
}