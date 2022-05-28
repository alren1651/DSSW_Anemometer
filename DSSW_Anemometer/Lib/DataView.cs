
using MaterialSkin;
using MaterialSkin.Controls;

namespace DSSW_Anemometer.Lib
{
    internal class DataView
    {
        /// <summary>
        /// DataView - System Log
        /// </summary>
        /// <param name="LstViewCtrl"></param>
        /// <param name="Txt_Serverity"></param>
        /// <param name="Txt_Message"></param>
        public static void SystemLog(MaterialListView LstViewCtrl, string Txt_Serverity, string Txt_Message)
        {
            ListViewItem lvi = new(Txt_Serverity);
            lvi.SubItems.Add(Common.NowDateTime());
            lvi.SubItems.Add(Txt_Message);

            if (Txt_Serverity.Equals("Error") == true)
                lvi.ForeColor = Color.Red;
            else if (Txt_Serverity.Equals("Warning") == true)
                lvi.ForeColor = Color.Orange;

            if (LstViewCtrl.InvokeRequired)
            {
                LstViewCtrl.BeginInvoke(new MethodInvoker(delegate
                {
                    LstViewCtrl.Items.Add(lvi);
                    LstViewCtrl.EnsureVisible(LstViewCtrl.Items.Count - 1);
                }));
            }
            else
            {
                LstViewCtrl.Items.Add(lvi);
                LstViewCtrl.EnsureVisible(LstViewCtrl.Items.Count - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TxtBxCtrl"></param>
        /// <param name="SetColor"></param>
        /// <param name="StrMsg"></param>
        private delegate void Delegate_GUI(MaterialMultiLineTextBox TxtBxCtrl, int SetColor, string StrMsg);
        public static void RecvDataLog(MaterialMultiLineTextBox TxtBxCtrl, int SetColor, string StrMsg)
        {
            if (TxtBxCtrl.InvokeRequired)
            {
                Delegate_GUI delegateMethod = new Delegate_GUI(RecvDataLog);
                TxtBxCtrl.Invoke(delegateMethod, new object[] { TxtBxCtrl, SetColor, StrMsg });
            }
            else
                //this.Lst_Log_GUI.Items.Add(paramString);
                Fn_Display_LogView(TxtBxCtrl, SetColor, StrMsg);
        }

        private static void Fn_Display_LogView(MaterialMultiLineTextBox TxtBxCtrl, int SetColor, string StrMsg)
        {
            try
            {
                if (TxtBxCtrl.Lines.Length > 24) TxtBxCtrl.Clear();

                TxtBxCtrl.SelectionCharOffset = 5;
                TxtBxCtrl.SelectionColor = Color.DimGray;

                TxtBxCtrl.AppendText($"[{Common.NowDateTime()}] ");

                if (SetColor == 0)
                    TxtBxCtrl.SelectionColor = Color.Black;
                else if (SetColor == 1)
                    TxtBxCtrl.SelectionColor = Color.Red;
                else if (SetColor == 2)
                    TxtBxCtrl.SelectionColor = Color.Blue;
                else if (SetColor == 3)
                    TxtBxCtrl.SelectionColor = Color.DarkOrange;

                TxtBxCtrl.AppendText(StrMsg);
                TxtBxCtrl.AppendText(Environment.NewLine);

                TxtBxCtrl.ScrollToCaret();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
