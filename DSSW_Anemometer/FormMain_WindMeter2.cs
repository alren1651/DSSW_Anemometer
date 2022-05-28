
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO.Ports;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        public delegate void DataReceivedHandlerFunc_ModBUS(byte[] RecvData);
        public DataReceivedHandlerFunc_ModBUS DataReceivedHandler_ModBUS;

        private SerialPort COM_ModBUS;    // 풍속계 #2~4 연결을 위한 ModBUSport object
        private string str_PortModBUS;

        ///=======================================================================================================
        #region IO.Serial Functions - WindMeter #2

        //========================================================================================================//
        // Open - ModBUS Port
        private void Fn_Open_ModBUS()
        {
            try
            {
                //string portName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "COM3" : "/dev/ModBUS0";
                str_PortModBUS = Combo_Port_ModBUS.SelectedItem.ToString();
                int i_baudRate = Convert.ToInt32(Combo_Baudrate_ModBUS.SelectedItem);

                // Intialize ModBUS port
                COM_ModBUS = new SerialPort()
                {
                    PortName = str_PortModBUS,
                    BaudRate = i_baudRate,

                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,

                    ReadTimeout = 4000,
                    WriteTimeout = 100,

                    ReadBufferSize = 10
                };
                COM_ModBUS.DataReceived += ModBUS_DataReceived;

                // Open ModBUS port
                COM_ModBUS.Open();

                b_Open_ModBUS = true;
                DataView.RecvDataLog(Txt_Log_ModBUS, 0, $"{str_PortModBUS} opened successfully");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"Error: {ex.Message}");
            }
        }

        //========================================================================================================//
        // Close - ModBUS Port
        private void Fn_Close_ModBUS()
        {
            try
            {
                if (COM_ModBUS != null && COM_ModBUS.IsOpen)
                {
                    // Destroy ModBUS port
                    COM_ModBUS.Close();
                    COM_ModBUS.Dispose();

                    b_Open_ModBUS = false;
                    DataView.RecvDataLog(Txt_Log_ModBUS, 0, $"{str_PortModBUS} closed successfully");
                }
                else
                    DataView.RecvDataLog(Txt_Log_ModBUS, 0, $"{str_PortModBUS} is already closed.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"Error: {ex.Message}");
            }
        }

        //========================================================================================================//
        // ModBUS Data Received

        // 시리얼 통신시 쓰레기 값 자르기
        private int i_READtail_ModBUS = 0, i_LenTx_ModBus;
        private byte[] RecvBuff_ModBUS = new byte[10];

        private void ModBUS_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytesBuffer = ReadModBUSByteData_ModBUS();
                int iLen = bytesBuffer.Length;
                //string strBuffer = Encoding.ASCII.GetString(bytesBuffer);

                DataReceivedHandler_ModBUS?.Invoke(bytesBuffer);

                // Display Data
                //DoUpdate_GUI(Lst_Log_ModBUS, 3, $"Position: {COM_READtail} / Length: {iLen}");
                //DataView.RecvDataLog(Txt_Log_ModBUS, 3, $"Rx: {bytesBuffer.Length} >> {BitConverter.ToString(bytesBuffer).Replace("-", " ")}");

                Array.Copy(bytesBuffer, 0, RecvBuff_ModBUS, i_READtail_ModBUS, iLen);
                i_READtail_ModBUS += iLen;

                // if Tx:Device ID
                if (i_LenTx_ModBus == 1 && i_READtail_ModBUS > 6)
                {
                    // Display Data
                    DataView.RecvDataLog(Txt_Log_ModBUS, 3, $"Rx >> {BitConverter.ToString(RecvBuff_ModBUS).Replace("-", " ")}");

                    Array.Clear(RecvBuff_ModBUS, 0, 10);
                    i_READtail_ModBUS = 0;
                }

                // if Tx:Read Sensor Values
                if (i_LenTx_ModBus == 2 && i_READtail_ModBUS > 8)
                {
                    //--------------------------------------------------------------------------------------------------------//
                    // PostProcessing
                    Fn_PostProcessing(RecvBuff_ModBUS);

                    //--------------------------------------------------------------------------------------------------------//
                    Array.Clear(RecvBuff_ModBUS, 0, 10);
                    i_READtail_ModBUS = 0;
                }

                if (i_READtail_ModBUS > 7) i_READtail_ModBUS = 0;
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"Error: {ex.Message}");
            }
        }

        private byte[] ReadModBUSByteData_ModBUS()
        {
            byte[] bytesBuffer = new byte[COM_ModBUS.BytesToRead];
            int bufferOffset = 0;
            int bytesToRead = COM_ModBUS.BytesToRead;

            while (bytesToRead > 0)
            {
                try
                {
                    int readBytes = COM_ModBUS.Read(bytesBuffer, bufferOffset, bytesToRead - bufferOffset);
                    bytesToRead -= readBytes;
                    bufferOffset += readBytes;
                }
                catch (TimeoutException ex)
                {
                    DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"Error: {ex.Message}");
                }
            }

            return bytesBuffer;
        }

        //========================================================================================================//
        // Display Data & Send to MsgBoard, Insert DB
        private void Fn_PostProcessing(byte[] RecvBuff)
        {
            //--------------------------------------------------------------------------------------------------------//
            // Parsing Data - Wind Speed
            byte[] b_Spd = new byte[2];
            b_Spd[0] = RecvBuff[3];
            b_Spd[1] = RecvBuff[4];
            // If the system architecture is little-endian (that is, little end first), reverse the byte array.
            if (BitConverter.IsLittleEndian) Array.Reverse(b_Spd);

            int i_WindSpd = BitConverter.ToInt16(b_Spd, 0);
            //--------------------------------------------------------------------------------------------------------//
            // Parsing Data - Wind Direction
            byte[] b_Dir = new byte[2];
            b_Dir[0] = RecvBuff[5];
            b_Dir[1] = RecvBuff[6];
            // If the system architecture is little-endian (that is, little end first), reverse the byte array.
            if (BitConverter.IsLittleEndian) Array.Reverse(b_Dir);

            //--------------------------------------------------------------------------------------------------------//
            // Get Device ID, Wind Speed & Dir
            int i_DeviceID = Convert.ToInt16(RecvBuff[0]);
            string str_WindSpd = (Convert.ToDouble(i_WindSpd) / 100).ToString("F1");
            int i_WindDir = BitConverter.ToInt16(b_Dir, 0);

            //--------------------------------------------------------------------------------------------------------//
            // Display Data
            DataView.RecvDataLog(Txt_Log_ModBUS, 3, $"Rx >> Device_{i_DeviceID} | Speed: {str_WindSpd} / Dir: {i_WindDir}");

            //--------------------------------------------------------------------------------------------------------//
            // Send & Display MsgBoard
            string strCmd = Fn_CmdMsg(str_WindSpd);
            string CurTime = DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss");

            DataView_Sensor1.Set_State = "Ready";
            DataView_Sensor2.Set_State = "Ready";
            DataView_Sensor3.Set_State = "Ready";

            if (i_DeviceID == 1)
            {
                Fn_Send_MsgBoard1(strCmd);

                DataView_Sensor1.Set_Value = str_WindSpd;
                DataView_Sensor1.Set_Dir = i_WindDir.ToString();
                DataView_Sensor1.Set_CurTime = CurTime;
                DataView_Sensor1.Set_State = "Run";
            }
            else if (i_DeviceID == 2)
            {
                Fn_Send_MsgBoard2(strCmd);

                DataView_Sensor2.Set_Value = str_WindSpd;
                DataView_Sensor2.Set_Dir = i_WindDir.ToString();
                DataView_Sensor2.Set_CurTime = CurTime;
                DataView_Sensor2.Set_State = "Run";
            }
            else if (i_DeviceID == 3)
            {
                //Fn_Send_MsgBoard3(strCmd);

                DataView_Sensor3.Set_Value = str_WindSpd;
                DataView_Sensor3.Set_Dir = i_WindDir.ToString();
                DataView_Sensor3.Set_CurTime = CurTime;
                DataView_Sensor3.Set_State = "Run";
            }

            //--------------------------------------------------------------------------------------------------------//
            // Insert Database
            InsertDB(i_DeviceID, str_WindSpd);
        }

        private string Fn_CmdMsg(string Value)
        {
            double d_WindSpd = Convert.ToDouble(Value);

            // ------------------------------------------------------------------------- //
            // 풍속에 따라 음성경고문 선택
            int iCmd;
            if (0 <= d_WindSpd && d_WindSpd <= 9.9)
            {
                // Normal
                iCmd = 22;
            }
            else if (10.0 <= d_WindSpd && d_WindSpd <= 12.9)
            {
                // Attention
                iCmd = 23;
            }
            else if (13.0 <= d_WindSpd && d_WindSpd <= 14.9)
            {
                // Warning
                iCmd = 24;
            }
            else
            {
                // Alart
                iCmd = 25;
            }

            //string strSend = "LNE=1,YSZ=1,RST=1,TXT=$f00$c00" + value.ToString("F1") + " m/s,LNE=2,YSZ=1,TXT=$f00$c01●●●";
            string strSend = d_WindSpd.ToString("F1") + "/" + iCmd.ToString() + Environment.NewLine;

            return strSend;
        }

        //========================================================================================================//
        // ModBUS Data Send
        private void Fn_Send_ModBUS(byte[] sendData)
        {
            try
            {
                // Display Data
                DataView.RecvDataLog(Txt_Log_ModBUS, 2, $"Tx >> {BitConverter.ToString(sendData).Replace("-", " ")}");

                // Send Packet
                if (COM_ModBUS != null && COM_ModBUS.IsOpen)
                    COM_ModBUS.Write(sendData, 0, sendData.Length);
                else
                    DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"ModBUSPort: not Open.");
            }
            catch (Exception ex)
            {
                DataView.RecvDataLog(Txt_Log_ModBUS, 1, $"Error: {ex.Message}");
            }
        }

        #endregion

        ///=====================================================================================================
        #region ModBUS Commands

        //========================================================================================================//
        // ModBUS 장치 ID 확인
        private void Fn_ModBUS_CheckDeviceID(byte DeviceId)
        {
            i_READtail_ModBUS = 0; i_LenTx_ModBus = 1;

            // Make Send Packet
            byte[] b_Cmd = new byte[8];
            b_Cmd[0] = DeviceId;    // ID
            b_Cmd[1] = 0x03;        // Function Code
            b_Cmd[2] = 0x07;        // Register = 0x07D0
            b_Cmd[3] = 0xD0;
            b_Cmd[4] = 0x00;        // Data Length = 0x0001
            b_Cmd[5] = 0x01;
            b_Cmd[6] = 0x84;        // CRC
            b_Cmd[7] = 0x87;
            // --------------------------------------------- //
            if (DeviceId == 0x01)
                b_Cmd[7] = 0x87;
            else if (DeviceId == 0x02)
                b_Cmd[7] = 0xB4;

            // Send Data
            Fn_Send_ModBUS(b_Cmd);
        }

        //========================================================================================================//
        // 풍속계 센서 풍속 & 풍향 확인
        private void Fn_ModBUS_Read(byte DeviceId)
        {
            i_READtail_ModBUS = 0; i_LenTx_ModBus = 2;

            // Device #1 : 01 03 00 00 00 02 C4 0B
            // Device #2 : 02 03 00 00 00 02 C4 38
            // Device #3 : 03 03 00 00 00 02 C5 E9

            // Make Send Packet
            byte[] b_Cmd = new byte[8];
            b_Cmd[0] = DeviceId;    // ID
            b_Cmd[1] = 0x03;        // Function Code
            b_Cmd[2] = 0x00;        // Register = 0x0000
            b_Cmd[3] = 0x00;
            b_Cmd[4] = 0x00;        // Data Length = 0x0002
            b_Cmd[5] = 0x02;
            b_Cmd[6] = 0xC4;        // CRC
            b_Cmd[7] = 0x0B;
            // --------------------------------------------- //
            if (DeviceId == 0x01)
                b_Cmd[7] = 0x0B;
            else if (DeviceId == 0x02)
                b_Cmd[7] = 0x38;
            else if (DeviceId == 0x03)
            {
                b_Cmd[6] = 0xC5;
                b_Cmd[7] = 0xE9;
            }

            // Send Data
            Fn_Send_ModBUS(b_Cmd);
        }

        #endregion


        ///=====================================================================================================
        private void Btn_Connect_ModBUS_Click(object sender, EventArgs e)
        {
            // Connect the ModBUS port
            Fn_Open_ModBUS();

            Btn_Connect_ModBUS.Enabled = !b_Open_ModBUS;
            Btn_Close_ModBUS.Enabled = b_Open_ModBUS;
        }

        private void Btn_Close_ModBUS_Click(object sender, EventArgs e)
        {
            // Close the ModBUS port
            Fn_Close_ModBUS();

            Btn_Connect_ModBUS.Enabled = !b_Open_ModBUS;
            Btn_Close_ModBUS.Enabled = b_Open_ModBUS;
        }

        private void Btn_ChkID_ModBUS_Click(object sender, EventArgs e)
        {
            byte DeviceId = Convert.ToByte(Txt_DeviceID_ModBUS.Text);

            // ModBUS 장치 ID 확인
            Fn_ModBUS_CheckDeviceID(DeviceId);
        }

        private void Btn_Read_ModBUS_Click(object sender, EventArgs e)
        {
            byte DeviceId = Convert.ToByte(Txt_DeviceID_ModBUS.Text);

            // 풍속계 센서 풍속 & 풍향 확인
            Fn_ModBUS_Read(DeviceId);
        }
    }
}