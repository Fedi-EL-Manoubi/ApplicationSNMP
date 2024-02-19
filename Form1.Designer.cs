using System.Runtime;

namespace ApplicationSNMP
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TextBoxIPAddress = new TextBox();
            button1 = new Button();
            TextBoxCommunity = new TextBox();
            label1 = new Label();
            label2 = new Label();
            LabelOid = new Label();
            eXitToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            affichageToolStripMenuItem = new ToolStripMenuItem();
            plaineÉcranToolStripMenuItem = new ToolStripMenuItem();
            BoxOid1 = new ComboBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxIPAddress
            // 
            TextBoxIPAddress.Anchor = AnchorStyles.None;
            TextBoxIPAddress.Location = new Point(291, 127);
            TextBoxIPAddress.Name = "TextBoxIPAddress";
            TextBoxIPAddress.Size = new Size(136, 23);
            TextBoxIPAddress.TabIndex = 3;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.Location = new Point(406, 288);
            button1.Name = "button1";
            button1.Size = new Size(108, 39);
            button1.TabIndex = 4;
            button1.Text = "Obtenir Infos";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click_1;
            // 
            // TextBoxCommunity
            // 
            TextBoxCommunity.Anchor = AnchorStyles.None;
            TextBoxCommunity.Location = new Point(505, 127);
            TextBoxCommunity.Name = "TextBoxCommunity";
            TextBoxCommunity.Size = new Size(136, 23);
            TextBoxCommunity.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Location = new Point(325, 109);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 6;
            label1.Text = "Adresse IP";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(531, 109);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 7;
            label2.Text = "Communauté";
            // 
            // LabelOid
            // 
            LabelOid.Anchor = AnchorStyles.None;
            LabelOid.AutoSize = true;
            LabelOid.Location = new Point(451, 188);
            LabelOid.Name = "LabelOid";
            LabelOid.Size = new Size(27, 15);
            LabelOid.TabIndex = 9;
            LabelOid.Text = "OID";
            // 
            // eXitToolStripMenuItem
            // 
            eXitToolStripMenuItem.Name = "eXitToolStripMenuItem";
            eXitToolStripMenuItem.Size = new Size(56, 20);
            eXitToolStripMenuItem.Text = "Quitter";
            eXitToolStripMenuItem.Click += eXitToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { eXitToolStripMenuItem, affichageToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(935, 24);
            menuStrip1.TabIndex = 10;
            menuStrip1.Text = "menuStrip1";
            // 
            // affichageToolStripMenuItem
            // 
            affichageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { plaineÉcranToolStripMenuItem });
            affichageToolStripMenuItem.Name = "affichageToolStripMenuItem";
            affichageToolStripMenuItem.Size = new Size(70, 20);
            affichageToolStripMenuItem.Text = "Affichage";
            // 
            // plaineÉcranToolStripMenuItem
            // 
            plaineÉcranToolStripMenuItem.Name = "plaineÉcranToolStripMenuItem";
            plaineÉcranToolStripMenuItem.Size = new Size(180, 22);
            plaineÉcranToolStripMenuItem.Text = "Plein écran";
            plaineÉcranToolStripMenuItem.TextAlign = ContentAlignment.MiddleLeft;
            plaineÉcranToolStripMenuItem.Click += plaineÉcranToolStripMenuItem_Click;
            // 
            // BoxOid1
            // 
            BoxOid1.Anchor = AnchorStyles.None;
            BoxOid1.FormattingEnabled = true;
            BoxOid1.Items.AddRange(new object[] { "BoxOid1" });
            BoxOid1.Location = new Point(376, 206);
            BoxOid1.Name = "BoxOid1";
            BoxOid1.Size = new Size(181, 23);
            BoxOid1.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(935, 531);
            Controls.Add(BoxOid1);
            Controls.Add(LabelOid);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TextBoxCommunity);
            Controls.Add(button1);
            Controls.Add(TextBoxIPAddress);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(616, 450);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SNMP Info";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox TextBoxIPAddress;
        private Button button1;
        private TextBox TextBoxCommunity;
        private Label label1;
        private Label label2;
        private Label LabelOid;
        private ToolStripMenuItem eXitToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem affichageToolStripMenuItem;
        private ToolStripMenuItem plaineÉcranToolStripMenuItem;
        private ComboBox BoxOid1;
    }
}
