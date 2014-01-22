using System.Drawing;
namespace ChatClientInterface
{
    partial class ChatWindow
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
            this.button1 = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.TextBox();
            Output = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(366, 426);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(12, 426);
            this.Input.MaxLength = 1024;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(348, 20);
            this.Input.TabIndex = 1;
            this.Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyPressed);
            // 
            // Output
            //
            Output.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Output.FormattingEnabled = true;
            Output.ItemHeight = 16;
            Output.Location = new System.Drawing.Point(12, 12);
            Output.Name = "Output";
            Output.Size = new System.Drawing.Size(460, 404);
            Output.TabIndex = 2;
            Output.ForeColor = Color.Black;
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(Output);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChatWindow";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosed);
            this.Load += new System.EventHandler(this.ChatWindow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ChatWindow_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Input;
        public static System.Windows.Forms.ListBox Output;
    }
}

