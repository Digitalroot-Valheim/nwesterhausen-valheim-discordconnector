﻿using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace DiscordConnector.Config
{
    internal class MainConfig
    {
        private static ConfigFile config;
        private static List<String> mutedPlayers;
        private const string MAIN_SETTINGS = "Main Settings";

        // Main Settings
        private ConfigEntry<string> webhookUrl;
        private ConfigEntry<bool> discordEmbedMessagesToggle;
        private ConfigEntry<string> mutedDiscordUserlist;
        private ConfigEntry<bool> statsAnnouncementToggle;
        private ConfigEntry<int> statsAnnouncementPeriod;
        private ConfigEntry<bool> colectStatsToggle;
        private ConfigEntry<bool> sendPositionsToggle;
        private ConfigEntry<bool> announcePlayerFirsts;
        private ConfigEntry<bool> discordBotToggle;
        private ConfigEntry<int> numberRankingsListed;

        public MainConfig(ConfigFile configFile)
        {
            config = configFile;
            LoadConfig();

            mutedPlayers = new List<string>(mutedDiscordUserlist.Value.Split(';'));
        }

        public void ReloadConfig()
        {
            config.Reload();
            config.Save();
            mutedPlayers = new List<string>(mutedDiscordUserlist.Value.Split(';'));

        }


        private void LoadConfig()
        {

            webhookUrl = config.Bind<string>(MAIN_SETTINGS,
                "Webhook URL",
                "",
                "Discord channel webhook URL. For instructions, reference the 'MAKING A WEBHOOK' section of " + Environment.NewLine +
                "Discord's documentation: https://support.Discord.com/hc/en-us/articles/228383668-Intro-to-Webhook");

            discordEmbedMessagesToggle = config.Bind<bool>(MAIN_SETTINGS,
                "Use fancier discord messages",
                false,
                "Enable this setting to use embeds in the messages sent to Discord. Currently this will affect the position details for the messages.");

            mutedDiscordUserlist = config.Bind<string>(MAIN_SETTINGS,
                "Ignored Players",
                "",
                "It may be that you have some players that you never want to send Discord messages for. Adding a player name to this list will ignore them." + Environment.NewLine +
                "Format should be a semicolon-separated list: Stuart;John McJohnny;Weird-name1");

            sendPositionsToggle = config.Bind<bool>(MAIN_SETTINGS,
                "Send Positions with Messages",
                true,
                "Disable this setting to disable any positions/coordinates being sent with messages (e.g. players deaths or players joining/leaving). (Overwrites any individual setting.)");

            colectStatsToggle = config.Bind<bool>(MAIN_SETTINGS,
                "Collect Player Stats",
                true,
                "Disable this setting to disable all stat collection. (Overwrites any individual setting.)");

            statsAnnouncementToggle = config.Bind<bool>(MAIN_SETTINGS,
                "Periodic Player Stats Notifications",
                false,
                "Disable this setting to disable all stat announcements (i.e. leader board messages). (Overwrites any individual setting.)" + Environment.NewLine +
                "EX: Top Player Deaths: etc etc Top Player Joins: etc etc");

            statsAnnouncementPeriod = config.Bind<int>(MAIN_SETTINGS,
                "Player Stats Notifications Period",
                600,
                "Set the number of minutes between a leader board announcement sent to discord." + Environment.NewLine +
                "This time starts when the server is started. Default is set to 10 hours (600 mintues).");

            announcePlayerFirsts = config.Bind<bool>(MAIN_SETTINGS,
                "Announce Player Firsts",
                true,
                "Disable this setting to disable all extra announcements the first time each player does something. (Overwrites any individual setting.)");

            discordBotToggle = config.Bind<bool>(MAIN_SETTINGS,
                "Enable Discord Bot Integration",
                false,
                "Enable this setting to allow Discord Bot integration with this plugin. See the -bot.cfg file for all the config options related to this integration." + Environment.NewLine +
                "When this is turned on, a listening HTTP webhook opens on a port specified in the config. This enables the Discord bot to communicate with this plugin (and the server).");

            numberRankingsListed = config.Bind<int>(MAIN_SETTINGS,
                "How many places to list in the top ranking leaderboards",
                3,
                "Set how many places (1st, 2nd, 3rd by default) to display when sending the ranked leaderboard.");


            config.Save();
        }

        public string ConfigAsJson()
        {
            string jsonString = "{";
            jsonString += "\"discord\":{";
            jsonString += $"\"webhook\":\"{(string.IsNullOrEmpty(WebHookURL) ? "unset" : "REDACTED")}\",";
            jsonString += $"\"fancierMessages\":\"{DiscordEmbedsEnabled}\",";
            jsonString += $"\"ignoredPlayers\":\"{mutedDiscordUserlist.Value}\",";
            jsonString += $"\"botWebhookEnabled\":\"{DiscordBotEnabled}\"";
            jsonString += "},";
            jsonString += $"\"periodicLeaderboardEnabled\":\"{StatsAnnouncementEnabled}\",";
            jsonString += $"\"periodicLeaderboardPeriodSeconds\":{StatsAnnouncementPeriod},";
            jsonString += $"\"colectStatsEnabled\":\"{CollectStatsEnabled}\",";
            jsonString += $"\"sendPositionsEnabled\":\"{SendPositionsEnabled}\",";
            jsonString += $"\"announcePlayerFirsts\":\"{AnnouncePlayerFirsts}\",";
            jsonString += $"\"numberRankingsListed\":\"{IncludedNumberOfRankings}\"";
            jsonString += "}";
            return jsonString;
        }

        public string WebHookURL => webhookUrl.Value;
        public bool StatsAnnouncementEnabled => statsAnnouncementToggle.Value;
        public int StatsAnnouncementPeriod => statsAnnouncementPeriod.Value;
        public bool CollectStatsEnabled => colectStatsToggle.Value;
        public bool DiscordEmbedsEnabled => discordEmbedMessagesToggle.Value;
        public bool SendPositionsEnabled => sendPositionsToggle.Value;
        public List<string> MutedPlayers => mutedPlayers;
        public bool AnnouncePlayerFirsts => announcePlayerFirsts.Value;
        public bool DiscordBotEnabled => discordBotToggle.Value;
        public int IncludedNumberOfRankings => numberRankingsListed.Value;

    }
}
