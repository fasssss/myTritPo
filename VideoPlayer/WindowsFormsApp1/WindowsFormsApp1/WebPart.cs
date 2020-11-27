using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
	class WebPart
	{
		public class HostPart
		{
			protected IPAddress hostADR { get; private set; }
			protected TcpClient[] clOut = Array.Empty<TcpClient>();
			protected TcpClient[] clGet = Array.Empty<TcpClient>();
			protected NetworkStream[] stOut = Array.Empty<NetworkStream>();
			protected NetworkStream[] stGet = Array.Empty<NetworkStream>();
			TcpListener serverGet;
			TcpListener serverOut;
			protected NetworkStream stream { get; private set; }
			public HostPart(string ip)
			{
				hostADR = IPAddress.Parse(ip);
				serverGet = new TcpListener(hostADR, 8888);
				serverOut = new TcpListener(hostADR, 8882);
				serverGet.Start();
				serverOut.Start();
			}

			public HostPart(IPAddress hostIP)
			{
				hostADR = hostIP;
				serverGet = new TcpListener(hostADR, 8888);
				serverOut = new TcpListener(hostADR, 8882);
				serverGet.Start();
				serverOut.Start();
			}

			public void Listener()
			{
				if (serverGet.Pending())
				{
					Array.Resize(ref clOut, clOut.Length + 1);
					clOut[clOut.Length - 1] = serverGet.AcceptTcpClient();
					Array.Resize(ref clGet, clGet.Length + 1);
					clGet[clGet.Length - 1] = serverOut.AcceptTcpClient();
					Array.Resize(ref stOut, stOut.Length + 1);
					stOut[stOut.Length - 1] = clOut[clOut.Length - 1].GetStream();
					Array.Resize(ref stGet, stGet.Length + 1);
					stGet[stGet.Length - 1] = clGet[clGet.Length - 1].GetStream();
				}

				for (int i = 0; i < stOut.Length; i++)
				{
					if (stOut[i].DataAvailable)
					{
						byte[] data = new byte[256];
						StringBuilder response = new StringBuilder();
						int bytes = stOut[i].Read(data, 0, data.Length);
						response.Append(Encoding.Unicode.GetString(data, 0, bytes));
						for (int j = 0; j < stGet.Length; j++)
						{
							stGet[j].Write(Encoding.Unicode.GetBytes(response.ToString()), 0, response.Length * 2);
						}
					}
				}
			}
		}

		public class ClientPart
		{
			protected IPAddress hostADR;
			protected TcpClient clientGet;
			protected TcpClient clientOut;
			protected NetworkStream streamGet;
			protected NetworkStream streamOut;
			public string nickName;
			public ClientPart(string ip)
			{

				hostADR = IPAddress.Parse(ip);
				clientGet = new TcpClient();
				clientOut = new TcpClient();
				clientGet.Connect(hostADR, 8882);
				clientOut.Connect(hostADR, 8888);
				streamOut = clientGet.GetStream();
				streamGet = clientOut.GetStream();
			}

			public ClientPart(StreamReader file)
			{

				hostADR = IPAddress.Parse(file.ReadLine());
				clientGet = new TcpClient();
				clientOut = new TcpClient();
				clientGet.BeginConnect(hostADR, 8882, null, null);
				clientOut.BeginConnect(hostADR, 8888, null, null);
				Thread.Sleep(1000);
				streamOut = clientGet.GetStream();
				streamGet = clientOut.GetStream();
			}

			public void Writer(StringBuilder response)
			{
				if (response.Length != 0)
				{
					response = new StringBuilder(nickName + ": " + response.ToString());
				}
				if (response.Length > 256)
				{
					throw new ArgumentOutOfRangeException(nameof(response));
				}

				byte[] data = new byte[256];
				data = Encoding.Unicode.GetBytes(response.ToString());
				streamGet.Write(data, 0, data.Length);
			}

			public string Listner()
			{
				StringBuilder response = new StringBuilder();
				if (streamOut.DataAvailable)
				{
					byte[] data = new byte[256];
					int bytes = streamOut.Read(data, 0, data.Length);
					response.Append(Encoding.Unicode.GetString(data, 0, bytes));
				}
				return response.ToString();
			}
		}
	}
}
