
namespace ioboard_tester
{
    partial class Form1
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
            this.txtSent = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtReceived = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.checkBoxOutput = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIONumOutput = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIONumInput = new System.Windows.Forms.TextBox();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button13 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSent
            // 
            this.txtSent.Location = new System.Drawing.Point(189, 30);
            this.txtSent.Name = "txtSent";
            this.txtSent.Size = new System.Drawing.Size(215, 248);
            this.txtSent.TabIndex = 0;
            this.txtSent.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "Comm Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Location = new System.Drawing.Point(646, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(111, 197);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baud Rate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port Name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 92);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(99, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "115200";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(99, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(3, 144);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(104, 33);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(149, 32);
            this.button3.TabIndex = 3;
            this.button3.Text = "Get Software Ver.";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtReceived
            // 
            this.txtReceived.Location = new System.Drawing.Point(429, 30);
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.Size = new System.Drawing.Size(196, 248);
            this.txtReceived.TabIndex = 4;
            this.txtReceived.Text = "";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(15, 107);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(149, 30);
            this.button4.TabIndex = 5;
            this.button4.Text = "Get Hardware Ver.";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(271, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Sent";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(499, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Received";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(502, 284);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "Clear";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(21, 152);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(139, 26);
            this.button6.TabIndex = 10;
            this.button6.Text = "Hard Meter 1";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(21, 184);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(139, 26);
            this.button7.TabIndex = 11;
            this.button7.Text = "Hard Meter 2";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(18, 230);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(139, 26);
            this.button8.TabIndex = 12;
            this.button8.Text = "Get All Inputs";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(5, 6);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(139, 26);
            this.button9.TabIndex = 13;
            this.button9.Text = "Set Output";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // checkBoxOutput
            // 
            this.checkBoxOutput.AutoSize = true;
            this.checkBoxOutput.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxOutput.Location = new System.Drawing.Point(272, 9);
            this.checkBoxOutput.Name = "checkBoxOutput";
            this.checkBoxOutput.Size = new System.Drawing.Size(64, 21);
            this.checkBoxOutput.TabIndex = 14;
            this.checkBoxOutput.Text = "HIGH";
            this.checkBoxOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxOutput.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtIONumOutput);
            this.panel2.Controls.Add(this.button9);
            this.panel2.Controls.Add(this.checkBoxOutput);
            this.panel2.Location = new System.Drawing.Point(213, 411);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(353, 42);
            this.panel2.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "IO#";
            // 
            // txtIONumOutput
            // 
            this.txtIONumOutput.Location = new System.Drawing.Point(189, 10);
            this.txtIONumOutput.Name = "txtIONumOutput";
            this.txtIONumOutput.Size = new System.Drawing.Size(62, 22);
            this.txtIONumOutput.TabIndex = 15;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(6, 5);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(139, 26);
            this.button10.TabIndex = 16;
            this.button10.Text = "Get One Input";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.txtIONumInput);
            this.panel3.Controls.Add(this.button10);
            this.panel3.Location = new System.Drawing.Point(213, 346);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(353, 40);
            this.panel3.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(154, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "IO#";
            // 
            // txtIONumInput
            // 
            this.txtIONumInput.Location = new System.Drawing.Point(190, 9);
            this.txtIONumInput.Name = "txtIONumInput";
            this.txtIONumInput.Size = new System.Drawing.Size(62, 22);
            this.txtIONumInput.TabIndex = 17;
            this.txtIONumInput.TextChanged += new System.EventHandler(this.txtIONumInput_TextChanged);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(18, 262);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(139, 26);
            this.button11.TabIndex = 18;
            this.button11.Text = "Get Switch States";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(19, 294);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(139, 26);
            this.button12.TabIndex = 19;
            this.button12.Text = "Get Output States";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(286, 9);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(62, 22);
            this.textBox2.TabIndex = 18;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(15, 343);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(149, 34);
            this.button13.TabIndex = 20;
            this.button13.Text = "Clear Errors";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 465);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtReceived);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSent);
            this.Name = "Form1";
            this.Text = "Bravery IO Board Test App";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtSent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox txtReceived;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.CheckBox checkBoxOutput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIONumOutput;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtIONumInput;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button13;
    }
}

