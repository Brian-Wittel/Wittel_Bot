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
using Wittel_Bot;

public class WB_Functions
{
    string filePath = Credentials.FilePath;
    string motd = "Wittel_Bot lives! Better than ever! Chat '!commands' for a comprehensive list of commands both current and future!";

    public string gameQuestion()
    {
        Console.WriteLine("What commands are you going to need to use tonight?\n" +
                          "General - 0\n" +
                          "XCOM LW - 1\n" +
                          "XCOM 2 -- 2\n" +
                          "DnD ----- 3\n" +
                          "Please enter your choice: ");
        return Console.ReadLine();
    }

    public List<User> UserTake(List<User> userList)
    {

        string temp;

        StreamReader file = new StreamReader(filePath + "masterList.txt");

        while ((temp = file.ReadLine()) != null)
        {
            var line = temp.Split('|');

            User tempUser = new User();

            tempUser.userName = line[0];
            tempUser.firstName = line[1];
            tempUser.nickName = line[2];
            tempUser.lastName = line[3];
            tempUser.xP = Convert.ToInt16(line[4]);
            tempUser.links = Convert.ToBoolean(line[5]);
            tempUser.follower = Convert.ToBoolean(line[6]);
            tempUser.subscriber = Convert.ToBoolean(line[7]);
            tempUser.general = Convert.ToBoolean(line[8]);
            tempUser.longWar = Convert.ToBoolean(line[9]);
            tempUser.x2 = Convert.ToBoolean(line[10]);

            userList.Add(tempUser);
        }

        file.Close();
        return userList;
    }

    public void UserReturn(List<User> userList)
    {
        StreamWriter sw = new StreamWriter(filePath + "masterList.txt");
        string temp;

        for (int i = 0; i < userList.Count; i++)
        {
            temp = userList[i].userName + "|" + userList[i].firstName + "|" + userList[i].nickName + "|" + userList[i].lastName + "|" +
                   userList[i].xP + "|" + userList[i].links + "|" + userList[i].follower + "|" + userList[i].subscriber + "|" +
                   userList[i].general + "|" + userList[i].longWar + "|" + userList[i].x2;
            sw.WriteLine(temp);
        }
        sw.Close();
    }

    public List<Soldier> SoldierTake(List<Soldier> userList)
    {
        string temp;
        string barrack = "XCOM.txt";
        
        StreamReader file = new StreamReader(filePath + barrack);

        while ((temp = file.ReadLine()) != null)
        {
            var line = temp.Split('|');

            Soldier tempUser = new Soldier();

            tempUser.userName = line[0];
            tempUser.firstName = line[1];
            tempUser.nickName = line[2];
            tempUser.lastName = line[3];
            tempUser.barracks = Convert.ToBoolean(line[4]);
            tempUser.active = Convert.ToBoolean(line[5]);
            tempUser.kIA = Convert.ToBoolean(line[6]);
            tempUser.x2Barracks = Convert.ToBoolean(line[7]);
            tempUser.x2Active = Convert.ToBoolean(line[8]);
            tempUser.x2KIA = Convert.ToBoolean(line[9]);
            userList.Add(tempUser);
        }

        file.Close();
        return userList;
    }

    public void SoldierReturn(List<Soldier> userList)
    {
        string temp;
        string barrack = "XCOM.txt";
        
        StreamWriter sw = new StreamWriter(filePath + barrack);

        for (int i = 0; i < userList.Count; i++)
        {
            temp = userList[i].userName + "|" + userList[i].firstName + "|" + userList[i].nickName + "|" + userList[i].lastName + "|" +
                   userList[i].barracks + "|" + userList[i].active + "|" + userList[i].kIA + "|" +
                   userList[i].x2Barracks + "|" + userList[i].x2Active + "|" + userList[i].x2KIA;
            sw.WriteLine(temp);
        }
        sw.Close();
    }

    public List<Soldier> CampaignReset(List<Soldier> userList)
    {   
        for(int i=0; i<userList.Count; i++)
        {
            userList[i].barracks = true;
            userList[i].active = false;
            userList[i].kIA = false;
        }
        return userList;
    }

    public List<NPC> NPC_Take(List<NPC> userList)
    {
        string temp;
        StreamReader file = new StreamReader(filePath + "Concourse_NPC");

        while ((temp = file.ReadLine()) != null)
        {
            var line = temp.Split('|');

            NPC tempUser = new NPC();

            tempUser.userName = line[0];
            tempUser.firstName = line[1];
            tempUser.lastName = line[3];
            tempUser.barracks = Convert.ToBoolean(line[4]);
            tempUser.active = Convert.ToBoolean(line[5]);
            tempUser.sex = Convert.ToInt16(line[6]);
            
            userList.Add(tempUser);
        }

        file.Close();
        return userList;
    }

    public void NPC_Return(List<NPC> userList)
    {
        string temp;
        StreamWriter sw = new StreamWriter(filePath + "Concourse_NPC");

        for (int i = 0; i < userList.Count; i++)
        {
            temp = userList[i].userName + "|" + userList[i].firstName + "|" + userList[i].lastName + "|" +
                   userList[i].barracks + "|" + userList[i].active + "|" + userList[i].sex + "|" + userList[i].race + "|" + userList[i].description;
            sw.WriteLine(temp);
        }
        sw.Close();
    }

    public List<User> EnlistUser(string user, List<User> userList)
    {
        if (userList.Exists(x => x.userName == user))
        {
            ;
        }
        else
        {
            User tempUser = new User();

            tempUser.userName = user;
            tempUser.firstName = "---";
            tempUser.nickName = "---";
            tempUser.lastName = "---";
            tempUser.xP = 0;
            tempUser.links = false;
            tempUser.follower = false;
            tempUser.subscriber = false;
            tempUser.general = true;
            tempUser.longWar = false;
            tempUser.x2 = false;

            userList.Add(tempUser);
        }
        return userList;
    }

    public List<User> XP_Distribute(List<string> activeList, List<User> masterList)
    {
        User tempUser = new User();
        for (int i = 0; i < activeList.Count; i++)
        {
            masterList[masterList.FindIndex(x => x.userName == activeList[i])].xP++;
        }
        return masterList;
    }

    public string UserReadout(ref List<Soldier> barracks, string user)
    {
        string temp = "";

        Soldier guy = new Soldier();
        guy = barracks[(barracks.FindIndex(x => x.userName.Contains(user.ToLower())))];
        if (guy.barracks)
        {
            temp = "Soldier is in Barracks.";
        }
        else if (guy.active)
        {
            temp = "Soldier is Active.";
        }
        else
        {
            temp = "Soldier is KIA.";
        }
        /*
        for (int i = 0; i < userList.Count; i++)
        {
            temp = temp + " | " + userList[i].userName;
        }
        */
        return temp;
    }

    public string ActiveReadout(List<string> userList)
    {
        string temp = "";
        for (int i = 0; i < userList.Count; i++)
        {
            temp = temp + " | " + userList[i];
        }
        return temp;
    }

    public string RankCall(int xp)
    {
        string temp = "";

        if(xp < 100) //Rookie 
        {
            temp = "Rookie";
        }
        else if (xp < 200) //SPEC (100)
        {
            temp = "Specialist";
        }
        else if (xp < 400) //CPL (200)
        {
            temp = "Corporal";
        }
        else if (xp < 800) //SGT (400)
        {
            temp = "Sergeant";
        }
        else if (xp < 1600) //MSGT (800)
        {
            temp = "Master Sergeant";
        }
        else //Central (1600)
        {
            temp = "Central";
        }
        return temp;
    }

    public string Promotion(ref List<Soldier> barracks, string viewer, int cS)
    {
        Soldier soldier = new Soldier();
        if (viewer.Equals(""))
        {
            int bs = barracks.Count;
            Random rnd = new Random();
            int start = rnd.Next(bs);
            for (int i = start; i < bs; i++)
            {
                if (cS == 1)
                {
                    if (barracks[i].barracks)
                    {
                        barracks[i].barracks = false;
                        barracks[i].active = true;
                        soldier = barracks[i];
                        return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                    }
                }
                else
                {
                    if (barracks[i].x2Barracks)
                    {
                        barracks[i].x2Barracks = false;
                        barracks[i].x2Active = true;
                        soldier = barracks[i];
                        return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                    }
                }
            }
            for (int i = 0; i< start; i++)
            {
                if (cS == 1)
                {
                    if (barracks[i].barracks)
                    {
                        barracks[i].barracks = false;
                        barracks[i].active = true;
                        soldier = barracks[i];
                        return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                    }
                }
                else
                {
                    if (barracks[i].x2Barracks)
                    {
                        barracks[i].x2Barracks = false;
                        barracks[i].x2Active = true;
                        soldier = barracks[i];
                        return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                    }
                }
            }
            return "Commander! The Barracks is emtpy, Sir! No more Soldiers available! o7";
        }
        else
        {
            if(barracks.Exists(x => x.userName.Equals(viewer.ToLower())))
            {
                if (cS == 1)
                {
                    soldier = barracks[barracks.FindIndex(x => x.userName.Equals(viewer.ToLower()))];
                    soldier.barracks = false;
                    soldier.active = true;
                    return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                }
                else
                {
                    soldier = barracks[barracks.FindIndex(x => x.userName.Equals(viewer.ToLower()))];
                    soldier.x2Barracks = false;
                    soldier.x2Active = true;
                    return "Commander! " + soldier.userName + "'s Soldier has been promoted! Welcome " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " to the XCOM Project! o7";
                }
            }
            else
            {
                return "Commander! No Soldier with such a name in the Barracks, Sir! o7";
            }
        }
    }

    public string Demotion(ref List<Soldier> barracks, string viewer, int cS)
    {
        Soldier soldier = new Soldier();
        if (barracks.Exists(x => x.userName.Equals(viewer.ToLower())))
        {
            if (cS == 1)
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(viewer.ToLower()))];
                soldier.barracks = true;
                soldier.active = false;
                return "Commander! " + soldier.userName + "'s Soldier has been demoted! Go back to Boot Camp Rookie " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + "! o7";
            }
            else
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(viewer.ToLower()))];
                soldier.x2Barracks = true;
                soldier.x2Active = false;
                return "Commander! " + soldier.userName + "'s Soldier has been demoted! Go back to Boot Camp Rookie " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + "! o7";
            }
        }
        else
        {
            return "Commander! No Soldier with such a name in the Base, Sir! o7";
        }
    }

    public string Kill(ref List<Soldier> barracks, string lastname, int cS)
    {
        Soldier soldier = new Soldier();
        if (barracks.Exists(x => x.userName.Equals(lastname)))
        {
            if (cS == 1)
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(lastname))];
                soldier.kIA = true;
                soldier.active = false;
                return "Commander! " + soldier.userName + "'s Soldier has been KIA! " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " will be avenged! o7";
            }
            else
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(lastname))];
                soldier.x2KIA = true;
                soldier.x2Active = false;
                return "Commander! " + soldier.userName + "'s Soldier has been KIA! " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " will be avenged! o7";
            }
        }
        else
        {
            return "Commander! No Soldier with such a name in the Base, Sir! o7";
        }
    }

    public string Revive(ref List<Soldier> barracks, string lastname, int cS)
    {
        Soldier soldier = new Soldier();
        if (barracks.Exists(x => x.userName.Equals(lastname)))
        {
            if (cS == 1)
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(lastname))];
                soldier.kIA = false;
                soldier.active = true;
                return "Commander! I thought " + soldier.userName + "'s Soldier, " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + ", had died in a fire! Welcome back! o7";
            }
            else
            {
                soldier = barracks[barracks.FindIndex(x => x.userName.Equals(lastname))];
                soldier.x2KIA = false;
                soldier.x2Active = true;
                return "Commander! I thought " + soldier.userName + "'s Soldier, " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + ", had died in a fire! Welcome back! o7";
            }
        }
        else
        {
            return "Commander! No Soldier with such a name in the Graveyard, Sir! o7";
        }
    }

    public string Search(ref List<Soldier> barracks, string parameter, int cS)
    {
        int j = 0;
        string location = "Graveyard!";
        string starting = "Commander! I have found these forms -> ";
        Soldier soldier = new Soldier();
        for(int i = 0; i < barracks.Count; i++)
        {
            if (barracks[i].userName.Equals(parameter.ToLower()) || barracks[i].lastName.ToLower().Contains(parameter.ToLower()) || barracks[i].firstName.ToLower().Contains(parameter.ToLower()) || barracks[i].nickName.ToLower().Contains(parameter.ToLower()))
            {
                soldier = barracks[i];
                j++;
                if (cS == 1)
                {
                    if (soldier.barracks)
                    {
                        location = "Barracks!";
                    }
                    else if (soldier.active)
                    {
                        location = "XCOM Project!";
                    }
                }
                else
                {
                    if (soldier.x2Barracks)
                    {
                        location = "Barracks!";
                    }
                    else if (soldier.x2Active)
                    {
                        location = "XCOM Project!";
                    }
                }
                starting = starting + "| " + soldier.userName + ": " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " is in the " + location + " | --- ";
            }
        }
        if (j == 0)
        {
            return "Commander! No entries with such a name. o7";
        }
        else
        {
            return starting + "o7";
        }
    }

    public string Numbers(ref List<Soldier> barracks, int cS)
    {
        int i = 0, j = 0, k = 0;
        for (int x=0;x<barracks.Count;x++)
        {
            if (cS == 1)
            {
                if (barracks[x].barracks)
                {
                    i++;
                }
                else if (barracks[x].active)
                {
                    j++;
                }
                else
                {
                    k++;
                }
            }
            else
            {
                if (barracks[x].x2Barracks)
                {
                    i++;
                }
                else if (barracks[x].x2Active)
                {
                    j++;
                }
                else
                {
                    k++;
                }
            }
        }
        return "Commander! There are currently " + i + " Rookies, " + j + " Soldiers, and " + k + " Gravestones. o7";
    }

    public string Reset(ref List<Soldier> barracks, int cS)
    {
        for (int i=0;i<barracks.Count;i++)
        {
            if (cS == 1)
            {
                barracks[i].barracks = true;
                barracks[i].active = false;
                barracks[i].kIA = false;
            }
            else
            {
                barracks[i].x2Barracks = true;
                barracks[i].x2Active = false;
                barracks[i].x2KIA = false;
            }
        }
        return "Campaign is Reset Commander! o7";
    }

    public Messages ValidMessage(OnChatCommandReceivedArgs e, ref List<User> masterList, ref List<string> activeList, ref List<Soldier>barracks, ref List<NPC>npc, int commandSwitch)
    {
        Messages temp = new Messages();
        temp.mExists = true;

        //---XCOM---//
        if (e.Command.CommandText.Equals("enlist"))
        {
            if (commandSwitch == 1)
            {
                temp.message = "Prepare for the Long War " + e.Command.ChatMessage.Username + " ! Check your Whispers.";
            }
            else if (commandSwitch == 2)
            {
                temp.message = "Prepare to be Chosen " + e.Command.ChatMessage.Username + " ! Check your Whispers.";
            }
            else
            {
                temp.message = "Wrong game! However we're always looking for recruits " + e.Command.ChatMessage.Username + " ! Check you Whispers.";
            }
            temp.wExists = true;
            temp.whisper = "Interested in joining the XCOM Project " + e.Command.ChatMessage.Username + "? Think of a First name, Last name, and Nick name you would like to use for your soldier. Type '!enlist' again in this chat for more instructions.";
        }
        else if (e.Command.CommandText.Equals("status") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if (barracks.Exists(x => x.userName.Contains(e.Command.ChatMessage.Username.ToLower())))
            {
                string ending = "";
                Soldier soldier = new Soldier();
                soldier = barracks[(barracks.FindIndex(x => x.userName.Contains(e.Command.ChatMessage.Username.ToLower())))];
                if (commandSwitch == 1)
                {
                    if (soldier.barracks)
                    {
                        ending = " in the Barracks awaiting promotion! o7";
                    }
                    else if (soldier.active)
                    {
                        ending = " active in the XCOM Project! o7";
                    }
                    else
                    {
                        ending = " KIA. Vigilo Confido o7";
                    }
                }
                else
                {
                    if (soldier.x2Barracks)
                    {
                        ending = " in the Barracks awaiting promotion! o7";
                    }
                    else if (soldier.x2Active)
                    {
                        ending = " active in the XCOM Project! o7";
                    }
                    else
                    {
                        ending = " KIA. Vigilo Confido o7";
                    }
                }
                temp.message = e.Command.ChatMessage.Username + " ! Your Soldier " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName + " is currently" + ending;
            }
            else
            {
                temp.message = e.Command.ChatMessage.Username + " ! You have not enlisted in the XCOM project yet! Check your Whispers.";
                temp.wExists = true;
                temp.whisper = "Interested in joining the XCOM Project " + e.Command.ChatMessage.Username + "? Think of a First name, Last name, and Nick name you would like to use for your soldier. Type '!enlist' again in this chat for more instructions.";
            }
        }
        else if (e.Command.CommandText.Equals("promote") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if (e.Command.ArgumentsAsList.Count == 0)
            {
                temp.message = Promotion(ref barracks, "", commandSwitch);
            }
            else if (e.Command.ArgumentsAsList.Count == 1)
            {
                temp.message = Promotion(ref barracks, e.Command.ArgumentsAsList[0], commandSwitch);
            }
            else
            {
                temp.message = "Commander! I can't read this chicken scratch. o7";
            }
        }
        else if (e.Command.CommandText.Equals("demote") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if(e.Command.ArgumentsAsList.Count == 1)
            {
                temp.message = Demotion(ref barracks, e.Command.ArgumentsAsList[0], commandSwitch);
            }
            else
            {
                temp.message = "Commander! I can't read this chicken scratch. o7";
            }
            
        }
        else if (e.Command.CommandText.Equals("kia") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if (e.Command.ArgumentsAsList.Count == 1)
            {
                temp.message = Kill(ref barracks, e.Command.ArgumentsAsList[0], commandSwitch);
            }
            else
            {
                temp.message = "Commander! I can't read this chicken scratch. o7";
            }
            
        }
        else if (e.Command.CommandText.Equals("revive") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if (e.Command.ArgumentsAsList.Count == 1)
            {
                temp.message = Revive(ref barracks, e.Command.ArgumentsAsList[0], commandSwitch);
            }
            else
            {
                temp.message = "Commander! I can't read this chicken scratch. o7";
            }
            
        }
        else if(e.Command.CommandText.Equals("search") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            if (e.Command.ArgumentsAsList.Count == 1)
            {
                temp.message = Search(ref barracks, e.Command.ArgumentsAsList[0], commandSwitch);
            }
            else
            {
                temp.message = "Commander! I can't read this chicken scratch. o7";
            }
        }
        else if (e.Command.CommandText.Equals("numbers") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            temp.message = Numbers(ref barracks, commandSwitch);
        }
        else if (e.Command.CommandText.Equals("campaignreset?_areyousure?_really?_okaythen!") && e.Command.ChatMessage.Username.Equals("wittel3") && (commandSwitch == 1 || commandSwitch == 2))
        {
            temp.message = Reset(ref barracks, commandSwitch);
        }
        //---DnD---//
        else if (e.Command.CommandText.Equals("npc"))
        {
            temp.message = "Hello " + e.Command.ChatMessage.Username + " ! Thank you for wanting to contribute a NPC to the campaign! Check your whispers to proceed with this process.";
            temp.wExists = true;
            temp.whisper = "T;hanks again " + e.Command.ChatMessage.Username + "for helping Wittel flesh out this world! Please INSTRUCTIONS. Thank you! o7"; //TODO
        }
        //---General---//
        else if (e.Command.CommandText.Equals("ping") && e.Command.ChatMessage.Username == "wittel3")
        {
            temp.message = "Do you love dogs and RNG? Subscribe today, free with Prime, and praise both! wittelPraise wittelBaron wittelPraise";
            //temp.message = "Pong!";
        }
        else if (e.Command.CommandText.Equals("test") && e.Command.ChatMessage.Username == "wittel3")
        {
            Console.WriteLine("? : " + UserReadout(ref barracks, e.Command.ChatMessage.Username));
        }
        else if (e.Command.CommandText.Equals("commands"))
        {
            temp.message = "Here are the available Wittel_Bot commands -> http://bit.ly/WB_Commands";
        }
        else if (e.Command.CommandText.Equals("motd"))
        {
            temp.message = motd;
        }
        else if (e.Command.CommandText.Equals("discord"))
        {
            temp.message = "Join Wittel's Barrack here! https://discord.gg/hzZCTqK";
        }
        else if (e.Command.CommandText.Equals("twitter"))
        {
            temp.message = "Tweet at Wittel here! https://twitter.com/Wittel3";
        }
        else if (e.Command.CommandText.Equals("youtube"))
        {
            temp.message = "Wittel used to make some videos! https://www.youtube.com/user/Wittel3";
        }
        else if (e.Command.CommandText.Equals("map"))
        {
            temp.message = "Where in the world are you? No, really. Where? https://www.zeemaps.com/map?group=1463814&add=1";
        }
        else if (e.Command.CommandText.Equals("xp"))
        {
            int i = masterList[masterList.FindIndex(x => x.userName == e.Command.ChatMessage.Username)].xP;
            temp.mExists = false;
            temp.wExists = true;
            temp.whisper = e.Command.ChatMessage.Username + "! You currently have " + i + " XP! This means you have the rank of " + RankCall(i) + "!";
        }
        else if (e.Command.CommandText.Equals("eos") && e.Command.ChatMessage.Username == "wittel3")
        {
            temp.message = "Thanks for watching everyone! Be sure to ";
        }
        else if (e.Command.CommandText.Equals("raid_message") && e.Command.ChatMessage.Username == "wittel3")
        {
            temp.message = "PraiseIt \\o/ PraiseIt";
        }
        else if (e.Command.CommandText.Equals("wittel_bot"))
        {
            temp.mExists = false;
            temp.wExists = true;
            temp.whisper = "Hello World! I am a C# Bot that utilizes mostly Twitch Lib.";
        }
        else if (e.Command.CommandText.Equals("spotlight"))
        {
            if (e.Command.ChatMessage.Username.Equals("wittel3"))
            {
                temp.message =  "Everyone! Please go give " + e.Command.ArgumentsAsString + " a follow over at https://www.twitch.tv/" +
                                e.Command.ArgumentsAsString.ToLower() + " ! They are a wonderful streamer and deserve some love! <3";
            }
            else
            {
                temp.mExists = false;
                temp.wExists = true;
                temp.whisper = "Hey! Only the Commander can use that command!";
            }
        }
        else
        {
            temp.message = "Sorry " + e.Command.ChatMessage.Username + " ! I don't understand that command! Double check or use '!commands' for a detailed list.";
        }
        return temp;
    }

    public Messages ValidWhisper(OnWhisperCommandReceivedArgs e, ref List<User> masterList, ref List<string> activeList, ref List<Soldier>barracks, ref List<NPC>npc, int commandSwitch)
    {
        Messages temp = new Messages();
        temp.wExists = true;
        //---XCOM---//
        if (e.Command.CommandText.Equals("enlist"))
        {
            if (e.Command.ArgumentsAsList.Count == 0)
            {
                temp.whisper =  "If you are ready to enlist your soldier then please type in '!enlist [FIRST] [NICK] [LAST]' for your Soldier. " +
                                "For example Wittel's Soldier was input as '!enlist Commander Little Wittel' without the quotes. o7";
            }
            else if (e.Command.ArgumentsAsList.Count == 3)
            {
                Soldier soldier = new Soldier();
                soldier.userName = e.Command.WhisperMessage.Username.ToLower();
                soldier.firstName = e.Command.ArgumentsAsList[0];
                soldier.nickName = e.Command.ArgumentsAsList[1];
                soldier.lastName = e.Command.ArgumentsAsList[2];
                soldier.barracks = true;
                soldier.x2Barracks = true;

                temp.mExists = true;
                temp.message = e.Command.WhisperMessage.Username + " ! Your Soldier " + soldier.firstName + " '" + soldier.nickName + "' " + soldier.lastName +
                                " has been enlisted in the XCOM Project! Vigilo Confido! o7";
                if (barracks.Exists(x => x.userName.Contains(soldier.userName)))
                {
                    temp.mExists = false;
                    if (barracks[barracks.FindIndex(x => x.userName.Contains(soldier.userName))].active)
                    {
                        temp.whisper = "Sorry " + e.Command.WhisperMessage.Username + "! Your Soldier is currently in Active Duty, and cannot be renamed at this moment.";
                        return temp;
                    }
                    else if (barracks[barracks.FindIndex(x => x.userName.Contains(soldier.userName))].kIA)
                    {
                        soldier.barracks = false;
                        soldier.kIA = true;
                        barracks.RemoveAt(barracks.FindIndex(x => x.userName.Contains(soldier.userName)));
                    }
                    else
                    {
                        barracks.RemoveAt(barracks.FindIndex(x => x.userName.Contains(soldier.userName)));
                    }
                }
                barracks.Add(soldier);
                temp.whisper =  "Congrats! Your Soldier " + e.Command.ArgumentsAsList[0] + " '" + e.Command.ArgumentsAsList[1] + "' " + e.Command.ArgumentsAsList[2] + 
                                " has been added to the Barracks! o7";
            }
            else
            {
                temp.whisper = "Error! Please double check your Enlistment Form!";
            }
        }
        else if (e.Command.CommandText.Equals("npc"))
        {
            if (e.Command.ArgumentsAsList.Count == 0)
            {
                temp.whisper = "INSTRUCTIONS"; //TODO
            }
            else if (e.Command.ArgumentsAsList.Count >= 4)
            {

            }
            else if (e.Command.ArgumentsAsList[0].ToLower().Equals("random"))
            {
                ; //TODO
            }
            else
            {
                temp.whisper = "Something went wrong! Please review the rules for creating a NPC! ('!npc' to see the rules once more)";
            }
        }
        //---General---//
        else if (false)
        {

        }
        else if (e.Command.CommandText.Equals("commands"))
        {
            temp.whisper = "Here are my available commands -> http://bit.ly/WB_Commands";
        }
        else
        {
            temp.whisper = "Sorry! I don't understand that command! Double check or use '!commands' for a detailed list.";
        }
        return temp;
    }
}
        