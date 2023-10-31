using Parcial2023.Datos.Servicios;
using Parcial2023.Dominio;
using Produccion.Domino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Produccion.Presentacion
{
    public partial class FrmAlta : Form
    {
        private OrdenProduccion orden;
        private GestorOrden gestorOrden;
        private GestorComponentes gestorComponentes;
        public FrmAlta()
        {
            InitializeComponent();
            orden = new OrdenProduccion();
            gestorOrden= new GestorOrden();
            gestorComponentes = new GestorComponentes();
            
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void FrmAlta_Load(object sender, EventArgs e)
        {
            CargarCombo();
            cboComponente.SelectedIndex = -1;
        }

        public void CargarCombo()
        {
            List<Componente> lista = gestorComponentes.Consultar();
            cboComponente.DataSource = lista;
            cboComponente.ValueMember = "codigo";
            cboComponente.DisplayMember = "nombre";
            cboComponente.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public bool Validacion()
        {
            if (string.IsNullOrEmpty(txtDT.Text))
            {
                MessageBox.Show("Debe ingresar un Modelo");
                txtDT.Focus();
                return false;
            }
            if(dtpFecha.Value < DateTime.Now.AddDays(-1))
            {
                MessageBox.Show("La fecha de la orden no puede ser menor que la actual");
                return false;
            }
            if(cboComponente.SelectedIndex == -1)
            {
                MessageBox.Show("Debe elegir un componente");
                return false;
            }

            foreach(DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["colComponente"].Value.ToString().Equals(cboComponente.Text))
                {
                    MessageBox.Show("Componente ya ingresado");
                    return false;
                }
            }
            return true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(Validacion())
            {
                Componente c = (Componente)cboComponente.SelectedItem;
                DetalleOrden det = new DetalleOrden(c, Convert.ToInt32(numericUpDown1.Value));
                orden.Agregar(det);

                dgvDetalles.Rows.Add(new object[] 
                {
                    c.Nombre,
                    Convert.ToInt32(nudCantidad.Value),
                    (nudCantidad.Value * numericUpDown1.Value),
                    "Quitar"
                });
            }
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvDetalles.CurrentCell.ColumnIndex == 3)
            {
                dgvDetalles.Rows.RemoveAt(dgvDetalles.CurrentCell.ColumnIndex);
                orden.Quitar(dgvDetalles.CurrentCell.ColumnIndex);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvDetalles.Rows.Count < 2)
            {
                MessageBox.Show("El modelo debe tener como mínimo dos componentes!");
                return;
            }

            orden.Fecha = dtpFecha.Value;
            orden.Modelo = txtDT.Text;
            orden.Cantidad = Convert.ToInt32(nudCantidad.Value);
            orden.Estado = "Creada";

            if (gestorOrden.Confirmar(orden))
            {
                MessageBox.Show("Orden confirmada!");
            }
            else
            {
                MessageBox.Show("Ocurrió un error");
            }
        }
    }
}
