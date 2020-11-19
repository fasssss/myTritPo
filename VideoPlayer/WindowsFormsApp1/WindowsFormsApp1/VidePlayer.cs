﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxWMPLib;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;
using System.IO;
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
	class VidePlayer
	{
		private bool isFullScreen = false;
		Form1 player;
		private Size windowSize1;
		private Size windowPosition;
		private Size playerSize1;
		private Size playerPosition1;
		private Size button1Position;
		public VidePlayer(Form1 form)
		{
			player = form;
			windowSize1 = player.Size;
			playerSize1 = player.axWindowsMediaPlayer1.Size;
			playerPosition1 = (Size)player.axWindowsMediaPlayer1.Location;
			button1Position = (Size)player.button1.Location;
			windowPosition = (Size)player.Location;
		}

		public void ResolutionChanger()
		{
			if (isFullScreen == false)
			{
				Size resolution = Screen.PrimaryScreen.Bounds.Size;
				resolution.Height -= 60;
				player.WindowState = FormWindowState.Maximized;
				player.axWindowsMediaPlayer1.Location = new Point(0, 0);
				player.axWindowsMediaPlayer1.Size = resolution;
				player.button1.BackgroundImage = Image.FromFile("s1200.jpg");
				player.button1.Location = new Point(resolution.Width - 40, resolution.Height - 30);
				isFullScreen = true;
			}
			else
			{
				player.Location = (Point)windowPosition;
				player.WindowState = FormWindowState.Normal;
				player.axWindowsMediaPlayer1.Location = (Point)playerPosition1;
				player.axWindowsMediaPlayer1.Size = playerSize1;
				player.button1.BackgroundImage = Image.FromFile("fullscreen.png");
				player.button1.Location = new Point(button1Position.Width, button1Position.Height);
				isFullScreen = false;
			}
		}
	}
}
