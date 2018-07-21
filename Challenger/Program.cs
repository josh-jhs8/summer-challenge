﻿using Challenger.Logic;
using Challenger.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Challenger
{
    class Program
    {
        private static string _configPath;

        static void Main(string[] args)
        {
            if (args.Length < 1) throw new Exception("Configuration not provided.");
            _configPath = args[0];

            var listener = ListenForConnections();
            Console.WriteLine("Press Enter to kill server");
            Console.ReadLine();
        }

        private static Task ListenForConnections()
        {
            return Task.Run(() =>
            {
                var host = Dns.GetHostEntry("localhost");
                var ipAddress = host.AddressList[host.AddressList.Length - 1];
                var localEndpoint = new IPEndPoint(ipAddress, 2092);

                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Bind(localEndpoint);

                    while (true)
                    {
                        socket.Listen(100);
                        var connection = socket.Accept();
                        var task = ManageConnection(connection);
                    }
                }
            });
        }

        private static Task ManageConnection(Socket connection)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("New Connection means new Challenge!");
                var config = ChallengeConfiguration.GetChallengeConfiguration(_configPath);
                var state = ChallengeState.GetStateFromConfiguration(config);
                var manager = new ChallengeManager(state);
                while (true)
                {
                    try
                    {
                        byte[] data = new byte[1000000];
                        var dataSize = connection.Receive(data);

                        if (!connection.Connected) break;
                        if (dataSize == 0) continue;

                        byte[] command = new byte[dataSize];
                        Array.Copy(data, command, dataSize);
                        var response = manager.ProcessCommand(command);
                        connection.Send(response);
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Connection died!");
                        break;
                    }
                    
                }
            });
        }
    }
}
