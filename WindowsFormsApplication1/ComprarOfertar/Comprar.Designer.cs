namespace MercadoEnvio.Comprar_Ofertar
{
    partial class Comprar
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
            this.labelMensaje = new System.Windows.Forms.Label();
            this.textBoxCant = new System.Windows.Forms.TextBox();
            this.botonConfirmarCompra = new System.Windows.Forms.Button();
            this.botonCancelar = new System.Windows.Forms.Button();
            this.groupBoxVendedor = new System.Windows.Forms.GroupBox();
            this.labelNumDniCuit = new System.Windows.Forms.Label();
            this.groupBoxDireccion = new System.Windows.Forms.GroupBox();
            this.labelPiso = new System.Windows.Forms.Label();
            this.labelNumCalle = new System.Windows.Forms.Label();
            this.labelTel = new System.Windows.Forms.Label();
            this.labelMail = new System.Windows.Forms.Label();
            this.labelLocalidad = new System.Windows.Forms.Label();
            this.labelPostal = new System.Windows.Forms.Label();
            this.labelDepartamento = new System.Windows.Forms.Label();
            this.labelCalle = new System.Windows.Forms.Label();
            this.labelTelefono = new System.Windows.Forms.Label();
            this.labelDniCuit = new System.Windows.Forms.Label();
            this.labelNombre = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxVendedor.SuspendLayout();
            this.groupBoxDireccion.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMensaje
            // 
            this.labelMensaje.AutoSize = true;
            this.labelMensaje.Location = new System.Drawing.Point(70, 37);
            this.labelMensaje.Name = "labelMensaje";
            this.labelMensaje.Size = new System.Drawing.Size(191, 13);
            this.labelMensaje.TabIndex = 0;
            this.labelMensaje.Text = "Ingrese la cantidad que desea comprar";
            // 
            // textBoxCant
            // 
            this.textBoxCant.Location = new System.Drawing.Point(107, 78);
            this.textBoxCant.Name = "textBoxCant";
            this.textBoxCant.Size = new System.Drawing.Size(106, 20);
            this.textBoxCant.TabIndex = 1;
            // 
            // botonConfirmarCompra
            // 
            this.botonConfirmarCompra.Location = new System.Drawing.Point(172, 447);
            this.botonConfirmarCompra.Name = "botonConfirmarCompra";
            this.botonConfirmarCompra.Size = new System.Drawing.Size(118, 41);
            this.botonConfirmarCompra.TabIndex = 2;
            this.botonConfirmarCompra.Text = "Confirmar Compra";
            this.botonConfirmarCompra.UseVisualStyleBackColor = true;
            this.botonConfirmarCompra.Click += new System.EventHandler(this.buttonConfirmarCompra_Click);
            // 
            // botonCancelar
            // 
            this.botonCancelar.Location = new System.Drawing.Point(29, 447);
            this.botonCancelar.Name = "botonCancelar";
            this.botonCancelar.Size = new System.Drawing.Size(110, 41);
            this.botonCancelar.TabIndex = 3;
            this.botonCancelar.Text = "Cancelar";
            this.botonCancelar.UseVisualStyleBackColor = true;
            this.botonCancelar.Click += new System.EventHandler(this.botonCancelar_Click);
            // 
            // groupBoxVendedor
            // 
            this.groupBoxVendedor.Controls.Add(this.labelNumDniCuit);
            this.groupBoxVendedor.Controls.Add(this.groupBoxDireccion);
            this.groupBoxVendedor.Controls.Add(this.labelTelefono);
            this.groupBoxVendedor.Controls.Add(this.labelDniCuit);
            this.groupBoxVendedor.Controls.Add(this.labelNombre);
            this.groupBoxVendedor.Location = new System.Drawing.Point(24, 136);
            this.groupBoxVendedor.Name = "groupBoxVendedor";
            this.groupBoxVendedor.Size = new System.Drawing.Size(282, 305);
            this.groupBoxVendedor.TabIndex = 4;
            this.groupBoxVendedor.TabStop = false;
            this.groupBoxVendedor.Text = "Datos del Vendedor";
            // 
            // labelNumDniCuit
            // 
            this.labelNumDniCuit.AutoSize = true;
            this.labelNumDniCuit.Location = new System.Drawing.Point(23, 69);
            this.labelNumDniCuit.Name = "labelNumDniCuit";
            this.labelNumDniCuit.Size = new System.Drawing.Size(35, 13);
            this.labelNumDniCuit.TabIndex = 5;
            this.labelNumDniCuit.Text = "label3";
            // 
            // groupBoxDireccion
            // 
            this.groupBoxDireccion.Controls.Add(this.label1);
            this.groupBoxDireccion.Controls.Add(this.label2);
            this.groupBoxDireccion.Controls.Add(this.label3);
            this.groupBoxDireccion.Controls.Add(this.label4);
            this.groupBoxDireccion.Controls.Add(this.label5);
            this.groupBoxDireccion.Controls.Add(this.label6);
            this.groupBoxDireccion.Controls.Add(this.label7);
            this.groupBoxDireccion.Controls.Add(this.label8);
            this.groupBoxDireccion.Controls.Add(this.labelPiso);
            this.groupBoxDireccion.Controls.Add(this.labelNumCalle);
            this.groupBoxDireccion.Controls.Add(this.labelTel);
            this.groupBoxDireccion.Controls.Add(this.labelMail);
            this.groupBoxDireccion.Controls.Add(this.labelLocalidad);
            this.groupBoxDireccion.Controls.Add(this.labelPostal);
            this.groupBoxDireccion.Controls.Add(this.labelDepartamento);
            this.groupBoxDireccion.Controls.Add(this.labelCalle);
            this.groupBoxDireccion.Location = new System.Drawing.Point(6, 85);
            this.groupBoxDireccion.Name = "groupBoxDireccion";
            this.groupBoxDireccion.Size = new System.Drawing.Size(270, 214);
            this.groupBoxDireccion.TabIndex = 4;
            this.groupBoxDireccion.TabStop = false;
            this.groupBoxDireccion.Text = "Contacto";
            // 
            // labelPiso
            // 
            this.labelPiso.AutoSize = true;
            this.labelPiso.Location = new System.Drawing.Point(74, 117);
            this.labelPiso.Name = "labelPiso";
            this.labelPiso.Size = new System.Drawing.Size(35, 13);
            this.labelPiso.TabIndex = 9;
            this.labelPiso.Text = "label8";
            // 
            // labelNumCalle
            // 
            this.labelNumCalle.AutoSize = true;
            this.labelNumCalle.Location = new System.Drawing.Point(74, 94);
            this.labelNumCalle.Name = "labelNumCalle";
            this.labelNumCalle.Size = new System.Drawing.Size(35, 13);
            this.labelNumCalle.TabIndex = 8;
            this.labelNumCalle.Text = "label7";
            // 
            // labelTel
            // 
            this.labelTel.AutoSize = true;
            this.labelTel.Location = new System.Drawing.Point(74, 51);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(35, 13);
            this.labelTel.TabIndex = 7;
            this.labelTel.Text = "label5";
            // 
            // labelMail
            // 
            this.labelMail.AutoSize = true;
            this.labelMail.Location = new System.Drawing.Point(74, 27);
            this.labelMail.Name = "labelMail";
            this.labelMail.Size = new System.Drawing.Size(35, 13);
            this.labelMail.TabIndex = 6;
            this.labelMail.Text = "label4";
            // 
            // labelLocalidad
            // 
            this.labelLocalidad.AutoSize = true;
            this.labelLocalidad.Location = new System.Drawing.Point(74, 160);
            this.labelLocalidad.Name = "labelLocalidad";
            this.labelLocalidad.Size = new System.Drawing.Size(41, 13);
            this.labelLocalidad.TabIndex = 3;
            this.labelLocalidad.Text = "label10";
            // 
            // labelPostal
            // 
            this.labelPostal.AutoSize = true;
            this.labelPostal.Location = new System.Drawing.Point(74, 179);
            this.labelPostal.Name = "labelPostal";
            this.labelPostal.Size = new System.Drawing.Size(41, 13);
            this.labelPostal.TabIndex = 2;
            this.labelPostal.Text = "label11";
            // 
            // labelDepartamento
            // 
            this.labelDepartamento.AutoSize = true;
            this.labelDepartamento.Location = new System.Drawing.Point(73, 139);
            this.labelDepartamento.Name = "labelDepartamento";
            this.labelDepartamento.Size = new System.Drawing.Size(35, 13);
            this.labelDepartamento.TabIndex = 1;
            this.labelDepartamento.Text = "label9";
            // 
            // labelCalle
            // 
            this.labelCalle.AutoSize = true;
            this.labelCalle.Location = new System.Drawing.Point(73, 73);
            this.labelCalle.Name = "labelCalle";
            this.labelCalle.Size = new System.Drawing.Size(35, 13);
            this.labelCalle.TabIndex = 0;
            this.labelCalle.Text = "label6";
            // 
            // labelTelefono
            // 
            this.labelTelefono.AutoSize = true;
            this.labelTelefono.Location = new System.Drawing.Point(23, 69);
            this.labelTelefono.Name = "labelTelefono";
            this.labelTelefono.Size = new System.Drawing.Size(0, 13);
            this.labelTelefono.TabIndex = 2;
            // 
            // labelDniCuit
            // 
            this.labelDniCuit.AutoSize = true;
            this.labelDniCuit.Location = new System.Drawing.Point(23, 48);
            this.labelDniCuit.Name = "labelDniCuit";
            this.labelDniCuit.Size = new System.Drawing.Size(35, 13);
            this.labelDniCuit.TabIndex = 1;
            this.labelDniCuit.Text = "label2";
            // 
            // labelNombre
            // 
            this.labelNombre.AutoSize = true;
            this.labelNombre.Location = new System.Drawing.Point(23, 25);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(35, 13);
            this.labelNombre.TabIndex = 0;
            this.labelNombre.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Piso";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Numero";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Telefono";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Mail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Localidad";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "CP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Depto";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Calle";
            // 
            // Comprar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 500);
            this.Controls.Add(this.groupBoxVendedor);
            this.Controls.Add(this.botonCancelar);
            this.Controls.Add(this.botonConfirmarCompra);
            this.Controls.Add(this.textBoxCant);
            this.Controls.Add(this.labelMensaje);
            this.Name = "Comprar";
            this.Text = "Comprar";
            this.Load += new System.EventHandler(this.Comprar_Load);
            this.groupBoxVendedor.ResumeLayout(false);
            this.groupBoxVendedor.PerformLayout();
            this.groupBoxDireccion.ResumeLayout(false);
            this.groupBoxDireccion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMensaje;
        private System.Windows.Forms.TextBox textBoxCant;
        private System.Windows.Forms.Button botonConfirmarCompra;
        private System.Windows.Forms.Button botonCancelar;
        private System.Windows.Forms.GroupBox groupBoxVendedor;
        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.Label labelTelefono;
        private System.Windows.Forms.Label labelDniCuit;
        private System.Windows.Forms.GroupBox groupBoxDireccion;
        private System.Windows.Forms.Label labelLocalidad;
        private System.Windows.Forms.Label labelPostal;
        private System.Windows.Forms.Label labelDepartamento;
        private System.Windows.Forms.Label labelCalle;
        private System.Windows.Forms.Label labelNumDniCuit;
        private System.Windows.Forms.Label labelMail;
        private System.Windows.Forms.Label labelTel;
        private System.Windows.Forms.Label labelNumCalle;
        private System.Windows.Forms.Label labelPiso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;

    }
}