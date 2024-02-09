using System.ComponentModel;
using Exiled.API.Interfaces;

namespace AnomalyHub
{
    public class Config : IConfig
    {
        [Description("Plugin is Enabled?")]
        public bool IsEnabled { get; set; } = true;
        [Description("Plugin Debug is Enabled?")]
        public bool Debug { get; set; } = false;
        [Description("Chaos spawn message (cassie style)")]
        public string CIspawnMessage { get; set; } = "Chaos Insurgency HasEntered from Gate Alpha";
        [Description("Round start message (cassie style)")]
        public string RoundStartMessage { get; set; } = "pitch_0.2 .g4 .g4 pitch_0.9 The Site is Experiencing Many Keter and Euclid Level SCP Containment Breaches. Full Site Lock down Initiated. pitch_1.0";
        [Description("Round is ended Friendly Fire will be open (true or false), dont use caps")]
        public string AutoFF { get; set; } = "true";
    }
}
