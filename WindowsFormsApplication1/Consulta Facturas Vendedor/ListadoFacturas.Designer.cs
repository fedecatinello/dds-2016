namespace MercadoEnvio.Consulta_Facturas_Vendedor
{
    partial class ListadoFacturas
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
            this.label1 = new System.Windows.Forms.Label();
            this.botonVolver = new System.Windows.Forms.Button();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.labelRubro = new System.Windows.Forms.Label();
            this.labelDescricpion = new System.Windows.Forms.Label();
            this.botonUltimaPagina = new System.Windows.Forms.Button();
            this.botonPaginaSiguiente = new System.Windows.Forms.Button();
            this.botonPaginaAnterior = new System.Windows.Forms.Button();
            this.botonPrimeraPagina = new System.Windows.Forms.Button();
            this.botonBuscar = new System.Windows.Forms.Button();
            this.botonLimpiar = new System.Windows.Forms.Button();
            this.dataGridViewFacturas = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_FechaHasta = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.monthCalendar_FechaHasta = new System.Windows.Forms.MonthCalendar();
            this.button_FechaHasta = new System.Windows.Forms.Button();
            this.labelNrosPagina = new System.Windows.Forms.Label();
            this.textBox_ImporteHasta = new System.Windows.Forms.TextBox();
            this.textBox_ImporteDesde = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.monthCalendar_FechaDesde = new System.Windows.Forms.MonthCalendar();
            this.button_FechaDesde = new System.Windows.Forms.Button();
            this.textBox_FechaDesde = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFacturas)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(78, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(416, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Consulta de facturas realizadas al vendedor";
            // 
            // botonVolver
            // 
            this.botonVolver.Location = new System.Drawing.Point(13, 578);
            this.botonVolver.Name = "botonVolver";
            this.botonVolver.Size = new System.Drawing.Size(170, 31);
            this.botonVolver.TabIndex = 26;
            this.botonVolver.Text = "< Volver al Menú Principal";
            this.botonVolver.UseVisualStyleBackColor = true;
            this.botonVolver.Click += new System.EventHandler(this.botonVolver_Click);
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(174, -78);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(195, 20);
            this.textBoxDescripcion.TabIndex = 24;
            // 
            // labelRubro
            // 
            this.labelRubro.AutoSize = true;
            this.labelRubro.Location = new System.Drawing.Point(94, -48);
            this.labelRubro.Name = "labelRubro";
            this.labelRubro.Size = new System.Drawing.Size(45, 13);
            this.labelRubro.TabIndex = 23;
            this.labelRubro.Text = "Rubro 1";
            // 
            // labelDescricpion
            // 
            this.labelDescricpion.AutoSize = true;
            this.labelDescricpion.Location = new System.Drawing.Point(94, -78);
            this.labelDescricpion.Name = "labelDescricpion";
            this.labelDescricpion.Size = new System.Drawing.Size(63, 13);
            this.labelDescricpion.TabIndex = 22;
            this.labelDescricpion.Text = "Descripcion";
            // 
            // botonUltimaPagina
            // 
            this.botonUltimaPagina.Location = new System.Drawing.Point(491, 529);
            this.botonUltimaPagina.Name = "botonUltimaPagina";
            this.botonUltimaPagina.Size = new System.Drawing.Size(67, 25);
            this.botonUltimaPagina.TabIndex = 21;
            this.botonUltimaPagina.Text = "Ultima";
            this.botonUltimaPagina.UseVisualStyleBackColor = true;
            this.botonUltimaPagina.Click += new System.EventHandler(this.botonUltimaPagina_Click);
            // 
            // botonPaginaSiguiente
            // 
            this.botonPaginaSiguiente.Location = new System.Drawing.Point(404, 528);
            this.botonPaginaSiguiente.Name = "botonPaginaSiguiente";
            this.botonPaginaSiguiente.Size = new System.Drawing.Size(66, 25);
            this.botonPaginaSiguiente.TabIndex = 20;
            this.botonPaginaSiguiente.Text = "Siguiente";
            this.botonPaginaSiguiente.UseVisualStyleBackColor = true;
            this.botonPaginaSiguiente.Click += new System.EventHandler(this.botonPaginaSiguiente_Click);
            // 
            // botonPaginaAnterior
            // 
            this.botonPaginaAnterior.Location = new System.Drawing.Point(118, 528);
            this.botonPaginaAnterior.Name = "botonPaginaAnterior";
            this.botonPaginaAnterior.Size = new System.Drawing.Size(65, 25);
            this.botonPaginaAnterior.TabIndex = 19;
            this.botonPaginaAnterior.Text = "Anterior";
            this.botonPaginaAnterior.UseVisualStyleBackColor = true;
            this.botonPaginaAnterior.Click += new System.EventHandler(this.botonPaginaAnterior_Click);
            // 
            // botonPrimeraPagina
            // 
            this.botonPrimeraPagina.Location = new System.Drawing.Point(27, 528);
            this.botonPrimeraPagina.Name = "botonPrimeraPagina";
            this.botonPrimeraPagina.Size = new System.Drawing.Size(64, 26);
            this.botonPrimeraPagina.TabIndex = 18;
            this.botonPrimeraPagina.Text = "Primera";
            this.botonPrimeraPagina.UseVisualStyleBackColor = true;
            this.botonPrimeraPagina.Click += new System.EventHandler(this.botonPrimeraPagina_Click);
            // 
            // botonBuscar
            // 
            this.botonBuscar.Location = new System.Drawing.Point(383, 201);
            this.botonBuscar.Name = "botonBuscar";
            this.botonBuscar.Size = new System.Drawing.Size(122, 28);
            this.botonBuscar.TabIndex = 17;
            this.botonBuscar.Text = "Buscar";
            this.botonBuscar.UseVisualStyleBackColor = true;
            this.botonBuscar.Click += new System.EventHandler(this.botonBuscar_Click);
            // 
            // botonLimpiar
            // 
            this.botonLimpiar.Location = new System.Drawing.Point(82, 201);
            this.botonLimpiar.Name = "botonLimpiar";
            this.botonLimpiar.Size = new System.Drawing.Size(99, 28);
            this.botonLimpiar.TabIndex = 16;
            this.botonLimpiar.Text = "Limpiar";
            this.botonLimpiar.UseVisualStyleBackColor = true;
            this.botonLimpiar.Click += new System.EventHandler(this.botonLimpiar_Click);
            // 
            // dataGridViewFacturas
            // 
            this.dataGridViewFacturas.AllowUserToAddRows = false;
            this.dataGridViewFacturas.AllowUserToDeleteRows = false;
            this.dataGridViewFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFacturas.Location = new System.Drawing.Point(13, 245);
            this.dataGridViewFacturas.Name = "dataGridViewFacturas";
            this.dataGridViewFacturas.Size = new System.Drawing.Size(551, 278);
            this.dataGridViewFacturas.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Importe Hasta";
            // 
            // textBox_FechaHasta
            // 
            this.textBox_FechaHasta.Location = new System.Drawing.Point(383, 118);
            this.textBox_FechaHasta.Name = "textBox_FechaHasta";
            this.textBox_FechaHasta.Size = new System.Drawing.Size(102, 20);
            this.textBox_FechaHasta.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(304, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Fecha Hasta";
            // 
            // monthCalendar_FechaHasta
            // 
            this.monthCalendar_FechaHasta.Location = new System.Drawing.Point(337, 99);
            this.monthCalendar_FechaHasta.Name = "monthCalendar_FechaHasta";
            this.monthCalendar_FechaHasta.TabIndex = 36;
            this.monthCalendar_FechaHasta.Visible = false;
            this.monthCalendar_FechaHasta.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_FechaHasta_DateSelected);
            // 
            // button_FechaHasta
            // 
            this.button_FechaHasta.Location = new System.Drawing.Point(491, 117);
            this.button_FechaHasta.Name = "button_FechaHasta";
            this.button_FechaHasta.Size = new System.Drawing.Size(80, 20);
            this.button_FechaHasta.TabIndex = 37;
            this.button_FechaHasta.Text = "Seleccionar";
            this.button_FechaHasta.UseVisualStyleBackColor = true;
            this.button_FechaHasta.Click += new System.EventHandler(this.button_FechaHasta_Click);
            // 
            // labelNrosPagina
            // 
            this.labelNrosPagina.AutoSize = true;
            this.labelNrosPagina.Location = new System.Drawing.Point(380, 587);
            this.labelNrosPagina.Name = "labelNrosPagina";
            this.labelNrosPagina.Size = new System.Drawing.Size(35, 13);
            this.labelNrosPagina.TabIndex = 38;
            this.labelNrosPagina.Text = "label6";
            // 
            // textBox_ImporteHasta
            // 
            this.textBox_ImporteHasta.Location = new System.Drawing.Point(383, 74);
            this.textBox_ImporteHasta.Name = "textBox_ImporteHasta";
            this.textBox_ImporteHasta.Size = new System.Drawing.Size(165, 20);
            this.textBox_ImporteHasta.TabIndex = 39;
            // 
            // textBox_ImporteDesde
            // 
            this.textBox_ImporteDesde.Location = new System.Drawing.Point(103, 74);
            this.textBox_ImporteDesde.Name = "textBox_ImporteDesde";
            this.textBox_ImporteDesde.Size = new System.Drawing.Size(165, 20);
            this.textBox_ImporteDesde.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Importe Desde";
            // 
            // monthCalendar_FechaDesde
            // 
            this.monthCalendar_FechaDesde.Location = new System.Drawing.Point(13, 99);
            this.monthCalendar_FechaDesde.Name = "monthCalendar_FechaDesde";
            this.monthCalendar_FechaDesde.TabIndex = 44;
            this.monthCalendar_FechaDesde.Visible = false;
            this.monthCalendar_FechaDesde.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_FechaDesde_DateSelected);
            // 
            // button_FechaDesde
            // 
            this.button_FechaDesde.Location = new System.Drawing.Point(211, 117);
            this.button_FechaDesde.Name = "button_FechaDesde";
            this.button_FechaDesde.Size = new System.Drawing.Size(80, 20);
            this.button_FechaDesde.TabIndex = 45;
            this.button_FechaDesde.Text = "Seleccionar";
            this.button_FechaDesde.UseVisualStyleBackColor = true;
            this.button_FechaDesde.Click += new System.EventHandler(this.button_FechaDesde_Click);
            // 
            // textBox_FechaDesde
            // 
            this.textBox_FechaDesde.Location = new System.Drawing.Point(103, 118);
            this.textBox_FechaDesde.Name = "textBox_FechaDesde";
            this.textBox_FechaDesde.Size = new System.Drawing.Size(102, 20);
            this.textBox_FechaDesde.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "Fecha Desde";
            // 
            // ListadoFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 619);
            this.Controls.Add(this.monthCalendar_FechaDesde);
            this.Controls.Add(this.button_FechaDesde);
            this.Controls.Add(this.textBox_FechaDesde);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_ImporteDesde);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_ImporteHasta);
            this.Controls.Add(this.labelNrosPagina);
            this.Controls.Add(this.monthCalendar_FechaHasta);
            this.Controls.Add(this.button_FechaHasta);
            this.Controls.Add(this.textBox_FechaHasta);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.botonVolver);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.labelRubro);
            this.Controls.Add(this.labelDescricpion);
            this.Controls.Add(this.botonUltimaPagina);
            this.Controls.Add(this.botonPaginaSiguiente);
            this.Controls.Add(this.botonPaginaAnterior);
            this.Controls.Add(this.botonPrimeraPagina);
            this.Controls.Add(this.botonBuscar);
            this.Controls.Add(this.botonLimpiar);
            this.Controls.Add(this.dataGridViewFacturas);
            this.Controls.Add(this.label1);
            this.Name = "ListadoFacturas";
            this.Text = "ListadoFacturas";
            this.Load += new System.EventHandler(this.ListadoFacturas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFacturas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button botonVolver;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.Label labelRubro;
        private System.Windows.Forms.Label labelDescricpion;
        private System.Windows.Forms.Button botonUltimaPagina;
        private System.Windows.Forms.Button botonPaginaSiguiente;
        private System.Windows.Forms.Button botonPaginaAnterior;
        private System.Windows.Forms.Button botonPrimeraPagina;
        private System.Windows.Forms.Button botonBuscar;
        private System.Windows.Forms.Button botonLimpiar;
        private System.Windows.Forms.DataGridView dataGridViewFacturas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_FechaHasta;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MonthCalendar monthCalendar_FechaHasta;
        private System.Windows.Forms.Button button_FechaHasta;
        private System.Windows.Forms.Label labelNrosPagina;
        private System.Windows.Forms.TextBox textBox_ImporteHasta;
        private System.Windows.Forms.TextBox textBox_ImporteDesde;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MonthCalendar monthCalendar_FechaDesde;
        private System.Windows.Forms.Button button_FechaDesde;
        private System.Windows.Forms.TextBox textBox_FechaDesde;
        private System.Windows.Forms.Label label4;
    }
}