using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		private VidePlayer controllerToPlayer;
		private TimerCallback tm;
		private System.Threading.Timer timer;
		public Form1()
		{
			InitializeComponent();
			controllerToPlayer = new VidePlayer(this);
			axWindowsMediaPlayer1.BringToFront();
			button1.BringToFront();
			textBox2.BringToFront();
			button2.BringToFront();
			button3.BringToFront();
			string[] fullName = Directory.GetFiles(Environment.CurrentDirectory + @"\MyVideos", "*.mp4");
			int divorce = Directory.GetFiles(Environment.CurrentDirectory + @"\MyVideos", "*.avi").Length;
			Array.Resize(ref fullName, divorce + fullName.Length);
			Array.Copy(Directory.GetFiles(Environment.CurrentDirectory + @"\MyVideos", "*.avi"), 0, fullName, fullName.Length - divorce, divorce);
			divorce = Directory.GetFiles(Environment.CurrentDirectory + @"\MyVideos", "*.mov").Length;
			Array.Resize(ref fullName, divorce + fullName.Length);
			Array.Copy(Directory.GetFiles(Environment.CurrentDirectory + @"\MyVideos", "*.mov"), 0, fullName, fullName.Length - divorce, divorce);
			for (int i = 0; i < fullName.Length; i++)
			{
				int lastIndex = fullName[i].LastIndexOf(@"\");
				fullName[i] = fullName[i].Substring(lastIndex + 1);
			}

			comboBox1.Items.AddRange(fullName);
			label1.Visible = false;
			label2.Visible = false;
			label3.Visible = false;
			linkLabel1.Visible = false;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void res_Click(object sender, EventArgs e)
		{
			controllerToPlayer.ResolutionChanger();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (timer != null)
			{
				timer.Dispose();
			}
			string fullPath = Environment.CurrentDirectory + @"\MyVideos\" + controllerToPlayer.VideoSelector();
			axWindowsMediaPlayer1.URL = fullPath;
			fullPath = fullPath.Insert(fullPath.LastIndexOf(".") + 1, "txt");
			fullPath = fullPath.Remove(fullPath.Length - 3);
			controllerToPlayer.Description(fullPath);
			fullPath = fullPath.Insert(fullPath.Length - 4, "S");
			tm = new TimerCallback(Refresher);
			timer = new System.Threading.Timer(tm, fullPath, 100, 1000);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (controllerToPlayer.links.Contains("https"))
			{
				System.Diagnostics.Process.Start(controllerToPlayer.links);
			}
		}

		private void Refresher(object fullPath)
		{
			Invoke(new NewMessage(controllerToPlayer.Subs), fullPath);
		}

		private delegate void NewMessage(object path);

		private void SubMode_Click(object sender, EventArgs e)
		{
			controllerToPlayer.SubsTumbler();
		}

		private void loop_Click(object sender, EventArgs e)
		{
			controllerToPlayer.LoopTumbler();
		}
	}
}
