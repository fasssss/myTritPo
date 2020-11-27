using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxWMPLib;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using WindowsFormsApp1;
using WMPLib;

namespace WindowsFormsApp1
{
	class VidePlayer
	{
		private bool isFullScreen = false;
		private bool isSubsOn = false;
		private bool isLoopOn = false;
		private Form1 player;
		private Size windowSize1;
		private Size windowPosition;
		private Size playerSize1;
		private Size playerPosition1;
		private Size button1Position;
		private Size button2Position;
		private Size button3Position;
		private Size subSize;
		private Size subPosition;
		public string links;
		public VidePlayer(Form1 form)
		{
			player = form;
			windowSize1 = player.Size;
			playerSize1 = player.axWindowsMediaPlayer1.Size;
			playerPosition1 = (Size)player.axWindowsMediaPlayer1.Location;
			button1Position = (Size)player.button1.Location;
			button2Position = (Size)player.button2.Location;
			button3Position = (Size)player.button3.Location;
			subSize = player.textBox2.Size;
			subPosition = (Size)player.textBox2.Location;
			windowPosition = (Size)player.Location;
		}

		public void ResolutionChanger()
		{
			if (isFullScreen == false)
			{
				Size resolution = Screen.PrimaryScreen.Bounds.Size;
				resolution.Height -= 60;
				player.WindowState = FormWindowState.Maximized;
				player.textBox2.Size = new Size(resolution.Width, 40);
				player.textBox2.Location = new Point(0, 0);
				player.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.25F, System.Drawing.FontStyle.Bold); ;
				player.axWindowsMediaPlayer1.Location = new Point(0, 0);
				player.axWindowsMediaPlayer1.Size = resolution;
				player.button1.BackgroundImage = Image.FromFile("s1200.jpg");
				player.button1.Location = new Point(resolution.Width - 40, resolution.Height - 30);
				player.button2.Location = new Point(resolution.Width - 80, resolution.Height - 30);
				player.button3.Location = new Point(resolution.Width - 120, resolution.Height - 30);
				isFullScreen = true;
			}
			else
			{
				player.Location = (Point)windowPosition;
				player.WindowState = FormWindowState.Normal;
				player.textBox2.Size = subSize;
				player.textBox2.Location = (Point)subPosition;
				player.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold); ;
				player.axWindowsMediaPlayer1.Location = (Point)playerPosition1;
				player.axWindowsMediaPlayer1.Size = playerSize1;
				player.button1.BackgroundImage = Image.FromFile("fullscreen.png");
				player.button1.Location = new Point(button1Position.Width, button1Position.Height);
				player.button2.Location = new Point(button2Position.Width, button2Position.Height);
				player.button3.Location = new Point(button3Position.Width, button3Position.Height);
				isFullScreen = false;
			}
		}

		public string VideoSelector()
		{
			return (string)player.comboBox1.SelectedItem;
		}

		public void Description(string path)
		{
			try
			{
				StreamReader file = new StreamReader(path);
				player.label1.Visible = true;
				player.label2.Visible = true;
				player.label3.Visible = true;
				player.linkLabel1.Visible = true;
				player.textBox1.Visible = true;
				player.label1.Text = file.ReadLine();
				player.label2.Text = "CHANNEL:";
				player.linkLabel1.Text = file.ReadLine();
				player.label3.Text = file.ReadLine();
				links = file.ReadLine();
				player.textBox1.Text = file.ReadToEnd();
			}
			catch (FileNotFoundException)
			{
				player.label1.Visible = false;
				player.label2.Visible = false;
				player.label3.Visible = false;
				player.linkLabel1.Visible = false;
				player.textBox1.Visible = false;
			}
		}
		public void Subs(object path)
		{
			try
			{
				StreamReader file = new StreamReader((string)path);
				double currentTime;
				double[] readedTime = new double[1000];
				string[] readedSubs = new string[1000];
				int i = 0;
				for (; !file.EndOfStream; i++)
				{
					readedTime[i] = double.Parse(file.ReadLine());
					readedSubs[i] = file.ReadLine();
				}
				readedTime[i] = double.MaxValue;
				currentTime = player.axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
				int j = 0;
				for (; !(readedTime[j] <= currentTime && readedTime[j + 1] >= currentTime); j++) ;
				player.textBox2.Text = readedSubs[j];
			}
			catch(FileNotFoundException)
			{
				player.textBox2.Text =	"No Subtitles file :)";
			}
		}

		public void SubsTumbler()
		{
			if (isSubsOn == false)
			{
				player.button3.BackgroundImage = Image.FromFile("SubOn.png");
				player.textBox2.Visible = true;
				isSubsOn = true;
			}
			else
			{
				player.button3.BackgroundImage = Image.FromFile("SubOffLowRes.png");
				player.textBox2.Visible = false;
				isSubsOn = false;
			}
		}

		public void LoopTumbler()
		{
			if (isLoopOn == false)
			{
				player.button2.BackgroundImage = Image.FromFile("repeaton.png");
				player.axWindowsMediaPlayer1.settings.setMode("loop", true);
				isLoopOn = true;
			}
			else
			{
				player.button2.BackgroundImage = Image.FromFile("repeatoff.png");
				player.axWindowsMediaPlayer1.settings.setMode("loop", false);
				isLoopOn = false;
			}
		}
	}
}
