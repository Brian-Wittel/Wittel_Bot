using System;
using TwitchLib;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Api.V5.Models.Users;
using TwitchLib.Client;
using System.Collections.Generic;
using TwitchLib.Api.Interfaces;

namespace Wittel_Bot
{
    class userCommands
    {
        int commandSwitch;
        string command, user;
        string returnString;

        public userCommands(string message, string username, int number)
        {
            this.command = message;
            this.user = username;
            this.commandSwitch = number;
        }

        public string basicCommand()
        {
            if (this.command.StartsWith("!"))
            {
                return exclaimCommands(this.command, this.user, this.commandSwitch);
            }
            else if (this.command.StartsWith("?"))
            {
                return questionCommands(this.command, this.user, this.commandSwitch);
            }
            return null;
        }

        public string exclaimCommands(string input, string user, int number)
        {
            //All Commands
            if(number >= 0)
            {

            // XCOM Commands
                if (number == 1)
                {

                }

            // Dungeons & Dragons Commands
                else if (number == 2)
                {

                }

            // League of Legends Commands
                else if (number == 3)
                {

                }

            // Serious Commands (ping, motd, map, twitter, uptime, pfc, enlist, spotlight [], EOS, commands)
                if (input.Equals("!ping"))
                {
                    return "Pong!";
                }
                else if (input.Equals("!commands"))
                {

                }
                else if (input.Equals("!map"))
                {
                    return "Where in the world are you? Literally. Where? https://www.zeemaps.com/map?group=1463814&add=1#";
                }
                else if (input.Equals("!twitter"))
                {

                }
                else if (input.Equals("!uptime"))
                {

                }
                // Allows for users to post links (Human verification)
                else if (input.Equals("!pfc"))
                {

                }
                else if (input.Contains("!enlist"))
                {

                }
                // Throws a viewers channel into chat calling for a follow
                else if (input.Contains("!spotlight"))
                {
                    if (user.Equals("wittel3"))
                    {
                        if (input.Length < 11)
                        {
                            return "Hey! " + user + "! Actually add someone to spotlight, dummy.";
                        }
                        else
                        {
                            string sub = input.Substring(11);
                            return "Everyone! Please go scout out https://www.twitch.tv/" + sub + " and give them a follow!";
                        }

                    }
                    else
                    {
                        return user + ", you do not have permission to use this feature.";
                    }

                }
                else if (input.Equals("!EOS"))
                {

                }



            }
            return "Invalid usage " + user + ". Please use a valid command!";
        }

        public string questionCommands(string input, string user, int number)
        {

            return returnString;
        }
    }
}