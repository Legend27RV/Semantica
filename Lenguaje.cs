/*Rodríguez Villicaña Leonardo*/
using System;
using System.Collections.Generic;
//(X) Publicar en github: git push https://github.com/Legend27RV/Semantica
//(X) Requerimiento 1.- Eliminar las dobles comillas del printf e interpretar las secuencias de escape
//                      dentro de la cadena
//(X) Requerimiento 2.- Marcar los errores sintacticos cuando la variable no exista
//(X) Requerimiento 3.- Modificar el valor de la variable en la asignacion
//(X) Requerimiento 4.- Obtener el valor de la variable cuando se requiera y programar getValor
//(X) Requerimiento 5.- Modificar el Valor de la variable en el Scanf

//(X) Requerimiento 2.1.- Actualizar el dominante para variables en la expresion
//                        Ejemplo: float x; char y; y=x;
//(X) Requerimiento 2.2.- Actualizar el dominante para el casteo y el valor de la subexpresion
//(X) Requerimiento 2.3.- Programar un metodo de conversion de un valor a un tipo de dato
//                        Ejemplo: private float convert(float valor, string TipoDato)
//                        Deberan usar el residuo de la division %255, %65535
//(X) Requerimiento 2.4.- Evaluar nuevamente la condicion del if - else(X), while(X), for(X), do while(X)  
//                        con respecto al parametro que recibe
//(X) Requerimiento 2.5.- Levantar una excepcion en el scanf cuando la captura no sea un numero
//(x) Requerimiento 2.6.- Ejecutar el for();

//( ) Requerimiento 3.1- Actualizacion: 
//                       A) Agregar el residuo de la division en PorFactor
//                       B) Agregar en Asignacion los incremetos de termino y factor
//                       a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1
//                       en donde el 1 puede ser cualquier expresion
//                       C) Programar el destructor para ejecutar el metodo cerrarArchivo (metodo cerrar de lexico)
//                       #libreria especiaL? contenedor?
//                       en la clase lexico
//( ) Requerimiento 3.2- Actualizacion la Venganza xd:
//                       C) Marcar errores semanticos cuando los incrementos de termino o factor superen el rango de la variable(char, int, float)
//                       D) Considerar el inciso B y C para el for
//                       E) Hacer que funcione el do-while y el while
namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();
        Variable.TipoDato dominante;
        public Lenguaje()
        {

        }
        public Lenguaje(string nombre) : base(nombre)
        {

        }
        ~Lenguaje()
        {
            Console.WriteLine("Destructor");
            cerrar();
        }
        //public string Replace (string oldChar, string? newChar);
        private void addVariable(String nombre,Variable.TipoDato tipo)
        {
            variables.Add(new Variable(nombre, tipo));
        }
        private void displayVariables()
        {
            log.WriteLine("Variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre()+" "+v.getTipo()+" "+v.getValor());
            }
        }
        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return true;
                }
            }
            return false;
        }
        private void modVariable(string nombre, float nuevoValor)
        {
            foreach(Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    v.setValor(nuevoValor);
                }
            }
        }
        private float getValor(string nombre)
        {
            foreach(Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return v.getValor();
                }
            }
            return 0;
        }
        private Variable.TipoDato getTipo(string nombre)
        {
            foreach(Variable v in variables)
            {
                if (v.getNombre().Equals(nombre))
                {
                    return v.getTipo();
                }
            }
            return Variable.TipoDato.Char;
        }
        //Requerimiento 2.3: Programar un metodo de conversion de un valor a un tipo de dato
        private float convert(float valor, Variable.TipoDato dato)
        {
            switch(dato)
            {
                case (Variable.TipoDato.Char):
                    return valor % 256;
                case (Variable.TipoDato.Int):
                    return valor % 65535;
                case (Variable.TipoDato.Float):
                    return valor;
            }
            return valor;
        }
        //Programa -> Librerias? Variables? Main
        public void Programa()
        {
            Libreria();
            Variables();
            Main();
            displayVariables();
        }
        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Libreria()
        {
            if (getContenido() == "#")
            {
                match("#");
                match("include");
                match("<");
                match(Tipos.Identificador);
                if (getContenido() == ".")
                {
                    match(".");
                    match("h");
                }
                match(">");
                Libreria();
            }
        }
        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variable.TipoDato tipo = Variable.TipoDato.Char; 
                switch (getContenido())
                {
                    case "int": tipo = Variable.TipoDato.Int; break;
                    case "float": tipo = Variable.TipoDato.Float; break;
                }
                match(Tipos.TipoDato);
                Lista_identificadores(tipo);
                match(Tipos.FinSentencia);
                Variables();
            }
        }
         //Lista_identificadores -> identificador (,Lista_identificadores)?
        private void Lista_identificadores(Variable.TipoDato tipo)
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                if (!existeVariable(getContenido()))
                {
                    addVariable(getContenido(), tipo);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable duplicada <" +getContenido()+"> en linea: "+linea, log);
                }
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                Lista_identificadores(tipo);
            }
        }
        //Main      -> void main() Bloque de instrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(true);
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }    
            match("}"); 
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion);
            }
        }
        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion)
        {
            Instruccion(evaluacion);
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion);
            }
        }
        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion)
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion);
            }
            else if(getContenido() == "do")
            {
                Do(evaluacion);
            }
            else if(getContenido() == "for")
            {
                For(evaluacion);
            }
            else if(getContenido() == "switch")
            {
                Switch(evaluacion);
            }
            else
            {
                Asignacion(evaluacion);
            }
        }
        private Variable.TipoDato evaluaNumero(float resultado)
        {
            if(resultado % 1 != 0)
            {
                return Variable.TipoDato.Float;
            }
            else if(resultado <= 255)
            {
                return Variable.TipoDato.Char;
            }
            else if(resultado <= 65535)
            {
                return Variable.TipoDato.Int;
            }
            return Variable.TipoDato.Float;
        }
        private bool evaluaSemantica(string variable, float resultado)
        {
            Variable.TipoDato tipoDato = getTipo(variable);
            //sacar el tipo de dato de la variable
            return false;
        }
        //Asignacion -> identificador = cadena | Expresion;
        private void Asignacion(bool evaluacion)
        {
            if(existeVariable(getContenido()))
            {
                log.WriteLine();
                log.Write(getContenido()+" = ");
                string nombre = getContenido();
                match(Tipos.Identificador);
                dominante = Variable.TipoDato.Char;
                if (getClasificacion() == Tipos.IncrementoTermino || getClasificacion() == Tipos.IncrementoFactor)
                {
                    //Requerimiemto 3.1-B
                    //Requerimiemto 3.2-C
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion();
                    match(";");
                    float resultado = stack.Pop();
                    log.Write("= " + resultado);
                    log.WriteLine();
                    if (dominante < evaluaNumero(resultado))
                    {
                        dominante = evaluaNumero(resultado);
                    }
                    if (dominante <= getTipo(nombre))
                    {
                        if(evaluacion)
                        {
                            modVariable(nombre, resultado);
                        }
                    }
                    else
                    {
                        throw new Error("Error de semantica: no podemos asignar un: <" + dominante + "> a un <" + getTipo(nombre) + "> en linea  " + linea, log);
                    }
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable no Existe <" +getContenido()+"> en linea: "+linea, log);
            }
        }
        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion)
        {
            match("while");
            match("(");
            //Requerimiento 2.4
            /*-->*/bool validarWhile = Condicion();
            if(!evaluacion)
            {
                validarWhile = false;
            }
            //que dependiendo de la condicion se ejecute o no el bloque de instrucciones o la instruccion
            match(")");
            if(getContenido() == "{") 
            {
                BloqueInstrucciones(validarWhile);
            }
            else
            {
                Instruccion(validarWhile);
            }
        }
        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion)
        {
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion);
            }
            else
            {
                Instruccion(evaluacion);
            } 
            match("while");
            match("(");
            //Requerimiento 2.4 
            bool validarDo = Condicion();
            if(!evaluacion)
            {
                validarDo = false;
            }
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion)
        {
            match("for");
            match("(");
            Asignacion(evaluacion);
            //Requerimiento 2.6:
            //a) Necesito guardar la posicion del archivo para poder regresar a ella con la variable int
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarFor,incremento;
            //b) Metemos un ciclo do while despues del valida for 
            do
            {
                //Requerimiento 2.4
                validarFor = Condicion();
                if(!evaluacion)
                {
                    validarFor = false;
                }
                match(";");
                incremento = Incremento(validarFor);
                //Requerimiento 3.2-D
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor);  
                }
                else
                {
                    Instruccion(validarFor);
                }
                if(validarFor)
                {
                    if(incremento)
                    {
                        modVariable(variable, getValor(variable) + 1/*en vez de 1 puede ser "incremento"*/);
                        //Hacer que el incremento regrese un int y se modifique la variable con lo que regresa
                        //No hay necesidad de usar el if(incremento)
                    }
                    else
                    {
                        modVariable(variable, getValor(variable) - 1);
                    }
                    //c) regresar a la posicion del archivo
                    posicion = posisionInit-tamanio;
                    linea = lineaInit;
                    archivo.DiscardBufferedData();
                    archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
                    //d) sacar otro token
                    NextToken();
                }
            }while(validarFor);
        }
        //Incremento -> Identificador ++ | --
        private bool Incremento(bool evaluacion)
        {
            //string variable=getContenido();
            if(existeVariable(getContenido()))
            {
                string variable = getContenido();
                match(Tipos.Identificador);
                
                if(getContenido() == "++")
                {
                    match("++");
                    /*if(evaluacion)
                    {   
                        modVariable(variable,getValor(variable)+1);
                    }*/
                    return true;
                }
                else
                {
                    match("--");
                    /*if(evaluacion)
                    {
                        modVariable(variable,getValor(variable)-1);
                    }*/
                    return false;
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable no Existe <" +getContenido()+"> en linea: "+linea, log);
            }
        }
        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion)
        {
            match("switch");
            match("(");
            Expresion();
            stack.Pop();
            match(")");
            match("{");
            ListaDeCasos(evaluacion);
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion);  
                }
                else
                {
                    Instruccion(evaluacion);
                }
            }
            match("}");
        }
        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion)
        {
            match("case");
            Expresion();
            stack.Pop();
            match(":");
            ListaInstruccionesCase(evaluacion);
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos(evaluacion);
            }
        }
        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion()
        {
            Expresion();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            float e1 = stack.Pop();
            switch(operador)
            {
                case "==":
                    return e1==e2;
                case "<":
                    return e1<e2;
                case "<=":
                    return e1<=e2;
                case ">":
                    return e1>e2;
                case ">=":
                    return e1>=e2;
                default:
                    return e1!=e2;
            }
        }
        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion)
        {
            match("if");
            match("(");
            //Requerimiento 2.4
            bool validaIf = Condicion();
            if(!evaluacion)
            {
                validaIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validaIf);  
            }
            else
            {
                Instruccion(validaIf);
            }
            if (getContenido() == "else")
            {
                match("else");
                if(evaluacion == false){
                    validaIf = true;
                }
                //Requerimiento 2.4
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!validaIf);
                }
                else
                {
                    Instruccion(!validaIf);
                }
            }
        }
        //Printf -> printf(cadena|expresion);
        private void Printf(bool evaluacion)
        {
            match("printf");
            match("(");
            if(getClasificacion()==Tipos.Cadena)
            {
                if(evaluacion)
                {
                    string nombre=getContenido().Replace( "\\n","\n");
                    nombre=nombre.Replace( "\\t","\t");
                    Console.Write(nombre.Replace( "\"",""));
                }
                match(Tipos.Cadena);
            }
            else
            {
                Expresion();
                float resultado = stack.Pop();
                if(evaluacion)
                {
                    Console.Write(resultado);
                }
            }
            match(")");
            match(";");
        }
        //Scanf -> scanf(cadena, & Identificador);
        private void Scanf(bool evaluacion)    
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            if(existeVariable(getContenido()))
            {
                string nombre=getContenido();
                if(evaluacion)
                {
                    string val=""+Console.ReadLine();
                    //Requerimiento 2.5
                    //if(val!=texto){haz el float, si no haz la excepcion}
                    float y;
                    try{
                        y = float.Parse(val);
                    }
                    catch
                    {
                        throw new Error("Error de sintaxis, el valor no es un numero <" +val+"> en linea: "+linea, log);
                    }
                    float valorFloat=float.Parse(val);
                    modVariable(nombre,valorFloat);
                }
                match(Tipos.Identificador);
                match(")");
                match(";");
            }
            else
            {
                throw new Error("Error de sintaxis, variable no Existe <" +getContenido()+"> en linea: "+linea, log);
            }
        }
        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(operador + " ");
                float n1 = stack.Pop();
                float n2 = stack.Pop();
                //Requerimiento 3.1-A
                //Guardar el residuo en caso de ser %
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " " );
                if (dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if(existeVariable(getContenido()))
                {
                    log.Write(getContenido() + " " );
                    //Requerimiento 2.1: Actualizar el dominante para variables en la expresion
                    if(dominante < getTipo(getContenido()))
                    {
                        dominante = getTipo(getContenido());
                    }
                    //Fin requerimiento 2.1
                    stack.Push(getValor(getContenido()));
                    match(Tipos.Identificador);
                }
                else
                {
                    throw new Error("Error de sintaxis, variable no Existe <" +getContenido()+"> en linea: "+linea, log);
                }

            }
            else
            {
                bool huboCasteo = false;
                Variable.TipoDato casteo = Variable.TipoDato.Char;
                match("(");
                if(getClasificacion() == Tipos.TipoDato)
                {
                    huboCasteo = true;
                    switch(getContenido())
                    {
                        case "char":
                            casteo = Variable.TipoDato.Char;
                            break;
                        case "int":
                            casteo = Variable.TipoDato.Int;
                            break;
                        case "float":
                            casteo = Variable.TipoDato.Float;
                            break;

                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion();
                match(")");
                if(huboCasteo)
                {
                    //Requerimiento 2.2: Actualizar el dominante para el casteo y el valor de la subexpresion
                    dominante = casteo;
                    //saco un elemento del stack
                    float dato = stack.Pop();
                    //convierto ese valor al equivalente en casteo
                    stack.Push(convert(dato,casteo));
                    //Requerimiento 2.3
                    //Ejemplo: si el casteo es char y el pop regresa un 256
                    //         el valor equivalente en casteo es 0
                    //y meto ese valor al stack 
                }
            }
        }
    }
}