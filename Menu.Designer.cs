namespace SqlClinicaUser
{
    partial class Menu
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
            this.Mnu = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuPaciente = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuMedico = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuConsultas = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuProntuario = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuPagamentos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuSair = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MnuCPaciente = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCMedico = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCConsultas = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuCProntuario = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCPagamentos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuCSair = new System.Windows.Forms.ToolStripMenuItem();
            this.Mnu.SuspendLayout();
            this.ContextMnu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Mnu
            // 
            this.Mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.Mnu.Location = new System.Drawing.Point(0, 0);
            this.Mnu.Name = "Mnu";
            this.Mnu.Size = new System.Drawing.Size(800, 24);
            this.Mnu.TabIndex = 0;
            this.Mnu.Text = "Mnu";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuPaciente,
            this.MnuMedico,
            this.MnuConsultas,
            this.toolStripMenuItem1,
            this.MnuProntuario,
            this.MnuPagamentos,
            this.toolStripMenuItem2,
            this.MnuSair});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // MnuPaciente
            // 
            this.MnuPaciente.Name = "MnuPaciente";
            this.MnuPaciente.Size = new System.Drawing.Size(192, 22);
            this.MnuPaciente.Text = "Cadastro de Pacientes";
            this.MnuPaciente.Click += new System.EventHandler(this.cadastroPacientesToolStripMenuItem_Click);
            // 
            // MnuMedico
            // 
            this.MnuMedico.Name = "MnuMedico";
            this.MnuMedico.Size = new System.Drawing.Size(192, 22);
            this.MnuMedico.Text = "Cadastro de Médicos";
            this.MnuMedico.Click += new System.EventHandler(this.MnuMedico_Click);
            // 
            // MnuConsultas
            // 
            this.MnuConsultas.Name = "MnuConsultas";
            this.MnuConsultas.Size = new System.Drawing.Size(192, 22);
            this.MnuConsultas.Text = "Consultas";
            this.MnuConsultas.Click += new System.EventHandler(this.MnuConsultas_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
            // 
            // MnuProntuario
            // 
            this.MnuProntuario.Name = "MnuProntuario";
            this.MnuProntuario.Size = new System.Drawing.Size(192, 22);
            this.MnuProntuario.Text = "Prontuário";
            this.MnuProntuario.Click += new System.EventHandler(this.MnuProntuario_Click);
            // 
            // MnuPagamentos
            // 
            this.MnuPagamentos.Name = "MnuPagamentos";
            this.MnuPagamentos.Size = new System.Drawing.Size(192, 22);
            this.MnuPagamentos.Text = "Pagamentos / Recibos";
            this.MnuPagamentos.Click += new System.EventHandler(this.MnuPagamentos_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(189, 6);
            // 
            // MnuSair
            // 
            this.MnuSair.Name = "MnuSair";
            this.MnuSair.Size = new System.Drawing.Size(192, 22);
            this.MnuSair.Text = "Sair";
            this.MnuSair.Click += new System.EventHandler(this.MnuSair_Click);
            // 
            // ContextMnu
            // 
            this.ContextMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuCPaciente,
            this.MnuCMedico,
            this.MnuCConsultas,
            this.toolStripMenuItem3,
            this.MnuCProntuario,
            this.MnuCPagamentos,
            this.toolStripMenuItem4,
            this.MnuCSair});
            this.ContextMnu.Name = "ContextMnu";
            this.ContextMnu.Size = new System.Drawing.Size(193, 148);
            // 
            // MnuCPaciente
            // 
            this.MnuCPaciente.Name = "MnuCPaciente";
            this.MnuCPaciente.Size = new System.Drawing.Size(192, 22);
            this.MnuCPaciente.Text = "Cadastro de Pacientes";
            this.MnuCPaciente.Click += new System.EventHandler(this.MnuCPaciente_Click);
            // 
            // MnuCMedico
            // 
            this.MnuCMedico.Name = "MnuCMedico";
            this.MnuCMedico.Size = new System.Drawing.Size(192, 22);
            this.MnuCMedico.Text = "Cadastro de Médicos";
            this.MnuCMedico.Click += new System.EventHandler(this.MnuCMedico_Click);
            // 
            // MnuCConsultas
            // 
            this.MnuCConsultas.Name = "MnuCConsultas";
            this.MnuCConsultas.Size = new System.Drawing.Size(192, 22);
            this.MnuCConsultas.Text = "Consultas";
            this.MnuCConsultas.Click += new System.EventHandler(this.MnuCConsultas_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(189, 6);
            // 
            // MnuCProntuario
            // 
            this.MnuCProntuario.Name = "MnuCProntuario";
            this.MnuCProntuario.Size = new System.Drawing.Size(192, 22);
            this.MnuCProntuario.Text = "Prontuário";
            this.MnuCProntuario.Click += new System.EventHandler(this.MnuCProntuario_Click);
            // 
            // MnuCPagamentos
            // 
            this.MnuCPagamentos.Name = "MnuCPagamentos";
            this.MnuCPagamentos.Size = new System.Drawing.Size(192, 22);
            this.MnuCPagamentos.Text = "Pagamentos / Recibos";
            this.MnuCPagamentos.Click += new System.EventHandler(this.MnuCPagamentos_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(189, 6);
            // 
            // MnuCSair
            // 
            this.MnuCSair.Name = "MnuCSair";
            this.MnuCSair.Size = new System.Drawing.Size(192, 22);
            this.MnuCSair.Text = "Sair";
            this.MnuCSair.Click += new System.EventHandler(this.MnuCSair_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.ContextMnu;
            this.Controls.Add(this.Mnu);
            this.MainMenuStrip = this.Mnu;
            this.Name = "Menu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.Mnu.ResumeLayout(false);
            this.Mnu.PerformLayout();
            this.ContextMnu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Mnu;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip ContextMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        public System.Windows.Forms.ToolStripMenuItem MnuPaciente;
        public System.Windows.Forms.ToolStripMenuItem MnuMedico;
        public System.Windows.Forms.ToolStripMenuItem MnuConsultas;
        public System.Windows.Forms.ToolStripMenuItem MnuProntuario;
        public System.Windows.Forms.ToolStripMenuItem MnuPagamentos;
        public System.Windows.Forms.ToolStripMenuItem MnuSair;
        public System.Windows.Forms.ToolStripMenuItem MnuCPaciente;
        public System.Windows.Forms.ToolStripMenuItem MnuCMedico;
        public System.Windows.Forms.ToolStripMenuItem MnuCConsultas;
        public System.Windows.Forms.ToolStripMenuItem MnuCProntuario;
        public System.Windows.Forms.ToolStripMenuItem MnuCPagamentos;
        public System.Windows.Forms.ToolStripMenuItem MnuCSair;
    }
}