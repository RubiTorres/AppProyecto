﻿using System;
using MySqlConnector;
using System.Windows.Forms;
namespace AppProyecto
{
  public partial class frmInicioSesion : Form
  {
    frmMenu a;
    MySqlConnection cnx;
    public frmInicioSesion(frmMenu frmenu)
    {
      InitializeComponent();
      a = frmenu;
      string cadena = "server=localhost;user=root;database=uyc;port=3306;Contraseña=";
      cnx = new MySqlConnection(cadena);
    }
    private void btnInicioSesion_Click_1(object sender, EventArgs e)
    {
      cnx.Open();
      string sql = "SELECT * FROM usuarios WHERE   Usuario='" + txtUsuario.Text + "' and Contraseña='" + txtContraseña.Text + "'";
      MySqlCommand cnd = new MySqlCommand(sql, cnx);
      object result = cnd.ExecuteScalar();
      if (result != null)
      {
        a.Opacity = 1;
        a.ShowInTaskbar = true;
        this.Close();
      }
      else
      {
        MessageBox.Show("Usuario o contraseñas incorrectos");
        txtUsuario.Clear();
        txtContraseña.Clear();
      }
      cnx.Close();
    }
  }
}
