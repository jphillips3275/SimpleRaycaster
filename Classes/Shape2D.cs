using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Engine.Classes
{
    public class Shape2D
    {
        public Vector Position = null;
        public Vector Scale = null;
        public string Tag = "";
        public int DirectionIndex = 0;

        public Shape2D(Vector Position, Vector Scale, string tag, int DirectionIndex)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = tag;
            this.DirectionIndex = DirectionIndex;

            Log.Info($"[SHAPE2D]({tag}) has been registered");
            Engine.RegisterShape(this);
        }

        public void DestroySelf()
        {
            Log.Info($"[SHAPE2D] has been destroyed");
            Engine.UnRegisterShape(this);
        }

        public void UpdateShape(Vector Position, Vector Scale)
        {
            this.Position = Position;
            this.Scale = Scale;
        }
    }
}
