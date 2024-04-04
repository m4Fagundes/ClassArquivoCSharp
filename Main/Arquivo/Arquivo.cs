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

            long espacoVazio = -1;  // valor padrao no campo de armazenagem de um endereco que indica registro deletado
            byte[] espacoVazilByte = BitConverter.GetBytes(espacoVazio);

            long endereco = arquivo.Position; //TODO indici direto para ser feito

            byte[] registro =  obj.ToByteArray();  //tranforma o objeto de entrada em um registro a ser armazenado no arquivo

            short tam = (short) registro.Length; // tamanho do registro para que consiga le-lo
            byte[] tamByte = BitConverter.GetBytes(tam); 


            //escrita dos dados a cima
            arquivo.Write(espacoVazilByte, 0, 8);
            arquivo.WriteByte(32); //Codigo ASCII do espaco em branco  
            arquivo.Write(tamByte, 0, tamByte.Length);
            arquivo.Write(registro, 0, registro.Length);
            arquivo.Flush();

            Console.WriteLine("O Registro foi salvo com sucesso");

        }

        public void Delete(int id)
        {
            /*
            * Nesse codigo iremos pegar o tamanho do arquivo e percorre o ponteiro de
            * posicao em posicao, primeiro nos metadados do registo e em seguida o id
            */
            try
            {
                arquivo.Seek(TAM_CABECALHO, SeekOrigin.Begin);
                
                long tamanhoArquivo = arquivo.Length;
                
                using (BinaryReader reader = new BinaryReader(arquivo))
                {
                    while(arquivo.Position < tamanhoArquivo)
                    {
                        
                        long espacoVazil = reader.ReadInt64(); //campo que armazenara o ponteiro de um regisro deletado
                        Console.WriteLine(espacoVazil);
                        
                        long endereco = arquivo.Position;


                        //le a lapide
                        byte lapide = (byte) arquivo.ReadByte(); //campo que marca se um registo e valido ou nao
                        Console.WriteLine(lapide);

                        //le o tamanho do registro
                        short tamanhoReg = reader.ReadInt16(); // comapo de indica o tamanho do registro
                        Console.WriteLine(tamanhoReg);

                        //grava o indice do registro pra comparacao
                        int indice = reader.ReadInt32(); //leitura do incie, dentro do registro (int na primeira posicao do registro)

                        if(indice == id) //achou o registro para ser deletado marca lapide com *, delentando logicamente
                        {
                            arquivo.Seek(endereco, SeekOrigin.Begin);
                            arquivo.WriteByte(42);
                            arquivo.Flush();
                            break;
                        }
                        else
                        {
                            arquivo.Seek(tamanhoReg-4, SeekOrigin.Current); //caso nao ache o registro pula ele e verifica o proximo na poxima excucao do while

                        }
                    }
                } 
            } catch (Exception e)
            {
                Console.WriteLine("Ocorreu uma excenssao: " + e);
            }
            

        }

    }
}
