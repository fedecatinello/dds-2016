namespace MercadoEnvio.Historial_Cliente
{
    partial class Historial
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_opciones = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Buscar = new System.Windows.Forms.Button();
            this.button_Limpiar = new System.Windows.Forms.Button();
            this.button_Cancelar = new System.Windows.Forms.Button();
            this.dataGridView_Historial = new System.Windows.Forms.DataGridView();
            this.botonPrimeraPagina = new System.Windows.Forms.Button();
            this.botonPaginaAnterior = new System.Windows.Forms.Button();
            this.botonPaginaSiguiente = new System.Windows.Forms.Button();
            this.botonUltimaPagina = new System.Windows.Forms.Button();
            this.labelNrosPagina = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Historial)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_opciones);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro de busquedas";
            // 
            // comboBox_opciones
            // 
            this.comboBox_opciones.FormattingEnabled = true;
            this.comboBox_opciones.Location = new System.Drawing.Point(119, 19);
            this.comboBox_opciones.Name = "comboBox_opciones";
            this.comboBox_opciones.Size = new System.Drawing.Size(243, 21);
            this.comboBox_opciones.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "¿De que queres ver?";
            // 
            // button_Buscar
            // 
            this.button_Buscar.Location = new System.Drawing.Point(280, 119);
            this.button_Buscar.Name = "button_Buscar";
            this.button_Buscar.Size = new System.Drawing.Size(100, 30);
            this.button_Buscar.TabIndex = 1;
            this.button_Buscar.Text = "Buscar";
            this.button_Buscar.UseVisualStyleBackColor = true;
            this.button_Buscar.Click += new System.EventHandler(this.button_Buscar_Click);
            // 
            // button_Limpiar
            // 
            this.button_Limpiar.Location = new System.Drawing.Point(175, 119);
            this.button_Limpiar.Name = "button_Limpiar";
            this.button_Limpiar.Size = new System.Drawing.Size(100, 30);
            this.button_Limpiar.TabIndex = 2;
            this.button_Limpiar.Text = "Limpiar";
            this.button_Limpiar.UseVisualStyleBackColor = true;
            this.button_Limpiar.Click += new System.EventHandler(this.button_Limpiar_Click);
            // 
            // button_Cancelar
            // 
            this.button_Cancelar.Location = new System.Drawing.Point(12, 119);
            this.button_Cancelar.Name = "button_Cancelar";
            this.button_Cancelar.Size = new System.Drawing.Size(100, 30);
            this.button_Cancelar.TabIndex = 3;
            this.button_Cancelar.Text = "Cancelar";
            this.button_Cancelar.UseVisualStyleBackColor = true;
            this.button_Cancelar.Click += new System.EventHandler(this.button_Cancelar_Click);
            // 
            // dataGridView_Historial
            // 
            this.dataGridView_Historial.AllowUserToAddRows = false;
            this.dataGridView_Historial.AllowUserToDeleteRows = false;
            this.dataGridView_Historial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_Historial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Historial.Location = new System.Drawing.Point(12, 155);
            this.dataGridView_Historial.Name = "dataGridView_Historial";
            this.dataGridView_Historial.ReadOnly = true;
            this.dataGridView_Historial.Size = new System.Drawing.Size(1068, 199);
            this.dataGridView_Historial.TabIndex = 4;
            // 
            // botonPrimeraPagina
            // 
            this.botonPrimeraPagina.Location = new System.Drawing.Point(254, 360);
            this.botonPrimeraPagina.Name = "botonPrimeraPagina";
            this.botonPrimeraPagina.Size = new System.Drawing.Size(92, 23);
            this.botonPrimeraPagina.TabIndex = 5;
            this.botonPrimeraPagina.Text = "Primera";
            this.botonPrimeraPagina.UseVisualStyleBackColor = true;
            this.botonPrimeraPagina.Click += new System.EventHandler(this.botonPrimeraPagina_Click);
            // 
            // botonPaginaAnterior
            // 
            this.botonPaginaAnterior.Location = new System.Drawing.Point(463, 360);
            this.botonPaginaAnterior.Name = "botonPaginaAnterior";
            this.botonPaginaAnterior.Size = new System.Drawing.Size(75, 23);
            this.botonPaginaAnterior.TabIndex = 6;
            this.botonPaginaAnterior.Text = "Anterior";
            this.botonPaginaAnterior.UseVisualStyleBackColor = true;
            this.botonPaginaAnterior.Click += new System.EventHandler(this.botonPaginaAnterior_Click);
            // 
            // botonPaginaSiguiente
            // 
            this.botonPaginaSiguiente.Location = new System.Drawing.Point(577, 360);
            this.botonPaginaSiguiente.Name = "botonPaginaSiguiente";
            this.botonPaginaSiguiente.Size = new System.Drawing.Size(75, 23);
            this.botonPaginaSiguiente.TabIndex = 7;
            this.botonPaginaSiguiente.Text = "Siguiente";
            this.botonPaginaSiguiente.UseVisualStyleBackColor = true;
            this.botonPaginaSiguiente.Click += new System.EventHandler(this.botonPaginaSiguiente_Click);
            // 
            // botonUltimaPagina
            // 
            this.botonUltimaPagina.Location = new System.Drawing.Point(784, 360);
            this.botonUltimaPagina.Name = "botonUltimaPagina";
            this.botonUltimaPagina.Size = new System.Drawing.Size(75, 23);
            this.botonUltimaPagina.TabIndex = 8;
            this.botonUltimaPagina.Text = "Ultima";
            this.botonUltimaPagina.UseVisualStyleBackColor = true;
            this.botonUltimaPagina.Click += new System.EventHandler(this.botonUltimaPagina_Click);
            // 
            // labelNrosPagina
            // 
            this.labelNrosPagina.AutoSize = true;
            this.labelNrosPagina.Location = new System.Drawing.Point(944, 365);
            this.labelNrosPagina.Name = "labelNrosPagina";
            this.labelNrosPagina.Size = new System.Drawing.Size(40, 13);
            this.labelNrosPagina.TabIndex = 9;
            this.labelNrosPagina.Text = "Pagina";
            // 
            // Historial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 394);
            this.Controls.Add(this.labelNrosPagina);
            this.Controls.Add(this.botonUltimaPagina);
            this.Controls.Add(this.botonPaginaSiguiente);
            this.Controls.Add(this.botonPaginaAnterior);
            this.Controls.Add(this.botonPrimeraPagina);
            this.Controls.Add(this.dataGridView_Historial);
            this.Controls.Add(this.button_Cancelar);
            this.Controls.Add(this.button_Limpiar);
            this.Controls.Add(this.button_Buscar);
            this.Controls.Add(this.groupBox1);
            this.Name = "Historial";
            this.Text = "Historial";
            this.Load += new System.EventHandler(this.Historial_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Historial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Buscar;
        private System.Windows.Forms.Button button_Limpiar;
        private System.Windows.Forms.Button button_Cancelar;
        private System.Windows.Forms.DataGridView dataGridView_Historial;
        private System.Windows.Forms.ComboBox comboBox_opciones;
        private System.Windows.Forms.Button botonPrimeraPagina;
        private System.Windows.Forms.Button botonPaginaAnterior;
        private System.Windows.Forms.Button botonPaginaSiguiente;
        private System.Windows.Forms.Button botonUltimaPagina;
        private System.Windows.Forms.Label labelNrosPagina;
    }
}