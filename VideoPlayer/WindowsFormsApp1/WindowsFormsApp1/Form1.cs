using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;
using System.IO;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		private VidePlayer controllerToPlayer;
		public Form1()
		{
			InitializeComponent();
			controllerToPlayer = new VidePlayer(this);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			axWindowsMediaPlayer1.URL = @"C:\Hollow Knight.avi";
		}

		private void res_Click(object sender, EventArgs e)
		{
			controllerToPlayer.ResolutionChanger();
		}
	}
}
