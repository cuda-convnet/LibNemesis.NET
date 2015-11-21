﻿using Piksel.Nemesis.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Nemesis = Piksel.Nemesis;

namespace NemesisTest
{
    partial class Program
    {
        private static void threadProcNodeA(object testData_)
        {
            var testData = (TestData)testData_;
            var _log = NLog.LogManager.GetLogger("NodeAThread");

            _log.Info("Creating node A...");

            var keyStore = new MemoryKeyStore();
            keyStore.Load(testData.NodeAKeys);


            var nodeA = new Nemesis.NemesisNode(testData.NodeAId, testData.Ports, testData.Host.ToString(), false);
            nodeA.EnableEncryption(keyStore);
            nodeA.SetLogName("nNodeA");


            nodeA.CommandRecieved += NodeA_CommandRecieved;
            
            var hubKeyStore = new MemoryKeyStore();
            hubKeyStore.Load(testData.HubKeys);
            nodeA.HubPublicKey = hubKeyStore.PublicKey;
            
            nodeA.Connect();

   
            var cmdTest = "n2h";
            _log.Info($"Sending command \"{cmdTest}\" (1/2) from Node A to Hub");

            var response = nodeA.SendCommand(cmdTest);
            _log.Info("Got response: {0}", response.Result);
            


            _log.Info($"Sending command \"{cmdTest}\" (2/2) from Node A to Hub");
            response = nodeA.SendCommand(cmdTest);
            _log.Info("Got response: {0}", response.Result);



        }

        private static void NodeA_CommandRecieved(object sender, Piksel.Nemesis.CommandRecievedEventArgs e)
        {
            e.ResultSource.SetResult("Node A Result");
        }
    }
}
