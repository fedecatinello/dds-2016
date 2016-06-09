namespace MercadoEnvio.Editar_Publicacion
{
    partial class FiltrarPublicacion
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
            this.button_Buscar = new System.Windows.Forms.Button();
            this.button_Limpiar = new System.Windows.Forms.Button();
            this.button_Cancelar = new System.Windows.Forms.Button();
            this.dataGridView_Publicacion = new System.Windows.Forms.DataGridView();
            this.labelDescricpion = new System.Windows.Forms.Label();
            this.labelRubro = new System.Windows.Forms.Label();
            this.comboBoxRubro = new System.Windows.Forms.ComboBox();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Publicacion)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxRubro);
            this.groupBox1.Controls.Add(this.textBoxDescripcion);
            this.groupBox1.Controls.Add(this.labelRubro);
            this.groupBox1.Controls.Add(this.labelDescricpion);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 101);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro de busquedas";
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
            // dataGridView_Publicacion
            // 
            this.dataGridView_Publicacion.AllowUserToAddRows = false;
            this.dataGridView_Publicacion.AllowUserToDeleteRows = false;
            this.dataGridView_Publicacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_Publicacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Publicacion.Location = new System.Drawing.Point(12, 155);
            this.dataGridView_Publicacion.Name = "dataGridView_Publicacion";
            this.dataGridView_Publicacion.ReadOnly = true;
            this.dataGridView_Publicacion.Size = new System.Drawing.Size(1059, 199);
            this.dataGridView_Publicacion.TabIndex = 4;
            // 
            // labelDescricpion
            // 
            this.labelDescricpion.AutoSize = true;
            this.labelDescricpion.Location = new System.Drawing.Point(6, 27);
            this.labelDescricpion.Name = "labelDescricpion";
            this.labelDescricpion.Size = new System.Drawing.Size(63, 13);
            this.labelDescricpion.TabIndex = 9;
            this.labelDescricpion.Text = "Descripcion";
            // 
            // labelRubro
            // 
            this.labelRubro.AutoSize = true;
            this.labelRubro.Location = new System.Drawing.Point(6, 52);
            this.labelRubro.Name = "labelRubro";
            this.labelRubro.Size = new System.Drawing.Size(36, 13);
            this.labelRubro.TabIndex = 10;
            this.labelRubro.Text = "Rubro";
            // 
            // comboBoxRubro
            // 
            this.comboBoxRubro.FormattingEnabled = true;
            this.comboBoxRubro.Location = new System.Drawing.Point(87, 53);
            this.comboBoxRubro.Name = "comboBoxRubro";
            this.comboBoxRubro.Size = new System.Drawing.Size(195, 21);
            this.comboBoxRubro.TabIndex = 13;
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(87, 26);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(195, 20);
            this.textBoxDescripcion.TabIndex = 12;
            // 
            // FiltrarPublicacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1383, 369);
            this.Controls.Add(this.dataGridView_Publicacion);
            this.Controls.Add(this.button_Cancelar);
            this.Controls.Add(this.button_Limpiar);
            this.Controls.Add(this.button_Buscar);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiltrarPublicacion";
            this.Text = "Filtro de publicaciones";
            this.Load += new System.EventHandler(this.FiltrarPublicacion_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Publicacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Buscar;
        private System.Windows.Forms.Button button_Limpiar;
        private System.Windows.Forms.Button button_Cancelar;
        private System.Windows.Forms.DataGridView dataGridView_Publicacion;
        private System.Windows.Forms.Label labelDescricpion;
        private System.Windows.Forms.Label labelRubro;
        private System.Windows.Forms.ComboBox comboBoxRubro;
        private System.Windows.Forms.TextBox textBoxDescripcion;
    }
}