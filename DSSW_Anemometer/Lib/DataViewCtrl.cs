
using System.ComponentModel;

namespace DSSW_Anemometer.Lib
{
    public partial class DataViewCtrl : UserControl
    {
        public DataViewCtrl()
        {
            InitializeComponent();
        }

        [Category("TSTE Controls")]
        public string Set_Name
        {
            get { return this.Lbl_Name.Text; }
            set { this.Lbl_Name.Text = value; }
        }

        [Category("TSTE Controls")]
        public string Set_Type
        {
            get { return this.Lbl_Type.Text; }
            set { this.Lbl_Type.Text = value; }
        }

        public string Set_State
        {
            set
            {
                if (value == "Ready")
                    this.Pic_State.Image = Resources.Resource_DataView.Ready;
                else if (value == "Run")
                    this.Pic_State.Image = Resources.Resource_DataView.Run;
                else if (value == "Error")
                    this.Pic_State.Image = Resources.Resource_DataView.Error;
            }
        }

        public string Set_Value { set => Lbl_Value.Text = value; }
        public string Set_Dir { set => Lbl_Dir.Text = value; }

        public string Set_CurTime { set => Lbl_CurTime.Text = value; }
    }
}
