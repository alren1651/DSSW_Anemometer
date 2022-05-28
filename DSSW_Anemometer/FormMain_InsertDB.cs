
using MaterialSkin;
using MaterialSkin.Controls;

using System.Data;
using MySql.Data.MySqlClient;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        // MariaDB 연결을 위한 Sql 객체
        private MySqlConnection MySQL_Conn;
        private MySqlCommand MySql_Cmd;
        // MariaDB 연결을 위한 Command 문자열
        private readonly string ConnStr = @"Server=127.0.0.1; Port=3306; Database=samwoo; uid=wind; pwd=!wind1234;";
        private readonly string InsertSQL = @"INSERT INTO `samwoo`.`tb_wind` (`id`, `indt`, `value`) VALUES (@ID,@Time,@Value)";

        // ------------------------------------------------------------------------
        /// <summary>Connection MySQL</summary>
        public void Fn_Open_MySQL()
        {
            try
            {
                // MySQL 연결
                MySQL_Conn = new MySqlConnection(ConnStr);
                MySQL_Conn.Open();

                Lib.DataView.SystemLog(Lst_Log_System, "Normal", $"Connect Database successfully!");
            }
            catch (Exception ex)
            {
                Lib.DataView.SystemLog(Lst_Log_System, "Error", $"DB Error: {ex.Message}");
            }
        }

        /// <summary>Destroy master instance</summary>
        public void Fn_Close_MySQL()
        {
            if (MySQL_Conn != null && MySQL_Conn.State == ConnectionState.Open)
            {
                MySQL_Conn.Close();
                MySQL_Conn.Dispose();

                //DoStatus_GUI($"[{NowDateTime()}] DB 연결 해제...");
            }
        }

        // INSERT 처리
        public void InsertDB(int DeviceID, string WindSpd)
        {
            if (MySQL_Conn != null && MySQL_Conn.State == ConnectionState.Open)
            {
                try
                {
                    MySql_Cmd = new MySqlCommand(InsertSQL, MySQL_Conn);

                    MySql_Cmd.Parameters.Clear();
                    MySql_Cmd.Parameters.AddWithValue("@ID", DeviceID);
                    MySql_Cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    MySql_Cmd.Parameters.AddWithValue("@Value", WindSpd);

                    MySql_Cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Lib.DataView.SystemLog(Lst_Log_System, "Error", $"DB Error: {ex.Message}");
                }
            }
        }
    }
}