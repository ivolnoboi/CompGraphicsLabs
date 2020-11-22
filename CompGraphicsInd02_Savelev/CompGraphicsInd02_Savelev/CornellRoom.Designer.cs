namespace CompGraphicsInd02_Savelev
{
    partial class CornellRoom
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
            this.room = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.room)).BeginInit();
            this.SuspendLayout();
            // 
            // room
            // 
            this.room.Location = new System.Drawing.Point(13, 13);
            this.room.Name = "room";
            this.room.Size = new System.Drawing.Size(800, 600);
            this.room.TabIndex = 0;
            this.room.TabStop = false;
            // 
            // CornellRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 625);
            this.Controls.Add(this.room);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CornellRoom";
            this.Text = "CornellRoom";
            ((System.ComponentModel.ISupportInitialize)(this.room)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox room;
    }
}

