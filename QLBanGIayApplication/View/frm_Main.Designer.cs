namespace QLBanGiay_Application.View
{
    partial class frm_Main
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
            menuStrip1 = new MenuStrip();
            quảnLýDanhMụcToolStripMenuItem = new ToolStripMenuItem();
            quảnLýSảnPhẩmToolStripMenuItem = new ToolStripMenuItem();
            quảnLýĐơnHàngToolStripMenuItem = new ToolStripMenuItem();
            quảnLýKháchHàngToolStripMenuItem = new ToolStripMenuItem();
            quảnLýKhoToolStripMenuItem = new ToolStripMenuItem();
            quảnLýChiNhánhToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { quảnLýDanhMụcToolStripMenuItem, quảnLýSảnPhẩmToolStripMenuItem, quảnLýĐơnHàngToolStripMenuItem, quảnLýKháchHàngToolStripMenuItem, quảnLýKhoToolStripMenuItem, quảnLýChiNhánhToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 3, 0, 3);
            menuStrip1.Size = new Size(1247, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // quảnLýDanhMụcToolStripMenuItem
            // 
            quảnLýDanhMụcToolStripMenuItem.Name = "quảnLýDanhMụcToolStripMenuItem";
            quảnLýDanhMụcToolStripMenuItem.Size = new Size(210, 24);
            quảnLýDanhMụcToolStripMenuItem.Text = "Quản lý danh mục sản phẩm";
            // 
            // quảnLýSảnPhẩmToolStripMenuItem
            // 
            quảnLýSảnPhẩmToolStripMenuItem.Name = "quảnLýSảnPhẩmToolStripMenuItem";
            quảnLýSảnPhẩmToolStripMenuItem.Size = new Size(141, 24);
            quảnLýSảnPhẩmToolStripMenuItem.Text = "Quản lý sản phẩm";
            // 
            // quảnLýĐơnHàngToolStripMenuItem
            // 
            quảnLýĐơnHàngToolStripMenuItem.Name = "quảnLýĐơnHàngToolStripMenuItem";
            quảnLýĐơnHàngToolStripMenuItem.Size = new Size(140, 24);
            quảnLýĐơnHàngToolStripMenuItem.Text = "Quản lý đơn hàng";
            // 
            // quảnLýKháchHàngToolStripMenuItem
            // 
            quảnLýKháchHàngToolStripMenuItem.Name = "quảnLýKháchHàngToolStripMenuItem";
            quảnLýKháchHàngToolStripMenuItem.Size = new Size(152, 24);
            quảnLýKháchHàngToolStripMenuItem.Text = "Quản lý khách hàng";
            // 
            // quảnLýKhoToolStripMenuItem
            // 
            quảnLýKhoToolStripMenuItem.Name = "quảnLýKhoToolStripMenuItem";
            quảnLýKhoToolStripMenuItem.Size = new Size(101, 24);
            quảnLýKhoToolStripMenuItem.Text = "Quản lý kho";
            // 
            // quảnLýChiNhánhToolStripMenuItem
            // 
            quảnLýChiNhánhToolStripMenuItem.Name = "quảnLýChiNhánhToolStripMenuItem";
            quảnLýChiNhánhToolStripMenuItem.Size = new Size(140, 24);
            quảnLýChiNhánhToolStripMenuItem.Text = "Quản lý chi nhánh";
            // 
            // frm_Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1247, 721);
            Controls.Add(menuStrip1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "frm_Main";
            Text = "frm_Main";
            Load += frm_Main_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem quảnLýDanhMụcToolStripMenuItem;
        private ToolStripMenuItem quảnLýSảnPhẩmToolStripMenuItem;
        private ToolStripMenuItem quảnLýĐơnHàngToolStripMenuItem;
        private ToolStripMenuItem quảnLýKháchHàngToolStripMenuItem;
        private ToolStripMenuItem quảnLýKhoToolStripMenuItem;
        private ToolStripMenuItem quảnLýChiNhánhToolStripMenuItem;
    }
}