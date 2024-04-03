using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Arquivo
{
    public interface Registro : IComparable, ICloneable 
    {
        int ID {get; set;}
        void FromByteArray(byte[] registro);
        new int CompareTo(object registro);
        new object Clone();

    }
}