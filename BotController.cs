using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TwitchLib;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Api;
using TwitchLib.Communication.Services;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Client.Models;
using System.Timers;
using TwitchLib.Communication.Events;

namespace Wittel_Bot
{
    internal class BotController
    {
        // Important Variables
        readonly ConnectionCredentials botCred = new ConnectionCredentials(Credentials.BotName, Credentials.BotToken);
        TwitchClient client;
        Throttlers breakPedal;
        IClient linkage;

        string filePath = Credentials.FilePath;

        // Timer Variables (Interval in Minutes)
        private static System.Timers.Timer uTimer;
        int upTime = 0;
        int socialMessage = 0;
        int subMessage = 0;
        int uInterval = 1;
        int saveInterval = 10;
        int enlistInterval = 20;
        int mapInterval = 30;
        int socialInterval = 15;
        int xpInterval = 6;

        // Lists
        List<User> masterList = new List<User>();
        List<string> activeList = new List<string>();
        List<Soldier> barracks = new List<Soldier>();
        List<NPC> npc = new List<NPC>();

        // Other Variables
        int commandSwitch = -1;
        string motd = "BLANK";

        // Function Call
        WB_Functions WBF = new WB_Functions();

        internal void Connect()
        {
            Console.WriteLine("Connecting...");

            // Client Connect
            client = new TwitchClient(linkage);
            client.Initialize(botCred, Credentials.ChannelName);
            breakPedal = new Throttlers(linkage, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
            
            while (commandSwitch < 0 || commandSwitch > 3)
            {
                commandSwitch = Convert.ToInt16(WBF.gameQuestion());
            }

            masterList = WBF.UserTake(masterList);

            if (commandSwitch == 1 || commandSwitch == 2)
            {
                barracks = WBF.SoldierTake(barracks);
            }
            else if(commandSwitch == 3)
            {
                npc = WBF.NPC_Take(npc);
            }
            else
            {
                ;
            }

            client.OnConnected += On_Connected;
            client.OnJoinedChannel += On_JoinedChennel;

            client.OnUserJoined += On_UserJoined;
            client.OnUserLeft += On_UserLeft;
            
            client.AddChatCommandIdentifier('!');
            client.OnChatCommandReceived += On_CommandReceived;
            client.AddWhisperCommandIdentifier('!');
            client.OnWhisperCommandReceived += On_WhisperCommandReceived;

            client.OnBeingHosted += On_BeingHosted;
            client.OnRaidNotification += On_BeingRaided;

            client.OnGiftedSubscription += On_GiftedSub;
            client.OnNewSubscriber += On_NewSub;
            client.OnReSubscriber += On_ReSub;

            client.OnMessageThrottled += On_MessageThrottle;
            client.OnWhisperThrottled += On_WhisperThrottle;

            client.Connect();

            upTimer();
        }














        //---Message Received---//
        private void On_CommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (e.Command.ArgumentsAsList.Count == 0 && e.Command.CommandText.Equals(""))
            {
                client.SendWhisper(e.Command.ChatMessage.Username, "You rang?");
                Console.WriteLine($"! : Sent Whisper to {e.Command.ChatMessage.Username}.");
            }
            else if (e.Command.CommandText.Equals("uptime"))
            {
                client.SendMessage(Credentials.ChannelName, "Wittel has been streaming for " + upTime/60 + " Hours and " + upTime%60 + " Minutes!");
                Console.WriteLine($"! : Sent Message for {e.Command.ChatMessage.Username}.");
            }
            else
            {
                Messages temp = new Messages();
                temp = WBF.ValidMessage(e, ref masterList, ref activeList, ref barracks, ref npc, commandSwitch);
                if (temp.mExists)
                {
                    client.SendMessage(Credentials.ChannelName, temp.message);
                    Console.WriteLine($"! : Sent Message for {e.Command.ChatMessage.Username}.");
                }
                Thread.Sleep(500);
                if (temp.wExists)
                {
                    client.SendWhisper(e.Command.ChatMessage.Username, temp.whisper);
                    Console.WriteLine($"! : Sent Whisper to {e.Command.ChatMessage.Username}.");
                }
            }
        }
        //---Whisper Received---//
        private void On_WhisperCommandReceived(object sender, OnWhisperCommandReceivedArgs e)
        {
            Messages temp = WBF.ValidWhisper(e, ref masterList, ref activeList, ref barracks, ref npc, commandSwitch);
            if (temp.wExists)
            {
                client.SendWhisper(e.Command.WhisperMessage.Username, temp.whisper);
                Console.WriteLine($"! : Sent Whisper to {e.Command.WhisperMessage.Username}.");
            }
            Thread.Sleep(500);
            if (temp.mExists)
            {
                client.SendMessage(Credentials.ChannelName, temp.message);
                Console.WriteLine($"! : Sent Message to Chat about {e.Command.WhisperMessage.Username}.");
            }
        }

        //------Events and Timers------//
        private void On_Connected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"\n* : Connecting to {e.AutoJoinChannel}...");
        }

        private void On_JoinedChennel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("? : Success! Please enter 'x' when ready to stop the program.");
        }

        private void On_UserJoined(object sender, OnUserJoinedArgs e)
        {
            masterList = WBF.EnlistUser(e.Username, masterList);
            activeList.Add(e.Username);
            Console.WriteLine($"+ : {e.Username} has joined the channel!");
        }

        private void On_UserLeft(object sender, OnUserLeftArgs e)
        {
            activeList.Remove(e.Username);
            Console.WriteLine($"- : {e.Username} has left the channel.");
        }

        private void On_ReSub(object sender, OnReSubscriberArgs e)
        {
            Console.WriteLine($"? : {e.ReSubscriber} has ReSubscribed!");
        }

        private void On_NewSub(object sender, OnNewSubscriberArgs e)
        {
            Console.WriteLine($"? : {e.Subscriber} has Subscribed!");
        }

        private void On_GiftedSub(object sender, OnGiftedSubscriptionArgs e)
        {
            Console.WriteLine($"? : {e.GiftedSubscription} has gifted a Subscription!");
        }

        private void On_BeingHosted(object sender, OnBeingHostedArgs e)
        {
            Console.WriteLine($"? : {e.BeingHostedNotification.HostedByChannel} has hosted the channel with {e.BeingHostedNotification.Viewers}!");
        }

        private void On_BeingRaided(object sender, OnRaidNotificationArgs e)
        {
            Console.WriteLine($"? : {e.RaidNotificaiton.DisplayName} has raided the channel! {e.RaidNotificaiton.Id} | {e.RaidNotificaiton.UserId}");
        }

        private void On_WhisperThrottle(object sender, OnWhisperThrottledEventArgs e)
        {
            Console.WriteLine("? : Whisper has been Throttled!");
        }

        private void On_MessageThrottle(object sender, OnMessageThrottledEventArgs e)
        {
            Console.WriteLine("? : Message has been Throttled!");
        }

        private void upTimer()
        {
            uTimer = new System.Timers.Timer(uInterval * 60000);
            uTimer.Elapsed += uEvent;
            uTimer.AutoReset = true;
            uTimer.Enabled = true;
        }
        
        private void uEvent(object sender, ElapsedEventArgs e)
        {
            upTime++;
            if (upTime % saveInterval == 0)
            {
                if (commandSwitch == 1 || commandSwitch == 2)
                {
                    WBF.SoldierReturn(barracks);
                }
                else if (commandSwitch == 3)
                {
                    WBF.NPC_Return(npc);
                }
                WBF.UserReturn(masterList);
                Console.WriteLine("/ : Timed Save of Users!");
            }
            if (upTime % enlistInterval == 0)
            {
                client.SendMessage(Credentials.ChannelName, "Want to enlist in the Wittel Barracks? Type, '!enlist' in chat to begin the process!");
                Console.WriteLine("/ : Timed Enlist Message!");
            }
            if (upTime % mapInterval == 0)
            {
                switch (subMessage%2)
                {
                    case 0:
                        client.SendMessage(Credentials.ChannelName, "Where in the world are you? No, really. Where? https://www.zeemaps.com/map?group=1463814&add=1");
                        Console.WriteLine("/ : Timed Map Message!");
                        break;
                    case 1:
                        client.SendMessage(Credentials.ChannelName, "Do you love dogs and RNG? Subscribe today, free with Prime, and praise both! wittelPraise wittelBaron wittelPraise");
                        Console.WriteLine("/ : Timed Sub Message!");
                        break;
                }
            }
            if (upTime % socialInterval == 0)
            {
                if(socialMessage%4 == 0)
                {
                    client.SendMessage(Credentials.ChannelName, "Be sure to follow up on the socials! Twitter & Discord linked below!");
                    Console.WriteLine("/ : Timed Social Message!");
                }
                else if(socialMessage%4 == 1)
                {
                    client.SendMessage(Credentials.ChannelName, "Want to join Wittel's Barrack? Do so here! https://discord.gg/hzZCTqK");
                    Console.WriteLine("/ : Timed Discord Message!");
                }
                else if(socialMessage%4 == 2)
                {
                    client.SendMessage(Credentials.ChannelName, "Enjoying the stream? Follow for when it goes live!");
                    Console.WriteLine("/ : Timed Follow/Subscribe Message!");
                }
                else
                {
                    client.SendMessage(Credentials.ChannelName, "Interested Tweeting at me? Do so here! https://twitter.com/Wittel3");
                    Console.WriteLine("/ : Timed Twitter Message!");
                }
                socialMessage++;
            }
            if (upTime % xpInterval == 0)
            {
                masterList = WBF.XP_Distribute(activeList, masterList);
                Console.WriteLine("/ : XP Distributed!");
            }
        }

        internal void Disconnect()
        {
            Console.WriteLine("? : Returning Users to Database.");
            if (commandSwitch == 1 || commandSwitch == 2)
            {
                WBF.SoldierReturn(barracks);
            }
            else if (commandSwitch == 3)
            {
                WBF.NPC_Return(npc);
            }
            WBF.UserReturn(masterList);
            Console.WriteLine("* : Disconnecting.");
            Thread.Sleep(1000);
        }

       

        //userList.Exists(x => x.userName.Contains("?"));







    }
}
 