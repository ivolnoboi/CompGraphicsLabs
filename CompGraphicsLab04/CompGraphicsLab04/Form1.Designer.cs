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
            this.button1 = new System.Windows.Forms.Button();
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
            this.ScaleVal = new System.Windows.Forms.TrackBar();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.PointForAffineLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleVal)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(166, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(698, 535);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(9, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "Очистить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(9, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбрать примитив";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(9, 90);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(55, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Точка";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(9, 113);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(106, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Ребро (отрезок)";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(9, 136);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(68, 17);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Полигон";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButton3_MouseClick);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(869, 11);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(174, 535);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 495);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(288, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Принадлежит ли точка выпуклому многоугольнику ";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 518);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(258, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Классфицикация точки относительно прямой";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(9, 155);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 34);
            this.label2.TabIndex = 9;
            this.label2.Text = "Аффинные\r\nпреобразования";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Перенос";
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(29, 205);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(48, 20);
            this.XBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y";
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(102, 205);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(48, 20);
            this.YBox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Поворот";
            // 
            // MoveBtn
            // 
            this.MoveBtn.Enabled = false;
            this.MoveBtn.Location = new System.Drawing.Point(75, 231);
            this.MoveBtn.Name = "MoveBtn";
            this.MoveBtn.Size = new System.Drawing.Size(75, 23);
            this.MoveBtn.TabIndex = 16;
            this.MoveBtn.Text = "Перенести";
            this.MoveBtn.UseVisualStyleBackColor = true;
            this.MoveBtn.Click += new System.EventHandler(this.MoveBtn_Click);
            // 
            // RotateAngle
            // 
            this.RotateAngle.Location = new System.Drawing.Point(9, 280);
            this.RotateAngle.Maximum = 180;
            this.RotateAngle.Minimum = -180;
            this.RotateAngle.Name = "RotateAngle";
            this.RotateAngle.Size = new System.Drawing.Size(141, 45);
            this.RotateAngle.TabIndex = 17;
            this.RotateAngle.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // angle_label
            // 
            this.angle_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.angle_label.AutoSize = true;
            this.angle_label.Location = new System.Drawing.Point(122, 263);
            this.angle_label.MinimumSize = new System.Drawing.Size(28, 13);
            this.angle_label.Name = "angle_label";
            this.angle_label.Size = new System.Drawing.Size(28, 13);
            this.angle_label.TabIndex = 18;
            this.angle_label.Text = "0";
            this.angle_label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RotateBtn
            // 
            this.RotateBtn.Enabled = false;
            this.RotateBtn.Location = new System.Drawing.Point(75, 311);
            this.RotateBtn.Name = "RotateBtn";
            this.RotateBtn.Size = new System.Drawing.Size(75, 23);
            this.RotateBtn.TabIndex = 19;
            this.RotateBtn.Text = "Повернуть";
            this.RotateBtn.UseVisualStyleBackColor = true;
            this.RotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 339);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Масштабирование";
            // 
            // ScaleVal
            // 
            this.ScaleVal.Location = new System.Drawing.Point(9, 355);
            this.ScaleVal.Maximum = 400;
            this.ScaleVal.Name = "ScaleVal";
            this.ScaleVal.Size = new System.Drawing.Size(141, 45);
            this.ScaleVal.TabIndex = 21;
            this.ScaleVal.Value = 100;
            this.ScaleVal.ValueChanged += new System.EventHandler(this.ScaleVal_ValueChanged);
            // 
            // scaleLabel
            // 
            this.scaleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(122, 339);
            this.scaleLabel.MinimumSize = new System.Drawing.Size(28, 13);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(28, 13);
            this.scaleLabel.TabIndex = 22;
            this.scaleLabel.Text = "100";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(45, 388);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 23);
            this.button2.TabIndex = 23;
            this.button2.Text = "Масштабировать";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 419);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "Выбор точки";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // PointForAffineLabel
            // 
            this.PointForAffineLabel.AutoSize = true;
            this.PointForAffineLabel.Location = new System.Drawing.Point(12, 445);
            this.PointForAffineLabel.Name = "PointForAffineLabel";
            this.PointForAffineLabel.Size = new System.Drawing.Size(88, 13);
            this.PointForAffineLabel.TabIndex = 25;
            this.PointForAffineLabel.Text = "Не установлена";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 554);
            this.Controls.Add(this.PointForAffineLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.ScaleVal);
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
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
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
        private System.Windows.Forms.TrackBar ScaleVal;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label PointForAffineLabel;
    }
}

