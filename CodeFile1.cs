/*

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TwitchLib;

using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using System.Timers;

/* Whisper Ruler
"----|----|----|----|----|----|----|----|----|----|----|-" +
"----|----|----|----|----|----|----|----|----|----|----|----|----|----|---");


namespace Wittel_Bot
{
    internal class BotController
    {
        readonly ConnectionCredentials botCred = new ConnectionCredentials(Credentials.BotName, Credentials.BotToken);
        TwitchClient client;
        List<User> userList;
        private static System.Timers.Timer mTimer;
        private static System.Timers.Timer uTimer;

        string filePath = Credentials.FilePath;
        int commandSwitch = -1;
        string motd = "BLANK";
        int upTime = 0;
        int uInterval = 1;
        int saveInterval = 15;
        int enlistInterval = 15;
        int mapInterval = 31;
        int socialInterval = 21;

        public class User
        {
            public string userName;
            public string firstName;
            public string nickName;
            public string lastName;
            public int rank;
            public int curEXP;
            public int maxEXP;
            public bool enlistable;
            public bool enlisted_XCOM_LW;
            public bool enlisted_XCOM_2;

        }

        //userList.Exists(x => x.userName.Contains("?"));

        internal void Connect()
        {
            Console.WriteLine("Connecting...");

            // Client Connect
            client = new TwitchClient();
            client.Initialize(botCred, Credentials.ChannelName);

            while (commandSwitch < 0 || commandSwitch > 3)
            {
                commandSwitch = Convert.ToInt16(gameQuestion());
            }

            userTake();

            client.OnConnected += On_Connected;
            client.OnJoinedChannel += On_JoinedChennel;
            client.OnMessageReceived += On_MessageReceived;
            client.OnWhisperReceived += On_WhisperReceived;

            client.Connect();

            upTimer();

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
                userReturn();
                Console.WriteLine("\n---Saved Users!---\n");
            }
            if (upTime % enlistInterval == 0)
            {
                client.SendMessage(Credentials.ChannelName, "Want to enlist in the Wittel Barracks? Type, '!enlist' in chat to begin the process!");
            }
            if (upTime % mapInterval == 0)
            {
                client.SendMessage(Credentials.ChannelName, "Where in the world are you? No, really. Where? https://www.zeemaps.com/map?group=1463814&add=1");
            }
            if (upTime % socialInterval == 0)
            {
                client.SendMessage(Credentials.ChannelName, "Be sure to follow up on the socials! Twitter & Discord linked below!");
            }
        }

        internal void Disconnect()
        {
            userReturn();
        }

        private void On_Connected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void On_JoinedChennel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Success!\nPlease enter 'x' when ready to stop the program.");
        }

        // All messages in genearl chat
        private void On_MessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.StartsWith("!"))
            {
                if (e.ChatMessage.Message.Equals("!ping"))
                {
                    client.SendMessage(Credentials.ChannelName, "Pong!");
                }
                else if (e.ChatMessage.Message.Equals("!uptime"))
                {
                    client.SendMessage(Credentials.ChannelName, "Wittel has been streaming for around " + (upTime / 60) + "H & " + (upTime % 60) + "M.");
                }
                else if (e.ChatMessage.Message.Equals("!commands"))
                {
                    client.SendMessage(Credentials.ChannelName, "Here are the commands currently available to you: !uptime - !enlist  ");
                }



                else if (e.ChatMessage.Message.Equals("!enlist"))
                {
                    switch (enlistUser(e.ChatMessage.Username))
                    {
                        case 0:
                            client.SendMessage(Credentials.ChannelName, e.ChatMessage.Username + " has successfully enlisted! Wittel_Bot will whisper you to continue this process.");
                            Thread.Sleep(500);
                            client.SendWhisper(e.ChatMessage.Username, whisperMenu(e.ChatMessage.Username));
                            break;
                        case 1:
                            client.SendMessage(Credentials.ChannelName, e.ChatMessage.Username + " is already enlisted! Check your whispers.");
                            Thread.Sleep(500);
                            client.SendWhisper(e.ChatMessage.Username, whisperMenu(e.ChatMessage.Username));
                            break;
                        default:
                            client.SendMessage(Credentials.ChannelName, "Commander Wittel! Enlistment issues!");
                            break;
                    }
                }
                else if (e.ChatMessage.Message.StartsWith("!promote") && e.ChatMessage.Username.Equals(Credentials.ChannelName))
                {
                    if (e.ChatMessage.Message.Equals("!promote"))
                    {
                        promoteSoldier("RANDOM");
                    }
                    else
                    {
                        promoteSoldier(e.ChatMessage.Message.Substring(9));
                    }
                }
            }


        }

        // INCOMPLETE

        private void promoteSoldier(string name)
        { /*
            name = name.ToLower();
            int flag1 = 1;
            User tempUser = new User();

            if (name.Equals("random"))
            {
                var r = new Random();
                while (flag1 == 1)
                {
                    int ranNum = r.Next(userList.Count);
                    tempUser = userList[ranNum];

                    





                    if (commandSwitch == 1)
                    {
                        if (tempUser.enlisted_XCOM_LW != true && tempUser.enlistable == true)
                        {
                            tempUser.enlisted_XCOM_LW = true;
                            flag1 = 0;
                        }
                    }
                    else if (commandSwitch == 2)
                    {
                        if (tempUser.enlisted_XCOM_2 != true && tempUser.enlistable == true)
                        {
                            tempUser.enlisted_XCOM_2 = true;
                            flag1 = 0;
                        }
                    }
                    else
                    {
                        flag1 = 0;
                    }
                }
            }
            else
            {
                if(userList.Exists(x => x.userName == name))
                {
                    tempUser = userList.Find(x => x.userName == name);
                }
                else
                {
                    client.SendMessage(Credentials.ChannelName, "405: Soldier not found!");
                    return;
                }

            }

            client.SendMessage(Credentials.ChannelName, tempUser.userName + " ! Your soldier, " + tempUser.firstName + " '" + tempUser.nickName + "' " + tempUser.lastName + ", has been promoted! o7");
        
        }

        // User whipsering Wittel_Bot
        private void On_WhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Message.Equals("!commands"))
            {
                client.SendWhisper(e.WhisperMessage.Username, "!commands - !enlist_switch - !enlist_first NAME - !enlist_nick NAME - !enlist_last NAME - !rank - !EXP - !form");
            }
            else if (e.WhisperMessage.Message.StartsWith("!enlist"))
            {
                if (e.WhisperMessage.Message.Equals("!enlist_switch"))
                {
                    User thisUser = new User();
                    enlistUser(e.WhisperMessage.Username);

                    thisUser = userList.Find(x => x.userName == e.WhisperMessage.Username);
                    if (thisUser.enlistable == true)
                    {
                        thisUser.enlistable = false;
                        client.SendWhisper(e.WhisperMessage.Username, e.WhisperMessage.Username + " is no longer enlistable!");
                    }
                    else
                    {
                        thisUser.enlistable = true;
                        client.SendWhisper(e.WhisperMessage.Username, e.WhisperMessage.Username + " can now be enlisted!");
                    }
                }
                else
                {

                }
            }
            /*
            else if (e.WhisperMessage.Message.StartsWith("!enlist_first"))
            {
                User thisUser = new User();
                enlistUser(e.WhisperMessage.Username);
                thisUser = userList.Find(x => x.userName == e.WhisperMessage.Username);

                try
                {
                    thisUser.firstName = e.WhisperMessage.Message.Substring(14);
                    client.SendWhisper(e.WhisperMessage.Username, "Thank you " + e.WhisperMessage.Username + "! The first name on your enlistment form is now " + thisUser.firstName + "!");
                }
                catch
                {
                    client.SendWhisper(e.WhisperMessage.Username, "Error! Please use the format, '!enlist_first FIRST' without quotes where FIRST is replaced with the first name of your choice.");
                }
            }
            else if (e.WhisperMessage.Message.StartsWith("!enlist_nick"))
            {
                User thisUser = new User();
                enlistUser(e.WhisperMessage.Username);
                thisUser = userList.Find(x => x.userName == e.WhisperMessage.Username);

                try
                {
                    thisUser.nickName = e.WhisperMessage.Message.Substring(13);
                    client.SendWhisper(e.WhisperMessage.Username, "Thank you " + e.WhisperMessage.Username + "! The nick name on your enlistment form is now " + thisUser.nickName + "!");
                }
                catch
                {
                    client.SendWhisper(e.WhisperMessage.Username, "Error! Please use the format, '!enlist_first NICK' without quotes where NICK is replaced with the nickname of your choice.");
                }
            }
            else if (e.WhisperMessage.Message.StartsWith("!enlist_last"))
            {
                User thisUser = new User();
                enlistUser(e.WhisperMessage.Username);
                thisUser = userList.Find(x => x.userName == e.WhisperMessage.Username);

                try
                {
                    thisUser.lastName = e.WhisperMessage.Message.Substring(13);
                    client.SendWhisper(e.WhisperMessage.Username, "Thank you " + e.WhisperMessage.Username + "! The last name on your enlistment form is now " + thisUser.lastName + "!");
                }
                catch
                {
                    client.SendWhisper(e.WhisperMessage.Username, "Error! Please use the format, '!enlist_first LAST' without quotes where LAST is replaced with the last name of your choice.");
                }
            }
            else if (e.WhisperMessage.Message.Equals("!rank"))
            {
                enlistUser(e.WhisperMessage.Username);

                client.SendWhisper(e.WhisperMessage.Username, "Feature not yet implemented. Sorry!");
            }
            else if (e.WhisperMessage.Message.Equals("!EXP"))
            {
                enlistUser(e.WhisperMessage.Username);

                client.SendWhisper(e.WhisperMessage.Username, "Feature not yet implemented. Sorry!");
            }
            else if (e.WhisperMessage.Message.Equals("!form"))
            {
                User thisUser = new User();
                enlistUser(e.WhisperMessage.Username);

                thisUser = userList.Find(x => x.userName == e.WhisperMessage.Username);
                client.SendWhisper(e.WhisperMessage.Username, "Username: " +
                                                              thisUser.userName +
                                                              " / First: " +
                                                              thisUser.firstName +
                                                              " / Nick: " +
                                                              thisUser.nickName +
                                                              " / Last: " +
                                                              thisUser.lastName +
                                                              " / Enlistable: " +
                                                              thisUser.enlistable.ToString() +
                                                              " / LW Enlisted: " +
                                                              thisUser.enlisted_XCOM_LW.ToString() +
                                                              " / X2 Enlisted: " +
                                                              thisUser.enlisted_XCOM_2.ToString()
                );
            }
            else
            {
                client.SendWhisper(e.WhisperMessage.Username, "Please use a valid command! ('!commands' for your list of available commands)");
            }
        }

        // Returns string with embedded username
        private string whisperMenu(string username)
        {
            return "Hello " + username + "! Thank you for enlisting with Wittel's channel! There are many perks associated with enlistment. " +
                   "Please whisper '!commands' to find out what whisper commands are available to you.";
        }

        // Fills the userList at the start of program
        public void userTake()
        {
            userList = new List<User>();
            string temp;

            StreamReader file = new StreamReader(filePath + "userList.txt");

            while ((temp = file.ReadLine()) != null)
            {
                var line = temp.Split('|');

                User tempUser = new User();

                tempUser.userName = line[0];
                tempUser.firstName = line[1];
                tempUser.nickName = line[2];
                tempUser.lastName = line[3];
                tempUser.rank = Convert.ToInt16(line[4]);
                tempUser.curEXP = Convert.ToInt16(line[5]);
                tempUser.maxEXP = Convert.ToInt16(line[6]);
                tempUser.enlistable = Convert.ToBoolean(line[7]);
                tempUser.enlisted_XCOM_LW = Convert.ToBoolean(line[8]);
                tempUser.enlisted_XCOM_2 = Convert.ToBoolean(line[9]);

                userList.Add(tempUser);
            }

            file.Close();
        }

        // Takes the userList and places it back into the file userList.txt
        public void userReturn()
        {
            StreamWriter sw = new StreamWriter(filePath + "userList.txt");

            string temp;

            for (int i = 0; i < userList.Count; i++)
            {
                temp = userList[i].userName + "|" + userList[i].firstName + "|" + userList[i].nickName + "|" + userList[i].lastName + "|" +
                       userList[i].rank + "|" + userList[i].curEXP + "|" + userList[i].maxEXP + "|" + userList[i].enlistable + "|" +
                       userList[i].enlisted_XCOM_LW + "|" + userList[i].enlisted_XCOM_2;

                sw.WriteLine(temp);
            }

            sw.Close();
        }

        // Adds user on "!enlist" command to the userList
        public int enlistUser(string user)
        {
            if (userList.Exists(x => x.userName == user))
            {
                return 1;
            }
            else
            {
                User tempUser = new User();

                tempUser.userName = user;
                tempUser.firstName = "---";
                tempUser.nickName = "---";
                tempUser.lastName = "---";
                tempUser.rank = 0;
                tempUser.curEXP = 0;
                tempUser.maxEXP = 0;
                tempUser.enlistable = true;
                tempUser.enlisted_XCOM_LW = false;
                tempUser.enlisted_XCOM_2 = false;

                userList.Add(tempUser);

                return 0;
            }
        }

        // Asks terminal what game is being played
        private string gameQuestion()
        {
            Console.WriteLine("What commands are you going to need to use tonight?\n" +
                              "General - 0\n" +
                              "XCOM LW - 1\n" +
                              "XCOM 2 -- 2\n" +
                              "DnD ----- 3\n" +
                              "Please enter your choice: ");
            return Console.ReadLine();
        }

    }
}


/*
if(e.ChatMessage.Message.Equals("!motd"))
{
    client.SendMessage(Credentials.ChannelName, motd);
}
else if(e.ChatMessage.Username.Equals(Credentials.ChannelName) && e.ChatMessage.Message.Contains("!motd"))
{
    motd = e.ChatMessage.Message.Substring(6);
    client.SendMessage(Credentials.ChannelName, "MOTD Changed!");
}
else if(e.ChatMessage.Message.Contains(".com") || e.ChatMessage.Message.Contains("www") || e.ChatMessage.Message.Contains(".net") || e.ChatMessage.Message.Contains(".org"))
{
    if (!pfcCheck(e.ChatMessage.Username, pfcList))
    {
        client.SendMessage(Credentials.ChannelName, $".timeout {e.ChatMessage.Username} {1}");
        client.SendMessage(Credentials.ChannelName, e.ChatMessage.Username + ", please use the command '!pfc' before attempting to post links!");
    }
}
else if(e.ChatMessage.Message.StartsWith("!") || e.ChatMessage.Message.StartsWith("?"))
{
    userCommands uCommand = new userCommands(e.ChatMessage.Message, e.ChatMessage.Username, commandSwitch);
    client.SendMessage(Credentials.ChannelName, uCommand.basicCommand());
}
*/
