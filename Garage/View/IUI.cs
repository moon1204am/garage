using Garage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Garage.View
{
    internal interface IUI
    {
        string GetInput();
        void Print(string msg);
        //Det skall gå att stänga av applikationen från gränssnittet
        //void ShutDown();
        void Display();

    }
}
