using System.Reflection;
using Main.Arquivo;

ConstructorInfo constructor = typeof(Registro).GetConstructor(Type.EmptyTypes);
Arquivo<Registro> arquivo = new Arquivo<Registro>(constructor);

RegistroEntrance objeto = new RegistroEntrance();
RegistroEntrance objeto2 = new RegistroEntrance();

objeto.ID = 3;
objeto.Nome = "Matheus";
objeto.Sobrenome = "Fagundes";

objeto2.ID = 2;
objeto2.Nome = "Luca";
objeto2.Sobrenome = "Lorensco";


arquivo.Create(objeto);
arquivo.Create(objeto2);

arquivo.Delete(2);