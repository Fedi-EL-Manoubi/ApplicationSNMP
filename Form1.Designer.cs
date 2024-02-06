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
            SuspendLayout();
            // 
            // TextBoxIPAddress
            // 
            TextBoxIPAddress.Location = new Point(233, 150);
            TextBoxIPAddress.Name = "TextBoxIPAddress";
            TextBoxIPAddress.Size = new Size(136, 23);
            TextBoxIPAddress.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(354, 261);
            button1.Name = "button1";
            button1.Size = new Size(108, 39);
            button1.TabIndex = 4;
            button1.Text = "Obtenir Infos";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click_1;
            // 
            // TextBoxCommunity
            // 
            TextBoxCommunity.Location = new Point(447, 150);
            TextBoxCommunity.Name = "TextBoxCommunity";
            TextBoxCommunity.Size = new Size(136, 23);
            TextBoxCommunity.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(267, 132);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 6;
            label1.Text = "Adresse IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(473, 132);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 7;
            label2.Text = "Communauté";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(826, 570);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TextBoxCommunity);
            Controls.Add(button1);
            Controls.Add(TextBoxIPAddress);
            Name = "Form1";
            Text = "SNMP Info";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox TextBoxIPAddress;
        private Button button1;
        private TextBox TextBoxCommunity;
        private Label label1;
        private Label label2;
        
    }
}
