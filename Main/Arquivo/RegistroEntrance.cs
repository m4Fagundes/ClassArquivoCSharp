using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Arquivo
{
    public class RegistroEntrance : Registro
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Sobrenome{ get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object registro)
        {
            throw new NotImplementedException();
        }

        public void FromByteArray(byte[] registro)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{ID}{Nome}{Sobrenome}";
        }
    }
}