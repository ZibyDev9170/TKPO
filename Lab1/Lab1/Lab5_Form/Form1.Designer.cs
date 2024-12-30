namespace Lab5_Form
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
            btnCallLift = new Button();
            btnLoadCargo = new Button();
            btnUnloadCargo = new Button();
            btnRestorePower = new Button();
            lblStatus = new Label();
            txtTargetFloor = new TextBox();
            txtCargoWeight = new TextBox();
            SuspendLayout();
            // 
            // btnCallLift
            // 
            btnCallLift.Location = new Point(247, 43);
            btnCallLift.Name = "btnCallLift";
            btnCallLift.Size = new Size(124, 74);
            btnCallLift.TabIndex = 0;
            btnCallLift.Text = "Вызвать лифт";
            btnCallLift.UseVisualStyleBackColor = true;
            btnCallLift.Click += btnCallLift_Click;
            // 
            // btnLoadCargo
            // 
            btnLoadCargo.Location = new Point(247, 134);
            btnLoadCargo.Name = "btnLoadCargo";
            btnLoadCargo.Size = new Size(124, 74);
            btnLoadCargo.TabIndex = 1;
            btnLoadCargo.Text = "Загрузить груз";
            btnLoadCargo.UseVisualStyleBackColor = true;
            btnLoadCargo.Click += btnLoadCargo_Click;
            // 
            // btnUnloadCargo
            // 
            btnUnloadCargo.Location = new Point(247, 227);
            btnUnloadCargo.Name = "btnUnloadCargo";
            btnUnloadCargo.Size = new Size(124, 74);
            btnUnloadCargo.TabIndex = 2;
            btnUnloadCargo.Text = "Разгрузить груз";
            btnUnloadCargo.UseVisualStyleBackColor = true;
            btnUnloadCargo.Click += btnUnloadCargo_Click;
            // 
            // btnRestorePower
            // 
            btnRestorePower.Location = new Point(247, 321);
            btnRestorePower.Name = "btnRestorePower";
            btnRestorePower.Size = new Size(124, 74);
            btnRestorePower.TabIndex = 3;
            btnRestorePower.Text = "Восстановить питание";
            btnRestorePower.UseVisualStyleBackColor = true;
            btnRestorePower.Click += btnRestorePower_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(17, 43);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(49, 20);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Пусто";
            // 
            // txtTargetFloor
            // 
            txtTargetFloor.Location = new Point(17, 134);
            txtTargetFloor.Name = "txtTargetFloor";
            txtTargetFloor.Size = new Size(212, 27);
            txtTargetFloor.TabIndex = 5;
            // 
            // txtCargoWeight
            // 
            txtCargoWeight.Location = new Point(17, 227);
            txtCargoWeight.Name = "txtCargoWeight";
            txtCargoWeight.Size = new Size(212, 27);
            txtCargoWeight.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 450);
            Controls.Add(txtCargoWeight);
            Controls.Add(txtTargetFloor);
            Controls.Add(lblStatus);
            Controls.Add(btnRestorePower);
            Controls.Add(btnUnloadCargo);
            Controls.Add(btnLoadCargo);
            Controls.Add(btnCallLift);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCallLift;
        private Button btnLoadCargo;
        private Button btnUnloadCargo;
        private Button btnRestorePower;
        private Label lblStatus;
        private TextBox txtTargetFloor;
        private TextBox txtCargoWeight;
    }
}
