namespace QLBanGiay_Application.View
{
    partial class frm_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txt_UserName = new TextBox();
            txt_PassWord = new TextBox();
            btn_DangNhap = new Button();
            SuspendLayout();
            // 
            // txt_UserName
            // 
            txt_UserName.Location = new Point(362, 135);
            txt_UserName.Name = "txt_UserName";
            txt_UserName.Size = new Size(100, 23);
            txt_UserName.TabIndex = 0;
            // 
            // txt_PassWord
            // 
            txt_PassWord.Location = new Point(362, 196);
            txt_PassWord.Name = "txt_PassWord";
            txt_PassWord.Size = new Size(100, 23);
            txt_PassWord.TabIndex = 1;
            // 
            // btn_DangNhap
            // 
            btn_DangNhap.Location = new Point(376, 270);
            btn_DangNhap.Name = "btn_DangNhap";
            btn_DangNhap.Size = new Size(75, 23);
            btn_DangNhap.TabIndex = 2;
            btn_DangNhap.Text = "Đăng nhập ";
            btn_DangNhap.UseVisualStyleBackColor = true;
            // 
            // frm_Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btn_DangNhap);
            Controls.Add(txt_PassWord);
            Controls.Add(txt_UserName);
            Name = "frm_Login";
            Text = "frm_Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txt_UserName;
        private TextBox txt_PassWord;
        private Button btn_DangNhap;
    }
}