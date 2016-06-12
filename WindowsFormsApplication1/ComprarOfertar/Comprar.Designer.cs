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
            this.groupBoxDireccion = new System.Windows.Forms.GroupBox();
            this.labelLocalidad = new System.Windows.Forms.Label();
            this.labelPostal = new System.Windows.Forms.Label();
            this.labelDepartamento = new System.Windows.Forms.Label();
            this.labelCalle = new System.Windows.Forms.Label();
            this.labelTelefono = new System.Windows.Forms.Label();
            this.labelDniCuit = new System.Windows.Forms.Label();
            this.labelNombre = new System.Windows.Forms.Label();
            this.labelNumDniCuit = new System.Windows.Forms.Label();
            this.labelMail = new System.Windows.Forms.Label();
            this.labelTel = new System.Windows.Forms.Label();
            this.labelNumCalle = new System.Windows.Forms.Label();
            this.labelPiso = new System.Windows.Forms.Label();
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
            // groupBoxDireccion
            // 
            this.groupBoxDireccion.Controls.Add(this.labelPiso);
            this.groupBoxDireccion.Controls.Add(this.labelNumCalle);
            this.groupBoxDireccion.Controls.Add(this.labelTel);
            this.groupBoxDireccion.Controls.Add(this.labelMail);
            this.groupBoxDireccion.Controls.Add(this.labelLocalidad);
            this.groupBoxDireccion.Controls.Add(this.labelPostal);
            this.groupBoxDireccion.Controls.Add(this.labelDepartamento);
            this.groupBoxDireccion.Controls.Add(this.labelCalle);
            this.groupBoxDireccion.Location = new System.Drawing.Point(26, 85);
            this.groupBoxDireccion.Name = "groupBoxDireccion";
            this.groupBoxDireccion.Size = new System.Drawing.Size(230, 214);
            this.groupBoxDireccion.TabIndex = 4;
            this.groupBoxDireccion.TabStop = false;
            this.groupBoxDireccion.Text = "Contacto";
            // 
            // labelLocalidad
            // 
            this.labelLocalidad.AutoSize = true;
            this.labelLocalidad.Location = new System.Drawing.Point(20, 161);
            this.labelLocalidad.Name = "labelLocalidad";
            this.labelLocalidad.Size = new System.Drawing.Size(41, 13);
            this.labelLocalidad.TabIndex = 3;
            this.labelLocalidad.Text = "label10";
            // 
            // labelPostal
            // 
            this.labelPostal.AutoSize = true;
            this.labelPostal.Location = new System.Drawing.Point(20, 180);
            this.labelPostal.Name = "labelPostal";
            this.labelPostal.Size = new System.Drawing.Size(41, 13);
            this.labelPostal.TabIndex = 2;
            this.labelPostal.Text = "label11";
            // 
            // labelDepartamento
            // 
            this.labelDepartamento.AutoSize = true;
            this.labelDepartamento.Location = new System.Drawing.Point(19, 140);
            this.labelDepartamento.Name = "labelDepartamento";
            this.labelDepartamento.Size = new System.Drawing.Size(35, 13);
            this.labelDepartamento.TabIndex = 1;
            this.labelDepartamento.Text = "label9";
            // 
            // labelCalle
            // 
            this.labelCalle.AutoSize = true;
            this.labelCalle.Location = new System.Drawing.Point(19, 74);
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
            // labelNumDniCuit
            // 
            this.labelNumDniCuit.AutoSize = true;
            this.labelNumDniCuit.Location = new System.Drawing.Point(23, 69);
            this.labelNumDniCuit.Name = "labelNumDniCuit";
            this.labelNumDniCuit.Size = new System.Drawing.Size(35, 13);
            this.labelNumDniCuit.TabIndex = 5;
            this.labelNumDniCuit.Text = "label3";
            // 
            // labelMail
            // 
            this.labelMail.AutoSize = true;
            this.labelMail.Location = new System.Drawing.Point(20, 28);
            this.labelMail.Name = "labelMail";
            this.labelMail.Size = new System.Drawing.Size(35, 13);
            this.labelMail.TabIndex = 6;
            this.labelMail.Text = "label4";
            // 
            // labelTel
            // 
            this.labelTel.AutoSize = true;
            this.labelTel.Location = new System.Drawing.Point(20, 52);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(35, 13);
            this.labelTel.TabIndex = 7;
            this.labelTel.Text = "label5";
            // 
            // labelNumCalle
            // 
            this.labelNumCalle.AutoSize = true;
            this.labelNumCalle.Location = new System.Drawing.Point(20, 95);
            this.labelNumCalle.Name = "labelNumCalle";
            this.labelNumCalle.Size = new System.Drawing.Size(35, 13);
            this.labelNumCalle.TabIndex = 8;
            this.labelNumCalle.Text = "label7";
            // 
            // labelPiso
            // 
            this.labelPiso.AutoSize = true;
            this.labelPiso.Location = new System.Drawing.Point(20, 118);
            this.labelPiso.Name = "labelPiso";
            this.labelPiso.Size = new System.Drawing.Size(35, 13);
            this.labelPiso.TabIndex = 9;
            this.labelPiso.Text = "label8";
            this.labelPiso.Click += new System.EventHandler(this.label1_Click);
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

    }
}