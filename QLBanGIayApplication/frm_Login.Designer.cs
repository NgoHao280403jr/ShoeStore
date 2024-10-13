namespace QLBanGiay_Application
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
            btn_DangNhap = new Button();
            txt_UserName = new TextBox();
            txt_PassWord = new TextBox();
            SuspendLayout();
            // 
            // btn_DangNhap
            // 
            btn_DangNhap.Location = new Point(355, 231);
            btn_DangNhap.Name = "btn_DangNhap";
            btn_DangNhap.Size = new Size(75, 23);
            btn_DangNhap.TabIndex = 0;
            btn_DangNhap.Text = "Đăng nhập";
            btn_DangNhap.UseVisualStyleBackColor = true;
            // 
            // txt_UserName
            // 
            txt_UserName.Location = new Point(311, 117);
            txt_UserName.Name = "txt_UserName";
            txt_UserName.Size = new Size(155, 23);
            txt_UserName.TabIndex = 1;
            // 
            // txt_PassWord
            // 
            txt_PassWord.Location = new Point(311, 179);
            txt_PassWord.Name = "txt_PassWord";
            txt_PassWord.Size = new Size(155, 23);
            txt_PassWord.TabIndex = 2;
            // 
            // frm_Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(748, 387);
            Controls.Add(txt_PassWord);
            Controls.Add(txt_UserName);
            Controls.Add(btn_DangNhap);
            Name = "frm_Login";
            Text = "frm_Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_DangNhap;
        private TextBox txt_UserName;
        private TextBox txt_PassWord;
    }
}