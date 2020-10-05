namespace CompGraphicsLab04
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.XBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.YBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.MoveBtn = new System.Windows.Forms.Button();
            this.RotateAngle = new System.Windows.Forms.TrackBar();
            this.angle_label = new System.Windows.Forms.Label();
            this.RotateBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ScaleAlpha = new System.Windows.Forms.TrackBar();
            this.ScaleAlphaLabel = new System.Windows.Forms.Label();
            this.ScaleBtn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.PointForAffineLabel = new System.Windows.Forms.Label();
            this.ScaleBeta = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ScaleBetaLabel = new System.Windows.Forms.Label();
            this.Rotate90 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBeta)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(221, 12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(931, 774);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // ClearBtn
            // 
            this.ClearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ClearBtn.Location = new System.Drawing.Point(12, 12);
            this.ClearBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(175, 50);
            this.ClearBtn.TabIndex = 1;
            this.ClearBtn.Text = "Очистить";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.Clear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Добавить примитив";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 111);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 21);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Точка";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 139);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(134, 21);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Ребро (отрезок)";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(12, 167);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(81, 21);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Полигон";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton3_MouseClick);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(1159, 14);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(231, 772);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(5, 736);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(365, 21);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Принадлежит ли точка выпуклому многоугольнику ";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(5, 764);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(331, 21);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Классфицикация точки относительно прямой";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 40);
            this.label2.TabIndex = 9;
            this.label2.Text = "Аффинные\r\nпреобразования";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 246);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Перенос";
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(39, 266);
            this.XBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(64, 22);
            this.XBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 268);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 268);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y";
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(136, 266);
            this.YBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(64, 22);
            this.YBox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 337);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Поворот";
            // 
            // MoveBtn
            // 
            this.MoveBtn.Enabled = false;
            this.MoveBtn.Location = new System.Drawing.Point(100, 298);
            this.MoveBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MoveBtn.Name = "MoveBtn";
            this.MoveBtn.Size = new System.Drawing.Size(100, 28);
            this.MoveBtn.TabIndex = 16;
            this.MoveBtn.Text = "Перенести";
            this.MoveBtn.UseVisualStyleBackColor = true;
            this.MoveBtn.Click += new System.EventHandler(this.MoveBtn_Click);
            // 
            // RotateAngle
            // 
            this.RotateAngle.Location = new System.Drawing.Point(12, 358);
            this.RotateAngle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RotateAngle.Maximum = 180;
            this.RotateAngle.Minimum = -180;
            this.RotateAngle.Name = "RotateAngle";
            this.RotateAngle.Size = new System.Drawing.Size(188, 53);
            this.RotateAngle.TabIndex = 17;
            this.RotateAngle.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // angle_label
            // 
            this.angle_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.angle_label.AutoSize = true;
            this.angle_label.Location = new System.Drawing.Point(163, 337);
            this.angle_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.angle_label.MinimumSize = new System.Drawing.Size(37, 16);
            this.angle_label.Name = "angle_label";
            this.angle_label.Size = new System.Drawing.Size(37, 17);
            this.angle_label.TabIndex = 18;
            this.angle_label.Text = "0";
            this.angle_label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RotateBtn
            // 
            this.RotateBtn.Enabled = false;
            this.RotateBtn.Location = new System.Drawing.Point(100, 396);
            this.RotateBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RotateBtn.Name = "RotateBtn";
            this.RotateBtn.Size = new System.Drawing.Size(100, 28);
            this.RotateBtn.TabIndex = 19;
            this.RotateBtn.Text = "Повернуть";
            this.RotateBtn.UseVisualStyleBackColor = true;
            this.RotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 431);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "Масштабирование";
            // 
            // ScaleAlpha
            // 
            this.ScaleAlpha.Location = new System.Drawing.Point(12, 450);
            this.ScaleAlpha.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ScaleAlpha.Maximum = 400;
            this.ScaleAlpha.Name = "ScaleAlpha";
            this.ScaleAlpha.Size = new System.Drawing.Size(188, 53);
            this.ScaleAlpha.TabIndex = 21;
            this.ScaleAlpha.Value = 100;
            this.ScaleAlpha.ValueChanged += new System.EventHandler(this.ScaleVal_ValueChanged);
            // 
            // ScaleAlphaLabel
            // 
            this.ScaleAlphaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleAlphaLabel.AutoSize = true;
            this.ScaleAlphaLabel.Location = new System.Drawing.Point(163, 431);
            this.ScaleAlphaLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ScaleAlphaLabel.MinimumSize = new System.Drawing.Size(37, 16);
            this.ScaleAlphaLabel.Name = "ScaleAlphaLabel";
            this.ScaleAlphaLabel.Size = new System.Drawing.Size(37, 17);
            this.ScaleAlphaLabel.TabIndex = 22;
            this.ScaleAlphaLabel.Text = "100";
            this.ScaleAlphaLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ScaleBtn
            // 
            this.ScaleBtn.Enabled = false;
            this.ScaleBtn.Location = new System.Drawing.Point(60, 555);
            this.ScaleBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ScaleBtn.Name = "ScaleBtn";
            this.ScaleBtn.Size = new System.Drawing.Size(140, 28);
            this.ScaleBtn.TabIndex = 23;
            this.ScaleBtn.Text = "Масштабировать";
            this.ScaleBtn.UseVisualStyleBackColor = true;
            this.ScaleBtn.Click += new System.EventHandler(this.ScaleBtn_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(5, 591);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(79, 44);
            this.button3.TabIndex = 24;
            this.button3.Text = "Выбор точки";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // PointForAffineLabel
            // 
            this.PointForAffineLabel.AutoSize = true;
            this.PointForAffineLabel.Location = new System.Drawing.Point(96, 598);
            this.PointForAffineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PointForAffineLabel.Name = "PointForAffineLabel";
            this.PointForAffineLabel.Size = new System.Drawing.Size(114, 17);
            this.PointForAffineLabel.TabIndex = 25;
            this.PointForAffineLabel.Text = "Не установлена";
            // 
            // ScaleBeta
            // 
            this.ScaleBeta.Location = new System.Drawing.Point(12, 501);
            this.ScaleBeta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ScaleBeta.Maximum = 400;
            this.ScaleBeta.Name = "ScaleBeta";
            this.ScaleBeta.Size = new System.Drawing.Size(188, 53);
            this.ScaleBeta.TabIndex = 26;
            this.ScaleBeta.Value = 100;
            this.ScaleBeta.ValueChanged += new System.EventHandler(this.ScaleVal_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 452);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 17);
            this.label8.TabIndex = 27;
            this.label8.Text = "α";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 501);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 17);
            this.label9.TabIndex = 28;
            this.label9.Text = "β";
            // 
            // ScaleBetaLabel
            // 
            this.ScaleBetaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleBetaLabel.AutoSize = true;
            this.ScaleBetaLabel.Location = new System.Drawing.Point(163, 490);
            this.ScaleBetaLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ScaleBetaLabel.MinimumSize = new System.Drawing.Size(37, 16);
            this.ScaleBetaLabel.Name = "ScaleBetaLabel";
            this.ScaleBetaLabel.Size = new System.Drawing.Size(37, 17);
            this.ScaleBetaLabel.TabIndex = 29;
            this.ScaleBetaLabel.Text = "100";
            this.ScaleBetaLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Rotate90
            // 
            this.Rotate90.Enabled = false;
            this.Rotate90.Location = new System.Drawing.Point(5, 644);
            this.Rotate90.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Rotate90.Name = "Rotate90";
            this.Rotate90.Size = new System.Drawing.Size(208, 46);
            this.Rotate90.TabIndex = 30;
            this.Rotate90.Text = "Поворот ребра на 90° вокруг своего центра";
            this.Rotate90.UseVisualStyleBackColor = true;
            this.Rotate90.Click += new System.EventHandler(this.Rotate90_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(15, 710);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(160, 21);
            this.checkBox3.TabIndex = 31;
            this.checkBox3.Text = "Пересечение ребер";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1403, 800);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.Rotate90);
            this.Controls.Add(this.ScaleBetaLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ScaleBeta);
            this.Controls.Add(this.PointForAffineLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ScaleBtn);
            this.Controls.Add(this.ScaleAlphaLabel);
            this.Controls.Add(this.ScaleAlpha);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RotateBtn);
            this.Controls.Add(this.angle_label);
            this.Controls.Add(this.RotateAngle);
            this.Controls.Add(this.MoveBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.XBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBeta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown XBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown YBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button MoveBtn;
        private System.Windows.Forms.TrackBar RotateAngle;
        private System.Windows.Forms.Label angle_label;
        private System.Windows.Forms.Button RotateBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar ScaleAlpha;
        private System.Windows.Forms.Label ScaleAlphaLabel;
        private System.Windows.Forms.Button ScaleBtn;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label PointForAffineLabel;
        private System.Windows.Forms.TrackBar ScaleBeta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label ScaleBetaLabel;
        private System.Windows.Forms.Button Rotate90;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}

