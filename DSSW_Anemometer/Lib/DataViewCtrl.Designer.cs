namespace DSSW_Anemometer.Lib
{
    partial class DataViewCtrl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.Lbl_CurTime = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard3 = new MaterialSkin.Controls.MaterialCard();
            this.Lbl_Dir = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.Lbl_Type = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.Lbl_Unit = new MaterialSkin.Controls.MaterialLabel();
            this.Lbl_Value = new MaterialSkin.Controls.MaterialLabel();
            this.Pic_State = new System.Windows.Forms.PictureBox();
            this.Lbl_Name = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard1.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.materialCard2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_State)).BeginInit();
            this.SuspendLayout();
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.Lbl_CurTime);
            this.materialCard1.Controls.Add(this.materialLabel2);
            this.materialCard1.Controls.Add(this.materialCard3);
            this.materialCard1.Controls.Add(this.materialLabel1);
            this.materialCard1.Controls.Add(this.Lbl_Type);
            this.materialCard1.Controls.Add(this.materialCard2);
            this.materialCard1.Controls.Add(this.Pic_State);
            this.materialCard1.Controls.Add(this.Lbl_Name);
            this.materialCard1.Depth = 0;
            this.materialCard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(5, 5);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(330, 160);
            this.materialCard1.TabIndex = 0;
            // 
            // Lbl_CurTime
            // 
            this.Lbl_CurTime.Depth = 0;
            this.Lbl_CurTime.Font = new System.Drawing.Font("Roboto", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_CurTime.FontType = MaterialSkin.MaterialSkinManager.fontType.Overline;
            this.Lbl_CurTime.Location = new System.Drawing.Point(210, 15);
            this.Lbl_CurTime.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_CurTime.Name = "Lbl_CurTime";
            this.Lbl_CurTime.Size = new System.Drawing.Size(100, 20);
            this.Lbl_CurTime.TabIndex = 7;
            this.Lbl_CurTime.Text = "2022-05-22 21:52:35";
            this.Lbl_CurTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // materialLabel2
            // 
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.FontType = MaterialSkin.MaterialSkinManager.fontType.Caption;
            this.materialLabel2.Location = new System.Drawing.Point(230, 61);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(90, 20);
            this.materialLabel2.TabIndex = 12;
            this.materialLabel2.Text = "Wind Direction";
            this.materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialCard3
            // 
            this.materialCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard3.Controls.Add(this.Lbl_Dir);
            this.materialCard3.Depth = 0;
            this.materialCard3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard3.Location = new System.Drawing.Point(230, 86);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(0);
            this.materialCard3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(10);
            this.materialCard3.Size = new System.Drawing.Size(80, 50);
            this.materialCard3.TabIndex = 11;
            // 
            // Lbl_Dir
            // 
            this.Lbl_Dir.Depth = 0;
            this.Lbl_Dir.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Dir.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.Lbl_Dir.Location = new System.Drawing.Point(5, 5);
            this.Lbl_Dir.Margin = new System.Windows.Forms.Padding(0);
            this.Lbl_Dir.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_Dir.Name = "Lbl_Dir";
            this.Lbl_Dir.Size = new System.Drawing.Size(70, 40);
            this.Lbl_Dir.TabIndex = 4;
            this.Lbl_Dir.Text = "0";
            this.Lbl_Dir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.Caption;
            this.materialLabel1.Location = new System.Drawing.Point(95, 61);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(100, 20);
            this.materialLabel1.TabIndex = 10;
            this.materialLabel1.Text = "Wind Speed";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Lbl_Type
            // 
            this.Lbl_Type.Depth = 0;
            this.Lbl_Type.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Type.FontType = MaterialSkin.MaterialSkinManager.fontType.Caption;
            this.Lbl_Type.Location = new System.Drawing.Point(120, 15);
            this.Lbl_Type.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_Type.Name = "Lbl_Type";
            this.Lbl_Type.Size = new System.Drawing.Size(100, 20);
            this.Lbl_Type.TabIndex = 9;
            this.Lbl_Type.Text = "[Ultrasonic]";
            this.Lbl_Type.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.Lbl_Unit);
            this.materialCard2.Controls.Add(this.Lbl_Value);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(95, 86);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(0);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(10);
            this.materialCard2.Size = new System.Drawing.Size(120, 50);
            this.materialCard2.TabIndex = 8;
            // 
            // Lbl_Unit
            // 
            this.Lbl_Unit.AutoSize = true;
            this.Lbl_Unit.Depth = 0;
            this.Lbl_Unit.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Unit.FontType = MaterialSkin.MaterialSkinManager.fontType.Body2;
            this.Lbl_Unit.Location = new System.Drawing.Point(85, 25);
            this.Lbl_Unit.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_Unit.Name = "Lbl_Unit";
            this.Lbl_Unit.Size = new System.Drawing.Size(26, 17);
            this.Lbl_Unit.TabIndex = 5;
            this.Lbl_Unit.Text = "m/s";
            // 
            // Lbl_Value
            // 
            this.Lbl_Value.Depth = 0;
            this.Lbl_Value.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Value.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.Lbl_Value.Location = new System.Drawing.Point(5, 5);
            this.Lbl_Value.Margin = new System.Windows.Forms.Padding(0);
            this.Lbl_Value.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_Value.Name = "Lbl_Value";
            this.Lbl_Value.Size = new System.Drawing.Size(70, 40);
            this.Lbl_Value.TabIndex = 4;
            this.Lbl_Value.Text = "0.0";
            this.Lbl_Value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Pic_State
            // 
            this.Pic_State.Image = global::DSSW_Anemometer.Resources.Resource_DataView.Ready;
            this.Pic_State.Location = new System.Drawing.Point(30, 90);
            this.Pic_State.Name = "Pic_State";
            this.Pic_State.Size = new System.Drawing.Size(40, 40);
            this.Pic_State.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_State.TabIndex = 1;
            this.Pic_State.TabStop = false;
            // 
            // Lbl_Name
            // 
            this.Lbl_Name.AutoSize = true;
            this.Lbl_Name.Depth = 0;
            this.Lbl_Name.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Lbl_Name.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.Lbl_Name.Location = new System.Drawing.Point(10, 10);
            this.Lbl_Name.MouseState = MaterialSkin.MouseState.HOVER;
            this.Lbl_Name.Name = "Lbl_Name";
            this.Lbl_Name.Size = new System.Drawing.Size(100, 24);
            this.Lbl_Name.TabIndex = 0;
            this.Lbl_Name.Text = "WindMeter";
            // 
            // DataViewCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.materialCard1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DataViewCtrl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(340, 170);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard3.ResumeLayout(false);
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_State)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialLabel Lbl_Unit;
        private MaterialSkin.Controls.MaterialLabel Lbl_Value;
        private MaterialSkin.Controls.MaterialLabel Lbl_CurTime;
        private PictureBox Pic_State;
        private MaterialSkin.Controls.MaterialLabel Lbl_Name;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel Lbl_Type;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialCard materialCard3;
        private MaterialSkin.Controls.MaterialLabel Lbl_Dir;
    }
}
