﻿using System;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Drawing;
namespace AppProyecto
{
  public partial class frmReproductor : Form
  {
    public frmReproductor()
    {
      InitializeComponent();
      btnCerrar.Visible = true;
    }
    string[] rutas, nombres;
    private void siguiente()
    {
      if (LbPlaylist.SelectedIndex < LbPlaylist.Items.Count - 1)
      {
        LbPlaylist.SelectedIndex = LbPlaylist.SelectedIndex + 1; 
      }
    }
    private void anterior()
    {
      if (LbPlaylist.SelectedIndex > 0)
      { LbPlaylist.SelectedIndex = LbPlaylist.SelectedIndex - 1; }
    }
    private void LbPlaylist_SelectedIndexChanged(object sender, EventArgs e)
    {
      vlcControl1.Play(new Uri(rutas[LbPlaylist.SelectedIndex])); 

      TagLib.File file = TagLib.File.Create(rutas[LbPlaylist.SelectedIndex]);
      var mStream = new MemoryStream();
      var firstPicture = file.Tag.Pictures.FirstOrDefault();
      if (firstPicture != null)
      {
        byte[] pData = firstPicture.Data.Data;
        mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
        var bm = new Bitmap(mStream, false);
        mStream.Dispose();
        pbAlbum.Image = bm;
        pbAlbum.SizeMode = PictureBoxSizeMode.StretchImage;
      }
      timer1.Start();
    }
    private void btnSiguiente_Click_1(object sender, EventArgs e)
    {
        siguiente();      
    }
    private void btnAleatorio_Click_1(object sender, EventArgs e)
    {
        LbPlaylist.Items.Clear();
        Random rand1 = new Random();
      try
      {
        rutas = rutas.OrderBy(rutas => rand1.Next()).ToArray();
      }
      catch (Exception)
      { }
        for (int y = 0; y < rutas.Length; y++)
        {
          nombres[y] = Path.GetFileName(rutas[y]);
          LbPlaylist.Items.Add(nombres[y]);
        } 
    }
    private void btnPausa_Click(object sender, EventArgs e)
    {
      vlcControl1.Pause();
    }
    private void btnAnterior_Click(object sender, EventArgs e)
    {
      anterior();
    }
    private void timer1_Tick(object sender, EventArgs e)
    {
      if (vlcControl1.IsPlaying)
      {
        sbTiempo.Maximum = (int) vlcControl1.Length / 1000; 
        sbTiempo.Value = (int) vlcControl1.Time /1000; 
      }
      TimeSpan t = TimeSpan.FromMilliseconds(vlcControl1.Time);
      string time = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
      lbTime.Text = time;
      TimeSpan t2 = TimeSpan.FromMilliseconds(vlcControl1.Length);
      string totalTime = string.Format("{0:D2}:{1:D2}", t2.Minutes, t2.Seconds);
      lbTotalTime.Text = totalTime;
      if (sbTiempo.Value >= sbTiempo.Maximum - 2)
      { siguiente(); }
    }
    private void sbTiempo_Scroll(object sender, ScrollEventArgs e)
    {
      vlcControl1.Time = sbTiempo.Value * 1000;
    }
    private void sbVolumen_Scroll(object sender, ScrollEventArgs e)
    {
      int nuevoValor = sbVolumen.Value;
      vlcControl1.Audio.Volume = nuevoValor;
    }
    private void pbReturn_Click(object sender, EventArgs e)
    {
      Form home = new frmMenuDoc();
      home.ShowDialog();
    }
    private void btnAbrir_Click_1(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Audio|*.mp3;*.wmv;*.wav;*.flac;*.m4a;*.jpg*;*.mp4;";
      openFileDialog.Multiselect = true;
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        nombres = openFileDialog.SafeFileNames;
        rutas = openFileDialog.FileNames;
        btnAleatorio.Visible = true;
        btnSiguiente.Visible = true;
        vlcControl1.Visible = true;
        btnAnterior.Visible = true;
        LbPlaylist.Visible = true;
        btnPausa.Visible = true;
        for (int i = 0; i < nombres.Length; i++)
        {
          LbPlaylist.Items.Add(nombres[i]);
          vlcControl1.Play(new Uri(rutas[0]));
        }
      }
    }
    private void btnCerrar_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
