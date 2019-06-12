using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECCL;
using ECCL.src;

namespace EWB_GUI_Alpha.Context
{
    public class LibContext
    {
        private static LibContext instance;

        private Circuit Circuit { get; set; }

        private LibContext()
        {
            Circuit = new Circuit();
        }

        public static LibContext GetInstance()
        {
            if (instance == null)
                instance = new LibContext();
            return instance;
        }

        public void AddComponent()
        {

        }

        public void RemoveComponent()
        {

        }

        public void ConnectComponent()
        {

        }

        public void DisconnectComponent()
        {

        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
