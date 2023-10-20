using ChatTranslate;
using ChatTwo.Ui;
using ChatTwo.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatTwo
{
    public static class CustomFeatures
    {
        public static Plugin? _plugin { get; set; }
        private static string _lastMatched = ""; 
        internal static bool MatchMessage(Message message)
        {
            if (_plugin == null || _lastMatched.Equals($"{message.SenderSource.TextValue}:{message.ContentSource.TextValue}"))
            {
                return false;
            }
            try
            {
                if (message == null) { return false; }
                //if (!Slutify.whiteList.Contains(message.SenderSource.TextValue)) { return false; }
                var match = Regex.Match(message.ContentSource.TextValue, @"(?i)\b(?:slut,)\s+(?:\((.*?)\)|(\w+))");
                if (match.Success)
                {
                    _lastMatched = $"{message.SenderSource.TextValue}:{message.ContentSource.TextValue}";
                    SendMessage($"/{match.Groups[2].Value} motion");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Plugin.Log.Debug(ex.Message);
                return false;
            }
        }
        static void SendMessage(string inputRaw)
        {
            if (_plugin == null)
            {
                return;
            }
            try
            {
                Plugin.Log.Debug($"{inputRaw}");
                var bytes = Encoding.UTF8.GetBytes(inputRaw);
                AutoTranslate.ReplaceWithPayload(_plugin.DataManager, ref bytes);
                _plugin.Common.Functions.Chat.SendMessageUnsafe(bytes);
            }
            catch (Exception ex)
            {
                Plugin.Log.Debug(ex.Message);
            }
}
    }
}
