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
        

        // static byte[] toByteArray(T obj)
        // {
        //     if(obj == null)
        //     return null;


        //     byte[] bytesID = BitConverter.GetBytes(obj.ID);
        //     string objectString = obj.ToString();
            


        //     byte[] byteObjeto = Encoding.UTF8.GetBytes(objectString);

        //     return byteObjeto;
            
        // }



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
            byte[] registro =  obj.ToByteArray();
            short tam = (short) registro.Length;
            byte[] tamByte = BitConverter.GetBytes(tam);
            arquivo.WriteByte(32); //Codigo ASCII do espaco em branco  
            arquivo.Write(tamByte, 0, tamByte.Length);
            arquivo.Write(registro, 0, registro.Length);
            arquivo.Flush();

            Console.WriteLine("O Registro foi salvo com sucesso");

        }

        public void Delete(int id)
        {
            try
            {
                arquivo.Seek(TAM_CABECALHO, SeekOrigin.Begin);
                
                long tamanhoArquivo = arquivo.Length;
                
                using (BinaryReader reader = new BinaryReader(arquivo))
                {
                    while(arquivo.Position < tamanhoArquivo)
                    {
                        long endereco = arquivo.Position;
                        
                        //le a lapide
                        byte lapide = (byte) arquivo.ReadByte();
                        Console.WriteLine(lapide);  

                        //le o tamanho do registro
                        short tamanhoReg = reader.ReadInt16();
                        Console.WriteLine(tamanhoReg);

                        //grava o indice do registro pra comparacao
                        int indice = reader.ReadInt32();
                        Console.WriteLine(indice);

                        if(indice == id)
                        {
                            arquivo.Seek(endereco, SeekOrigin.Begin);
                            arquivo.WriteByte(42);
                            arquivo.Flush();
                            break;
                        }
                        else
                        {
                            arquivo.Seek(tamanhoReg-4, SeekOrigin.Current);

                        }
                        Console.WriteLine("Aq acabou!!");
                    }
                } 
            } catch (Exception e)
            {
                Console.WriteLine("Ocorreu uma excenssao: " + e);
            }
            

        }

    }
}


            // foreach(byte b in intBuffer)
            // {
            //     Console.WriteLine(b + " ");
            // }