
using System.Text;

namespace DSSW_Anemometer.Lib
{
    internal class Common
    {
        ///==============================================================================================
        /// <summary>
        /// Set Delay Time
        /// </summary>
        /// <param name="MS"></param>
        /// <returns>Delay</returns>
        public static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Return string of NowTime
        /// </summary>
        /// <returns>NowTime</returns>
        public static string NowDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss.fff");
        }

        ///==============================================================================================
        /// <summary>
        /// 바이트 배열을 String으로 변환
        /// </summary>
        /// <param name="strByte"></param>
        /// <returns>string</returns>
        public static string ByteToString(byte[] strByte)
        {
            string str = Encoding.Default.GetString(strByte);
            return str;
        }

        /// <summary>
        /// String을 바이트 배열로 변환
        /// </summary>
        /// <param name="str"></param>
        /// <returns>byte array</returns>
        public static byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str);
            //byte[] StrByte = Encoding.Unicode.GetBytes(str);
            return StrByte;
        }

        /// <summary>
        /// Int를 바이트 배열로 변환
        /// </summary>
        /// <param name="Num"></param>
        /// <returns>byte array</returns>
        public static byte[] IntToByte(int Num)
        {
            //byte[] StrByte = Encoding.UTF8.GetBytes(str);
            byte[] intByte = BitConverter.GetBytes(Convert.ToInt16(Num));
            return intByte;
        }

        ///==============================================================================================
        /// <summary>
        /// Get All Controls
        /// </summary>
        /// <param name="containerControl"></param>
        /// <returns>all Controls Array</returns>
        public static Control[] GetAllControlsUsingRecursive(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            foreach (Control control in containerControl.Controls)
            {
                allControls.Add(control);

                if (control.Controls.Count > 0)
                    allControls.AddRange(GetAllControlsUsingRecursive(control));
            }

            return allControls.ToArray();
        }
    }
}
