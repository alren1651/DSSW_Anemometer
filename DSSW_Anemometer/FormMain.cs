
using MaterialSkin;
using MaterialSkin.Controls;

using System.IO.Ports;
using System.Timers;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        // Initialize MaterialSkinManager
        private readonly MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;

        // 센서 데이터 주기별 요청을 위한 타이머 선언
        System.Timers.Timer ReadTimer = new System.Timers.Timer();
        private int ReqCnt = 2;

        // HW 연결상태 저장
        private bool b_Open_Serial = false;
        private bool b_Open_ModBUS = false;
        private bool b_Open_MsgBoard1 = false;
        private bool b_Open_MsgBoard2 = false;


        public FormMain()
        {
            InitializeComponent();

            // Set this to false to disable backcolor enforcing on non-materialSkin components
            // This HAS to be set before the AddFormToManage()
            materialSkinManager.EnforceBackcolorOnAllComponents = true;

            // MaterialSkinManager properties
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);

            // 풍속계 센서 데이터 요청을 위한 타이머 생성
            ReadTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadTimer_Elapsed);
        }


        ///=====================================================================================================
        #region FormMain Fuction - Form Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            //---------------------------------------------------------------------//
            // Load Data for Combobox (Sensor & Message Boards)
            Fn_Load_SettingValues();

            //---------------------------------------------------------------------//
            string CurTime = DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss");
            DataView_Refer.Set_CurTime = CurTime;
            DataView_Sensor1.Set_CurTime = CurTime;
            DataView_Sensor2.Set_CurTime = CurTime;
            DataView_Sensor3.Set_CurTime = CurTime;

            /*
            //---------------------------------------------------------------------//
            // Set default values - combobox
            Fn_Set_SerialPort_List(Combo_Port_Serial);

            if (Combo_Port_Serial.FindString("COM11") < 0) Combo_Port_Serial.Items.Add("COM11");
            Combo_Port_Serial.SelectedIndex = Combo_Port_Serial.FindString("COM11");

            Combo_Baudrate_Serial.SelectedIndex = 1;

            //---------------------------------------------------------------------//
            Fn_Set_SerialPort_List(Combo_Port_ModBUS);

            if (Combo_Port_ModBUS.FindString("COM12") < 0) Combo_Port_ModBUS.Items.Add("COM12");
            Combo_Port_ModBUS.SelectedIndex = Combo_Port_ModBUS.FindString("COM12");

            Combo_Baudrate_ModBUS.SelectedIndex = 0;

            //---------------------------------------------------------------------//
            // 전광판 #1 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_MsgBoard1);

            if (Combo_Port_MsgBoard1.FindString("COM13") < 0) Combo_Port_MsgBoard1.Items.Add("COM13");
            Combo_Port_MsgBoard1.SelectedIndex = Combo_Port_MsgBoard1.FindString("COM13");

            Combo_Baudrate_MsgBoard1.SelectedIndex = 5;

            //---------------------------------------------------------------------//
            // 전광판 #2 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_MsgBoard2);

            if (Combo_Port_MsgBoard2.FindString("COM14") < 0) Combo_Port_MsgBoard2.Items.Add("COM14");
            Combo_Port_MsgBoard2.SelectedIndex = Combo_Port_MsgBoard2.FindString("COM14");

            Combo_Baudrate_MsgBoard2.SelectedIndex = 5;
            */

            //---------------------------------------------------------------------//
            // Cup Type 풍속계 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_Serial);

            if (Combo_Port_Serial.FindString("COM14") < 0) Combo_Port_Serial.Items.Add("COM14");
            Combo_Port_Serial.SelectedIndex = Combo_Port_Serial.FindString("COM14");

            Combo_Baudrate_Serial.SelectedIndex = 1;

            //---------------------------------------------------------------------//
            // Ultrasonic Type 풍속계 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_ModBUS);

            if (Combo_Port_ModBUS.FindString("COM11") < 0) Combo_Port_ModBUS.Items.Add("COM11");
            Combo_Port_ModBUS.SelectedIndex = Combo_Port_ModBUS.FindString("COM11");

            Combo_Baudrate_ModBUS.SelectedIndex = 0;

            //---------------------------------------------------------------------//
            // 전광판 #1 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_MsgBoard1);

            if (Combo_Port_MsgBoard1.FindString("COM12") < 0) Combo_Port_MsgBoard1.Items.Add("COM12");
            Combo_Port_MsgBoard1.SelectedIndex = Combo_Port_MsgBoard1.FindString("COM12");

            Combo_Baudrate_MsgBoard1.SelectedIndex = 5;

            //---------------------------------------------------------------------//
            // 전광판 #2 관련 Control 설정
            Fn_Set_SerialPort_List(Combo_Port_MsgBoard2);

            if (Combo_Port_MsgBoard2.FindString("COM13") < 0) Combo_Port_MsgBoard2.Items.Add("COM13");
            Combo_Port_MsgBoard2.SelectedIndex = Combo_Port_MsgBoard2.FindString("COM13");

            Combo_Baudrate_MsgBoard2.SelectedIndex = 5;

            //---------------------------------------------------------------------//
            Fn_ChangeState_IsAutoMode(true);
            Card_CtrlMode.Enabled = false;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // Force the last ListView column width to occupy all the available space.
            //LstView.Columns[LstView.Columns.Count - 1].Width = -2;
            //Lst_Log_System.Columns[^1].Width = -2;

            //---------------------------------------------------------------------//
            Common.Delay(100);
            DataView.SystemLog(Lst_Log_System, "Normal", $"Start System...");
            //---------------------------------------------------------------------//
            Common.Delay(200);
            DataView.SystemLog(Lst_Log_System, "Normal", $"Connect Database...");
            Fn_Open_MySQL();    // Database 연결
            //---------------------------------------------------------------------//
            Common.Delay(200);
            DataView.SystemLog(Lst_Log_System, "Normal", $"Connect WindMeter #1");
            Fn_Open_Serial();
            //---------------------------------------------------------------------//
            Common.Delay(200);
            DataView.SystemLog(Lst_Log_System, "Normal", $"Connect WindMeter #2/3/4");
            Fn_Open_ModBUS();
            //---------------------------------------------------------------------//
            Common.Delay(200);
            DataView.SystemLog(Lst_Log_System, "Normal", $"Connect Message Board #1/2");
            Fn_Open_MsgBoard1();
            Common.Delay(100);
            Fn_Open_MsgBoard2();

            //---------------------------------------------------------------------//
            Card_CtrlMode.Enabled = true;

            //---------------------------------------------------------------------//
            Common.Delay(100);
            if (/*b_Open_Serial && */b_Open_ModBUS && b_Open_MsgBoard1 && b_Open_MsgBoard2)
            {
                DataView.SystemLog(Lst_Log_System, "Normal", $"Ready!!");

                // --------------------------------------------------------------- //
                // 모든 시스템이 문제가 없을 때, 자동 실행 
                Common.Delay(500);
                Btn_Repeat_Start.PerformClick();
            }
            else
            {
                DataView.SystemLog(Lst_Log_System, "Error", $"System Fail/ Change Manual Mode & Check HW...");

                Switch_CtrlMode.CheckState = CheckState.Unchecked;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //---------------------------------------------------------------------//
            Fn_Close_MySQL();     // Close the Database

            //---------------------------------------------------------------------//
            Fn_Close_Serial();
            Fn_Close_ModBUS();

            Fn_Close_MsgBoard1();
            Fn_Close_MsgBoard2();

            //---------------------------------------------------------------------//
            Application.Exit();
        }

        private void Switch_CtrlMode_CheckedChanged(object sender, EventArgs e)
        {
            if (Switch_CtrlMode.Checked)
            {
                Switch_CtrlMode.Text = "Mode : Auto";
            }
            else
            {
                Switch_CtrlMode.Text = "Mode : Manual";
            }

            Fn_ChangeState_IsAutoMode(Switch_CtrlMode.Checked);
        }

        private void Fn_ChangeState_IsAutoMode(bool IsAuto)
        {
            Txt_TimeInterval.Enabled = IsAuto;
            Btn_Repeat_Start.Enabled = IsAuto;
            Btn_Repeat_Stop.Enabled = IsAuto;

            Card_Serial.Enabled = !IsAuto;
            Card_Ctrl_Serial.Visible = !IsAuto;

            Card_ModBUS.Enabled = !IsAuto;
            Card_Ctrl_ModBUS.Visible = !IsAuto;

            Card_MsgBoard.Enabled = !IsAuto;
        }

        #endregion


        ///=====================================================================================================
        #region FormMain Fuction - Button

        private void Btn_Repeat_Start_Click(object sender, EventArgs e)
        {
            // 스케쥴 간격 기본 5초
            double interv;

            if (Txt_TimeInterval.Text == "")
                interv = 5000;
            else
                interv = Convert.ToDouble(Txt_TimeInterval.Text);

            ReadTimer.Interval = interv;

            DataView.SystemLog(Lst_Log_System, "Normal", $"Start Auto-Repeat...");
            Fn_Serial_Read(1);
            //Fn_ModBUS_Read(1);

            // System.Timers.Timer
            ReqCnt = 2;
            ReadTimer.Start();

            //---------------------------------------------------------------------//
            Txt_TimeInterval.Enabled = false;
            Btn_Repeat_Start.Enabled = false;
            Btn_Repeat_Stop.Enabled = true;
        }

        private void Btn_Repeat_Stop_Click(object sender, EventArgs e)
        {
            DataView.SystemLog(Lst_Log_System, "Normal", $"Stop Auto-Repeat...");

            // System.Timers.Timer
            ReadTimer.Stop();

            //---------------------------------------------------------------------//
            Txt_TimeInterval.Enabled = true;
            Btn_Repeat_Start.Enabled = true;
            Btn_Repeat_Stop.Enabled = false;
        }

        // 픙속계 데이터 요청을 위한 타이머 설정
        delegate void TimerEvent();
        private void ReadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            BeginInvoke(new TimerEvent(ReadTimer_Work));
        }

        private void ReadTimer_Work()
        {
            if (ReqCnt == 5) ReqCnt = 1;

            // 설정된 TimeInterval 간격으로 풍속계 센서 풍속 & 풍향 확인
            if (ReqCnt == 1)
                Fn_Serial_Read(1);
            else if (ReqCnt == 2)
                Fn_ModBUS_Read(1);
            else if (ReqCnt == 3)
                Fn_ModBUS_Read(2);
            else if (ReqCnt == 4)
                Fn_ModBUS_Read(3);

            ReqCnt++;
        }

        #endregion


        ///=====================================================================================================
        #region Initialization

        private void Fn_Set_SerialPort_List(MaterialComboBox ComboBx)
        {
            try
            {
                ComboBx.Items.AddRange(SerialPort.GetPortNames());

                if (ComboBx.Items.Count > 0)
                    ComboBx.SelectedIndex = 0;
                else
                    DataView.SystemLog(Lst_Log_System, "Warning", $"Check the USB2Serial Device for ModBUS");
            }
            catch (Exception ex)
            {
                DataView.SystemLog(Lst_Log_System, "Error", $"Error: {ex.Message}");
            }
        }

        #endregion
    }
}