using System.Drawing;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.RAGEAPI.Extensions;

namespace TDS_Server.RAGEAPI.TextLabel
{
    class TextLabel : Entity.Entity, ITextLabel
    {
        private readonly GTANetworkAPI.TextLabel _instance;

        public TextLabel(GTANetworkAPI.TextLabel textLabel)
            : base(textLabel)
        {
            _instance = textLabel;
        }

        public Color Color
        {
            get => _instance.Color.ToTDS();
            set => _instance.Color = value.ToMod();
        }
        public string Text 
        { 
            get => _instance.Text; 
            set => _instance.Text = value; 
        }
        public bool Seethrough 
        { 
            get => _instance.Seethrough;
            set => _instance.Seethrough = value; 
        }
        public float Range 
        { 
            get => _instance.Range; 
            set => _instance.Range = value;
        }
    }
}
