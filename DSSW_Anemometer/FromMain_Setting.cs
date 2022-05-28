
using MaterialSkin;
using MaterialSkin.Controls;

using DSSW_Anemometer.Lib;

namespace DSSW_Anemometer
{
    public partial class FormMain : MaterialForm
    {
        private int colorSchemeIndex;

        ///=======================================================================================================
        #region Setting - Theme / Color

        private void Btn_Set_ChgColor_Click(object sender, EventArgs e)
        {
            colorSchemeIndex++;

            if (colorSchemeIndex > 2) colorSchemeIndex = 0;
            Fn_UpdateColor();
        }

        private void Btn_Set_ChgTheme_Click(object sender, EventArgs e)
        {
            materialSkinManager.Theme = materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? MaterialSkinManager.Themes.LIGHT : MaterialSkinManager.Themes.DARK;
            Fn_UpdateColor();
        }

        private void Switch_MainMenu1_CheckedChanged(object sender, EventArgs e)
        {
            DrawerUseColors = Switch_MainMenu1.Checked;
        }

        private void Switch_MainMenu2_CheckedChanged(object sender, EventArgs e)
        {
            DrawerHighlightWithAccent = Switch_MainMenu2.Checked;
        }

        private void Switch_MainMenu3_CheckedChanged(object sender, EventArgs e)
        {
            DrawerBackgroundWithAccent = Switch_MainMenu3.Checked;
        }

        private void Switch_MainMenu4_CheckedChanged(object sender, EventArgs e)
        {
            DrawerShowIconsWhenHidden = Switch_MainMenu4.Checked;
        }

        private void Fn_UpdateColor()
        {
            //These are just example color schemes
            switch (colorSchemeIndex)
            {
                case 0:
                    materialSkinManager.ColorScheme = new ColorScheme(
                        materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? Primary.Teal500 : Primary.Indigo500,
                        materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? Primary.Teal700 : Primary.Indigo700,
                        materialSkinManager.Theme == MaterialSkinManager.Themes.DARK ? Primary.Teal200 : Primary.Indigo100,
                        Accent.Pink200,
                        TextShade.WHITE);
                    break;

                case 1:
                    materialSkinManager.ColorScheme = new ColorScheme(
                        Primary.Green600,
                        Primary.Green700,
                        Primary.Green200,
                        Accent.Red100,
                        TextShade.WHITE);
                    break;

                case 2:
                    materialSkinManager.ColorScheme = new ColorScheme(
                        Primary.BlueGrey800,
                        Primary.BlueGrey900,
                        Primary.BlueGrey500,
                        Accent.LightBlue200,
                        TextShade.WHITE);
                    break;
            }

            // 모든 컨트롤들의 테마 & 색상 변경 반영
            this.Invalidate();

            Control[] controls = Common.GetAllControlsUsingRecursive(this);

            foreach (Control control in controls)
            {
                control.Invalidate();
            }
        }

        #endregion


        ///=======================================================================================================
        #region Setting - Initial

        private void Fn_Load_SettingValues()
        {
            Txt_PortSave_WindMeter1.Text = Resources.Resource_Setting.StrPort_WindMeter1.ToString();
            Txt_PortSave_WindMeter2.Text = Resources.Resource_Setting.StrPort_WindMeter2.ToString();
            Txt_PortSave_MsgBoard1.Text = Resources.Resource_Setting.StrPort_MsgBoard1.ToString();
            Txt_PortSave_MsgBoard2.Text = Resources.Resource_Setting.StrPort_MsgBoard2.ToString();
        }

        #endregion
    }
}