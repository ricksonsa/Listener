namespace Listener
{
    partial class Player
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.TryReconnectTimer = new System.Windows.Forms.Timer(this.components);
            this.ListenerIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseCMSitem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TryReconnectTimer
            // 
            this.TryReconnectTimer.Enabled = true;
            this.TryReconnectTimer.Interval = 5000;
            this.TryReconnectTimer.Tick += new System.EventHandler(this.TryReconnectTimer_Tick);
            // 
            // ListenerIcon
            // 
            this.ListenerIcon.BalloonTipText = "Listener";
            this.ListenerIcon.BalloonTipTitle = "Listener";
            this.ListenerIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.ListenerIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ListenerIcon.Icon")));
            this.ListenerIcon.Text = "Listener";
            this.ListenerIcon.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseCMSitem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(94, 26);
            // 
            // CloseCMSitem
            // 
            this.CloseCMSitem.Name = "CloseCMSitem";
            this.CloseCMSitem.Size = new System.Drawing.Size(93, 22);
            this.CloseCMSitem.Text = "Sair";
            this.CloseCMSitem.Click += new System.EventHandler(this.CloseCMSitem_Click);
            // 
            // Player
            // 
            this.ClientSize = new System.Drawing.Size(120, 18);
            this.Enabled = false;
            this.Name = "Player";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Player_FormClosing);
            this.Load += new System.EventHandler(this.Player_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Container;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.Button RecordBtn;
        private System.Windows.Forms.Timer TryReconnectTimer;
        private System.Windows.Forms.NotifyIcon ListenerIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CloseCMSitem;
    }
}