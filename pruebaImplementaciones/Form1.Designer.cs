namespace pruebaImplementaciones
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ejecutar = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.siguiente = new System.Windows.Forms.Button();
            this.anterior = new System.Windows.Forms.Button();
            this.textoPaso = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1024, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Archivos de Imagen|*.bmp;*.jpg;*.jpge;*.gif;*.png";
            // 
            // ejecutar
            // 
            this.ejecutar.Location = new System.Drawing.Point(32, 578);
            this.ejecutar.Name = "ejecutar";
            this.ejecutar.Size = new System.Drawing.Size(75, 23);
            this.ejecutar.TabIndex = 2;
            this.ejecutar.Text = "Ejecutar";
            this.ejecutar.UseVisualStyleBackColor = true;
            this.ejecutar.Click += new System.EventHandler(this.ejecutar_Click);
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(12, 12);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(101, 23);
            this.reset.TabIndex = 3;
            this.reset.Text = "Reset";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(71, 617);
            this.trackBar.Maximum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(882, 42);
            this.trackBar.TabIndex = 4;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // siguiente
            // 
            this.siguiente.Location = new System.Drawing.Point(959, 617);
            this.siguiente.Name = "siguiente";
            this.siguiente.Size = new System.Drawing.Size(33, 23);
            this.siguiente.TabIndex = 5;
            this.siguiente.Text = ">";
            this.siguiente.UseVisualStyleBackColor = true;
            this.siguiente.Click += new System.EventHandler(this.siguiente_Click);
            // 
            // anterior
            // 
            this.anterior.Location = new System.Drawing.Point(32, 617);
            this.anterior.Name = "anterior";
            this.anterior.Size = new System.Drawing.Size(33, 23);
            this.anterior.TabIndex = 6;
            this.anterior.Text = "<";
            this.anterior.UseVisualStyleBackColor = true;
            this.anterior.Click += new System.EventHandler(this.anterior_Click);
            // 
            // textoPaso
            // 
            this.textoPaso.AutoSize = true;
            this.textoPaso.Location = new System.Drawing.Point(150, 578);
            this.textoPaso.Name = "textoPaso";
            this.textoPaso.Size = new System.Drawing.Size(78, 13);
            this.textoPaso.TabIndex = 7;
            this.textoPaso.Text = "Imagen original";
            this.textoPaso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.textoPaso.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 663);
            this.Controls.Add(this.textoPaso);
            this.Controls.Add(this.anterior);
            this.Controls.Add(this.siguiente);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.ejecutar);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Prueba Implementaciones";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button ejecutar;
        private System.Windows.Forms.Button reset;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Button siguiente;
        private System.Windows.Forms.Button anterior;
        private System.Windows.Forms.Label textoPaso;
    }
}

