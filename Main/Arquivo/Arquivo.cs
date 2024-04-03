using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Main.Arquivo
{
    public class Arquivo<T> where T: Registro
    {
        protected FileStream arquivo;
        protected ConstructorInfo constructor;
        private Func<RegistroEntrance> value;
        protected readonly int TAM_CABECALHO = 4;


        static byte[] toByteArray(T obj)
        {
            if(obj == null)
            return null;


            byte[] bytesID = BitConverter.GetBytes(obj.ID);
            string objectString = obj.ToString();
            


            byte[] byteObjeto = Encoding.UTF8.GetBytes(objectString);

            return byteObjeto;
            
        }

        public Arquivo(ConstructorInfo constructor)
        {
            this.constructor = constructor;
            arquivo = new FileStream("nomes.db", FileMode.OpenOrCreate);

            if(arquivo.Length < TAM_CABECALHO)
            {
                arquivo.Seek(0, SeekOrigin.Begin);
                arquivo.Write(BitConverter.GetBytes(0), 0, 4);
            }
        }

        public Arquivo(Func<RegistroEntrance> value)
        {
            this.value = value;
        }

        public void Create(T obj)
        {
            /*
             * Metodo crate, vou para a posicao 0, pego a quantidade de registros,
            * salvo em uma variavel, em seguida incremento 1 unidade, volto para a
            * posicao 0 do arquivo e sobrescrevo com o TAMANHO++.
            */
            arquivo.Seek(0, SeekOrigin.Begin);
            int lastID = arquivo.ReadByte();
            lastID++;
            arquivo.Seek(0, SeekOrigin.Begin);
            arquivo.Write(BitConverter.GetBytes(lastID), 0, 4);


            /*
            * Agora iremos para a posicao final do rquivo para salvermos o registro ,
            * transformando-o em array de byte. 
            * Esquema do registro |Tamanho|Lapide|Conteudo|
            */

            arquivo.Seek(0, SeekOrigin.End);
            long endereco = arquivo.Position;
            byte[] registro =  toByteArray(obj);
            short tam = (short) registro.Length;
            byte[] tamByte = BitConverter.GetBytes(tam);
            arquivo.WriteByte(32); //Codigo ASCII do espaco em branco  
            arquivo.Write(tamByte, 0, 2);
            arquivo.Write(registro, 0, registro.Length);
            arquivo.Flush();
            arquivo.Close();

            Console.WriteLine("O Registro foi salvo com sucesso");

        }

        public bool Delete(int id)
        {

            



            return false;
        }

    }

    
}