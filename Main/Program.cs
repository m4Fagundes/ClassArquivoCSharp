﻿using System.Reflection;
using Main.Arquivo;

ConstructorInfo constructor = typeof(Registro).GetConstructor(Type.EmptyTypes);
Arquivo<Registro> arquivo = new Arquivo<Registro>(constructor);

var objeto = new RegistroEntrance();

objeto.ID = 2012301230;
objeto.Nome = "Matheus";
objeto.Sobrenome = "Fagundes";

arquivo.Create(objeto);