using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public byte[] ToByteArray()
        {


            byte[] bytesID = BitConverter.GetBytes(ID);
            string nome = Nome.ToString();
            string sobrenome = Sobrenome.ToString();

            byte[] bytesNome = Encoding.UTF8.GetBytes(nome);
            byte[] byteSobrenome = Encoding.UTF8.GetBytes(sobrenome);

            List<byte> listaBytes = new List<byte>();
            
            listaBytes.AddRange(bytesID);
            listaBytes.AddRange(bytesNome);
            listaBytes.AddRange(byteSobrenome);

            byte[] registro = listaBytes.ToArray();

            foreach(byte b in registro)
            {
                Console.Write(b + " ");
            }
            return registro;
        }

        public override string ToString()
        {
            return $"{ID}{Nome}{Sobrenome}";
        }
    }

}