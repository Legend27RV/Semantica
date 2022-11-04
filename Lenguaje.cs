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
//                       (X) A) Agregar el residuo (%) de la division en PorFactor
//                       (1/2 - falta hacer que el 1 sea variable) B) Agregar en Asignacion los incremetos de termino y factor
//                       a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1
//                       en donde el 1 puede ser cualquier expresion
//                       ( ) C) Programar el destructor para ejecutar el metodo cerrarArchivo (metodo cerrar de lexico)
//                       #libreria especiaL? contenedor?
//                       en la clase lexico
//( ) Requerimiento 3.2- Actualizacion la Venganza xd:
//                       (X) C) Marcar errores semanticos cuando los incrementos de termino o factor superen el rango de la variable(char, int, float)
//                       (1/2 - falta hacer que el 1 sea variable) D) Considerar el inciso 3.1-B y 3.2-C para el for
//                       (X) E) Hacer que funcione el do-while y el while
//( ) Requerimiento 3.3- Agregar:
//                       ( ) A) Considerar las variables y los casteos de las expresiones matematicas en ensamblador (stack.push)
//                       (X) B) Considerar el residuo de la division en ensamblador
//                       (X) C) Programar el printf y scanf en ensamblador
//( ) Requerimiento 3.4- 
//                       ( ) A) Programar el else en ensamblador
//                       ( ) B) Programar el for en ensamblador
//( ) Requerimiento 3.5-
//                       ( ) A) Programar el while en ensamblador
//                       ( ) B) Programar el do-while en ensamblador

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();
        Variable.TipoDato dominante;
        int cIf;
        int cFor;
        public Lenguaje()
        {
            cIf = cFor = 0;
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            cIf = cFor = 0;
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
        private void variablesAsm()
        {
            asm.WriteLine(";Variables: ");
            foreach (Variable v in variables)
            {
                //Hace un switch para poner a cada variable por su tipo de dato (char(1 byte), int(2 bytes), float(4 bytes))
                asm.WriteLine("\t"+v.getNombre()+" DW ?");
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
            asm.WriteLine("#make_COM#");
            asm.WriteLine("include 'emu8086.inc'");
            asm.WriteLine("ORG 100h");
            Libreria();
            Variables();
            variablesAsm();
            Main();
            displayVariables();
            asm.WriteLine("RET");
            asm.WriteLine("DEFINE_SCAN_NUM");
            asm.WriteLine("END");
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
                    bool posible = true;
                    string aux = getContenido();
                    //Requerimiemto 3.1-B
                    switch(getTipo(nombre))
                    {
                        case Variable.TipoDato.Char:
                            if(getValor(nombre)>=255)
                            {
                                posible=false;    
                            }
                            break;
                        case Variable.TipoDato.Int:
                            if(getValor(nombre)>=65535)
                            {
                                posible=false;    
                            }
                            break;
                        case Variable.TipoDato.Float:
                            break;                    
                    }
                    if(aux=="++"||aux=="--")
                    {
                        switch(aux)
                        {
                            case "++":
                                match("++");
                                if(posible)
                                {
                                    asm.WriteLine("INC " + nombre);
                                    modVariable(nombre, getValor(nombre) + 1);
                                }
                                else
                                {
                                    throw new Error("No se puede aumentar la variable <" +nombre+ "> en linea: " + linea, log); 
                                }
                                break;
                            case "--":
                                match("--");
                                asm.WriteLine("DEC " + nombre);
                                modVariable(nombre, getValor(nombre) - 1);
                                break;
                        }
                        match(";");
                    }
                    else
                    {
                        NextToken();
                        Factor();
                        float resultado = stack.Pop();
                        asm.WriteLine("POP AX");
                        //Requerimiemto 3.2-C
                        switch(aux){
                            case "+=":
                                if(posible)
                                {
                                    asm.WriteLine("ADD AX, " + nombre);
                                    asm.WriteLine("MOV " + nombre + ", AX");
                                    modVariable(nombre, getValor(nombre) + resultado);
                                }
                                else
                                {
                                    throw new Error("No se puede aumentar la variable <" +nombre+ "> en linea: " + linea, log); 
                                }
                                break;
                            case "*=":
                                if(posible)
                                {
                                    asm.WriteLine("MUL AX, " + nombre);
                                    asm.WriteLine("MOV " + nombre + ", AX");
                                    modVariable(nombre, getValor(nombre) * resultado);
                                }
                                else
                                {
                                    throw new Error("No se puede aumentar la variable <" +nombre+ "> en linea: " + linea, log); 
                                }
                                break;
                            case "-=":
                                asm.WriteLine("SUB AX, " + nombre);
                                asm.WriteLine("MOV " + nombre + ", AX");
                                modVariable(nombre, getValor(nombre) - resultado);
                                break;
                            case "/=":
                                asm.WriteLine("DIV AX, " + nombre);
                                asm.WriteLine("MOV " + nombre + ", AX");
                                modVariable(nombre, getValor(nombre) / resultado);
                                break;
                            case "%=":
                                asm.WriteLine("DIV AX, " + nombre);
                                asm.WriteLine("MOV " + nombre + ", DX");
                                modVariable(nombre, getValor(nombre) % resultado);
                                break;
                            default: 
                                throw new Error("Error de semantica, no se conoce el operador <" +aux+ "> en linea: " + linea, log);
                        }
                        match(";");
                    }
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion();
                    match(";");
                    float resultado = stack.Pop();
                    asm.WriteLine("POP AX");
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
                    //Modifica la variable en ensamblador (Buscar como modificar/crear variables en ensamblador)
                    asm.WriteLine("MOV " + nombre + ", AX");
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
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarWhile;
            do
            {
                match("(");
                validarWhile = Condicion("");
                if(!evaluacion)
                {
                    validarWhile = false;
                }
                match(")");
                if(getContenido() == "{") 
                {
                    BloqueInstrucciones(validarWhile);
                }
                else
                {
                    Instruccion(validarWhile);
                }
                if (validarWhile)
                {
                    posicion = posisionInit;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
            }while(validarWhile);
                
        }
        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion)
        {
            match("do");
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarDo;
            do{
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
                validarDo = Condicion("");
                if(!evaluacion)
                {
                    validarDo = false;
                }
                match(")");
                match(";");
                if(validarDo)
                {
                    posicion = posisionInit-tamanio+1;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
            }while(validarDo);
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion)
        {
            string etiquetaInicioFor = "Iniciofor" + cFor;
            string etiquetaFinFor = "Finfor" + cFor++;
            asm.WriteLine(etiquetaInicioFor + ":");
            match("for");
            match("(");
            Asignacion(evaluacion);
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarFor;
            float incremento;
            do
            {
                validarFor = Condicion("");
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
                    modVariable(variable, incremento);
                    posicion = posisionInit-tamanio;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
            }while(validarFor);
            asm.WriteLine(etiquetaFinFor+":");
        }
        private void setPosicion(int posicion){
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }
        //Incremento -> Identificador ++ | --
        private float Incremento(bool evaluacion/*,valor de la varialble a (b+=a)*/)
        {
            //string variable=getContenido();
            if(existeVariable(getContenido()))
            {
                string variable = getContenido();
                match(Tipos.Identificador);
                switch(getContenido())
                {
                    case "--":
                        match("--");
                        return -1;
                    case "-=":
                        match("-=");
                        return getValor(variable)-getValor(getContenido());
                    case "/=":
                        match("/=");
                        return getValor(variable)/getValor(getContenido()); 
                    case "%=":
                        match("%=");
                        return getValor(variable)%getValor(getContenido());
                }
                switch(getTipo(variable))
                {
                    case Variable.TipoDato.Char:
                        if(getValor(variable)>=255){
                            throw new Error("Error de semantica, la variable <" + variable + "> tipo <" + getTipo(variable) + "> sobrepasa el limite de 255 en linea: " + linea, log);
                        }
                        break;
                    case Variable.TipoDato.Int:
                        if(getValor(variable)>=65535){
                            throw new Error("Error de semantica, la variable <" + variable + "> tipo <" + getTipo(variable) + "> sobrepasa el limite de 65535 en linea: " + linea, log);
                        }
                        break;
                    case Variable.TipoDato.Float:
                        break;                    
                }
                switch(getContenido())
                {
                    case "++":
                        match("++");
                        asm.WriteLine("INC " + variable);
                        return 1; 
                    case "+=":
                        match("+=");
                        return getValor(variable)+getValor(getContenido()); 
                    case "*=":
                        match("*=");
                        return getValor(variable)*getValor(getContenido());
                    default: 
                        throw new Error("Error de sintaxis, no se reconoce el operador <" + getContenido() + "> en linea: " + linea, log);
                 
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
            asm.WriteLine("POP AX");
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
            asm.WriteLine("POP AX");
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
        private bool Condicion(string etiqueta)
        {
            Expresion();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float e2 = stack.Pop();
            asm.WriteLine("POP AX");
            float e1 = stack.Pop();
            asm.WriteLine("POP BX");
            asm.WriteLine("CMP AX,BX");
            switch(operador)
            {
                case "==":
                    asm.WriteLine("JNE "+etiqueta);
                    return e1==e2;
                case "<":
                    asm.WriteLine("JGE "+etiqueta);
                    return e1<e2;
                case "<=":
                    asm.WriteLine("JG "+etiqueta);
                    return e1<=e2;
                case ">":
                    asm.WriteLine("JLE "+etiqueta);
                    return e1>e2;
                case ">=":
                    asm.WriteLine("JL "+etiqueta);
                    return e1>=e2;
                default:
                    asm.WriteLine("JE "+etiqueta);
                    return e1!=e2;
            }
        }
        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion)
        {
            string etiquetaIf = "if" + ++cIf;
            match("if");
            match("(");
            bool validaIf = Condicion(etiquetaIf);
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
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!validaIf);
                }
                else
                {
                    Instruccion(!validaIf);
                }
            }
            asm.WriteLine(etiquetaIf+":");
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
                    /*
                    setContenido(getContenido().Replace("\"",""));
                    setContenido(getContenido().Replace( "\\n","\n"));
                    setContenido(getContenido().Replace( "\\t","\t"));
                    Console.Write(getContenido());
                    */
                    string nombre=getContenido().Replace( "\\n","\n");
                    nombre=nombre.Replace( "\\t","\t");
                    Console.Write(getContenido().Replace("\"",""));
                }
                asm.WriteLine("PRINTN "+getContenido()+"");
                match(Tipos.Cadena);
            }
            else
            {
                Expresion();
                float resultado = stack.Pop();
                asm.WriteLine("POP AX");
                if(evaluacion)
                {
                    Console.Write(resultado);
                }
                //Requerimiento: imprimir el PRINTN para numeros con PRINT_NUM
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
                    float y;
                    try{
                        y = float.Parse(val);
                    }
                    catch
                    {
                        throw new Error("Error de sintaxis, el valor no es un numero <" +val+"> en linea: "+linea, log);
                    }
                    asm.WriteLine("CALL SCAN_NUM");
                    asm.WriteLine("MOV "+getContenido()+",CX");
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
                asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                asm.WriteLine("POP AX");
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        asm.WriteLine("ADD AX,BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        asm.WriteLine("SUB AX,BX");
                        asm.WriteLine("PUSH AX");
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
                asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                asm.WriteLine("POP AX");
                //Requerimiento 3.1-A
                //Guardar el residuo en caso de ser %
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        asm.WriteLine("MUL BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH AX");
                        break;
                    //Requerimiento 3.3-B
                    case "%":
                        stack.Push(n2 % n1);
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH DX");
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
                asm.WriteLine("MOV AX," + getContenido());
                asm.WriteLine("PUSH AX");
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if(existeVariable(getContenido()))
                {
                    log.Write(getContenido() + " " );
                    if(dominante < getTipo(getContenido()))
                    {
                        dominante = getTipo(getContenido());
                    }
                    stack.Push(getValor(getContenido()));
                    //Requerimiento 3.3-A
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
                    dominante = casteo;
                    float dato = stack.Pop();
                    asm.WriteLine("POP AX");
                    stack.Push(convert(dato,casteo));
                }
            }
        }
    }
}