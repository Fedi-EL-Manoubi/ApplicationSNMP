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
            textBoxIPAddress = new TextBox();
            button1 = new Button();
            textBoxCommunity = new TextBox();
            SuspendLayout();
            // 
            // textBoxIPAddress
            // 
            textBoxIPAddress.Location = new Point(224, 140);
            textBoxIPAddress.Name = "textBoxIPAddress";
            textBoxIPAddress.Size = new Size(149, 23);
            textBoxIPAddress.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(351, 169);
            button1.Name = "button1";
            button1.Size = new Size(86, 23);
            button1.TabIndex = 4;
            button1.Text = "Obtenir Infos";
            button1.UseVisualStyleBackColor = true;

            // 
            // textBoxCommunity
            // 
            textBoxCommunity.Location = new Point(407, 140);
            textBoxCommunity.Name = "textBoxCommunity";
            textBoxCommunity.Size = new Size(149, 23);
            textBoxCommunity.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxCommunity);
            Controls.Add(button1);
            Controls.Add(textBoxIPAddress);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBoxIPAddress;
        private Button button1;
        private TextBox textBoxCommunity;
    }
}
