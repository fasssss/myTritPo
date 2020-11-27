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
		private WebPart.ClientPart clientPart;
#pragma warning disable CS0618 // Тип или член устарел
		private WebPart.HostPart hostPart = new WebPart.HostPart(System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0]);
#pragma warning restore CS0618 // Тип или член устарел
		private TimerCallback tm;
		private System.Threading.Timer timer;
		private string sendedMessage;
		private string chatLog;
		private bool isOnline;
		private string fullIpPath;
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

			chatLog = string.Empty;
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
			fullIpPath = fullPath.Insert(fullPath.Length - 4, "Conf");
			fullPath = fullPath.Insert(fullPath.Length - 4, "S");
			tm = new TimerCallback(ParallelStream);
			timer = new System.Threading.Timer(tm, fullPath, 100, 100);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (controllerToPlayer.links.Contains("https"))
			{
				System.Diagnostics.Process.Start(controllerToPlayer.links);
			}
		}

		private void ParallelStream(object fullPath)
		{
			Invoke(new NewSubDelegate(controllerToPlayer.Subs), fullPath);
			if (isOnline)
			{
				try
				{
					clientPart.Writer(new StringBuilder(sendedMessage));
				}
				catch(ArgumentOutOfRangeException)
				{
					sendedMessage = string.Empty;
					MessageBox.Show("That Is Too Huge Message");
				}
				sendedMessage = string.Empty;
				hostPart.Listener();
				Invoke(new MessageGeterDelegate(MessageSetToBox));
			}
		}

		private delegate void NewSubDelegate(object path);
		private delegate void MessageGeterDelegate();
		private void MessageSetToBox()
		{
			richTextBox1.Text += clientPart.Listner();
		}

		private void SubMode_Click(object sender, EventArgs e)
		{
			controllerToPlayer.SubsTumbler();
		}

		private void loop_Click(object sender, EventArgs e)
		{
			controllerToPlayer.LoopTumbler();
		}

		private void SendMessage_Click(object sender, EventArgs e)
		{
			if (isOnline)
			{
				sendedMessage = richTextBox2.Text + "\n";
				richTextBox2.Text = string.Empty;
			}
			else
			{
				MessageBox.Show("Enter your nickname first!");
				richTextBox2.Text = string.Empty;

			}
		}

		private void AplyNickName_Click(object sender, EventArgs e)
		{
			try
			{
				clientPart = new WebPart.ClientPart(new StreamReader(fullIpPath));
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("No connection to host");
				return;
			}
			catch (InvalidOperationException)
			{
				MessageBox.Show("Server Offline");
				return;
			}

			if (textBox3.Text.Length <= 20)
			{
				clientPart.nickName = textBox3.Text;
				textBox3.Enabled = false;
				button5.Visible = false;
				button5.Enabled = false;
				isOnline = true;
			}
			else
			{
				MessageBox.Show("Too long Name(it should be less 21)");
				textBox3.Text = string.Empty;
			}
		}
	}
}
