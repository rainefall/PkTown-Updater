using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace PokemonTownUpdater
{
    public partial class Form1 : Form
    {
        WebClient wc;

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            wc.CancelAsync();
            // restore old data.win
            File.Move("data.win.bak", "data.win");
            Application.Exit();
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/PokemonTown/update.tmp");
            Application.Exit();
        }

        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Progress: " + e.BytesReceived + "/" + e.TotalBytesToReceive;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                // backup old data.win
                File.Move("data.win", "data.win.bak");
                // read update file
                StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+"/PokemonTown/update.tmp");
                // star download
                wc = new WebClient();
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(sr.ReadLine()),
                    // Param2 = Path to save
                    "data.win"
                );
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("\"update.tmp\" could not be found. \n", "An error has occurred.", MessageBoxButtons.OK);
                // restore old data.win
                File.Move("data.win.bak", "data.win");
                Application.Exit();
            }
        }
    }
}
