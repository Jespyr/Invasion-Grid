using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;

namespace InvasionGrid
{
    class Utility
    {
        private static Utility instance;

        private Utility() { }

        public static Utility  Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Utility();
                }
                return instance;
            }
        }

        public float getVector2fLengthSquared(Vector2f vec)
        {
            return vec.X * vec.X + vec.Y * vec.Y;
        }
    }
}
