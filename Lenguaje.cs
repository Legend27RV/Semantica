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

//(X) Requerimiento 3.1- Actualizacion: 
//                       (X) A) Agregar el residuo (%) de la division en PorFactor
//                       (X) B) Agregar en Asignacion los incremetos de termino y factor
//                       a++, a--, a+=1, a-=1, a*=1, a/=1, a%=1
//                       en donde el 1 puede ser cualquier expresion
//                       (X) C) Programar el destructor para ejecutar el metodo cerrarArchivo (metodo cerrar de lexico)
//                       #libreria especiaL? contenedor?
//                       en la clase lexico
//(X) Requerimiento 3.2- Actualizacion la Venganza xd:
//                       (X) C) Marcar errores semanticos cuando los incrementos de termino o factor superen el rango de la variable(char, int, float)
//                       (X) D) Considerar el inciso 3.1-B y 3.2-C para el for
//                       (X) E) Hacer que funcione el do-while y el while
//(2/3) Requerimiento 3.3- Agregar:
//                       ( ) A) Considerar las variables y los casteos de las expresiones matematicas en ensamblador (stack.push)
//                       (X) B) Considerar el residuo de la division en ensamblador
//                       (X) C) Programar el printf y scanf en ensamblador
//( ) Requerimiento 3.4- 
//                       (X) A) Programar el else en ensamblador
//                       (X) B) Programar el for en ensamblador
//( ) Requerimiento 3.5-
//                       (X) A) Programar el while en ensamblador
//                       (X) B) Programar el do-while en ensamblador

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List <Variable> variables = new List<Variable>();
        Stack<float> stack = new Stack<float>();
        Variable.TipoDato dominante;
        int cIf;
        int cFor;
        int cWhile;
        int cDo;
        public Lenguaje()
        {
            cIf = cFor = cWhile = cDo = 0;
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
                //asm.WriteLine("\t"+v.getNombre()+" DW ?");
                switch (v.getTipo())
                {
                    case Variable.TipoDato.Char:
                        asm.WriteLine("\t"+v.getNombre()+" DB "+v.getValor());
                        break;
                    case Variable.TipoDato.Int:
                        asm.WriteLine("\t"+v.getNombre()+" DW "+v.getValor());
                        break;
                    case Variable.TipoDato.Float:
                        asm.WriteLine("\t"+v.getNombre()+" DD "+v.getValor());
                        break;
                }
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
            asm.WriteLine("DEFINE_PRINT_NUM");
            asm.WriteLine("DEFINE_PRINT_NUM_UNS");
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
            BloqueInstrucciones(true,true);
        }
        //Bloque de instrucciones -> {listaIntrucciones?}
        private void BloqueInstrucciones(bool evaluacion,bool impresion)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion,impresion);
            }    
            match("}"); 
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool evaluacion,bool impresion)
        {
            Instruccion(evaluacion,impresion);
            if (getContenido() != "}")
            {
                ListaInstrucciones(evaluacion,impresion);
            }
        }
        //ListaInstruccionesCase -> Instruccion ListaInstruccionesCase?
        private void ListaInstruccionesCase(bool evaluacion,bool impresion)
        {
            Instruccion(evaluacion,impresion);
            if (getContenido() != "case" && getContenido() !=  "break" && getContenido() != "default" && getContenido() != "}")
            {
                ListaInstruccionesCase(evaluacion,impresion);
            }
        }
        //Instruccion -> Printf | Scanf | If | While | do while | For | Switch | Asignacion
        private void Instruccion(bool evaluacion, bool impresion)
        {
            if (getContenido() == "printf")
            {
                Printf(evaluacion,impresion);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(evaluacion,impresion);
            }
            else if (getContenido() == "if")
            {
                If(evaluacion,impresion);
            }
            else if (getContenido() == "while")
            {
                While(evaluacion,impresion);
            }
            else if(getContenido() == "do")
            {
                Do(evaluacion,impresion);
            }
            else if(getContenido() == "for")
            {
                For(evaluacion,impresion);
            }
            else if(getContenido() == "switch")
            {
                Switch(evaluacion,impresion);
            }
            else
            {
                Asignacion(evaluacion,impresion);
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
        private void Asignacion(bool evaluacion,bool impresion)
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
                    float incremento;
                    incremento = Incremento(evaluacion,nombre,impresion);
                    modVariable(nombre,incremento);
                    match(";");
                }
                else
                {
                    match(Tipos.Asignacion);
                    Expresion(impresion);
                    match(";");
                    float resultado = stack.Pop();
                    if(impresion)
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
                    if(impresion)
                        asm.WriteLine("MOV " + nombre + ", AX;juejue");
                }
            }
            else
            {
                throw new Error("Error de sintaxis, variable no Existe <" +getContenido()+"> en linea: "+linea, log);
            }
        }
        //While -> while(Condicion) bloque de instrucciones | instruccion
        private void While(bool evaluacion,bool impresion)
        {
            string etiquetaInicioWhile = "Iniciowhile" + cWhile;
            string etiquetaFinWhile = "Finwhile" + cWhile++;
            if(impresion)
                asm.WriteLine(etiquetaInicioWhile + ":");
            match("while");
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarWhile;
            do
            {
                match("(");
                validarWhile = Condicion(etiquetaFinWhile,impresion);
                if(!evaluacion)
                {
                    validarWhile = false;
                }
                match(")");
                if(getContenido() == "{") 
                {
                    BloqueInstrucciones(validarWhile,impresion);
                }
                else
                {
                    Instruccion(validarWhile,impresion);
                }
                if (validarWhile)
                {
                    posicion = posisionInit-tamanio;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
                if(impresion){
                    asm.WriteLine("JMP " + etiquetaInicioWhile);
                    asm.WriteLine(etiquetaFinWhile+":");
                }
                impresion = false;
            }while(validarWhile);
            impresion = true;
        }
        //Do -> do bloque de instrucciones | intruccion while(Condicion)
        private void Do(bool evaluacion,bool impresion)
        {
            string etiquetaInicioDo = "inicioDo" + cDo;
            string etiquetaFinDo = "finDo" + cDo++;
            if(impresion)
                asm.WriteLine(etiquetaInicioDo+":");
            match("do");
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarDo;
            do{
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion,impresion);
                }
                else
                {
                    Instruccion(evaluacion,impresion);
                }
                match("while");
                match("(");
                validarDo = Condicion(etiquetaFinDo,impresion);
                if(!evaluacion)
                {
                    validarDo = false;
                }
                match(")");
                match(";");
                if(validarDo)
                {
                    posicion = posisionInit-tamanio;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
                if(impresion){
                    asm.WriteLine("JMP " + etiquetaInicioDo);
                    asm.WriteLine(etiquetaFinDo+":");
                }
                impresion = false;
            }while(validarDo);
            impresion = true;
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Intruccion 
        private void For(bool evaluacion,bool impresion)
        {
            string etiquetaInicioFor = "Iniciofor" + cFor;
            string etiquetaFinFor = "Finfor" + cFor++;
            match("for");
            match("(");
            Asignacion(evaluacion,impresion);
            if(impresion)
                asm.WriteLine(etiquetaInicioFor + ":");
            int posisionInit = posicion;
            int lineaInit = linea;
            int tamanio = getContenido().Length;
            string variable = getContenido();
            bool validarFor;
            float incremento;
            do
            {
                validarFor = Condicion(etiquetaFinFor,impresion);
                if(!evaluacion)
                {
                    validarFor = false;
                }
                match(";");
                match(Tipos.Identificador);
                string aux = getContenido();
                incremento = Incremento(validarFor,variable,false);
                float resultado = stack.Pop();
                //Requerimiento 3.2-D
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(validarFor,impresion);  
                }
                else
                {
                    Instruccion(validarFor,impresion);
                }
                if(validarFor)
                {
                    if(impresion){
                        switch(aux){
                            case "++":
                                asm.WriteLine("INC " + variable);
                                break;
                            case "--":
                                asm.WriteLine("DEC " + variable);
                                break;
                            case "+=":
                                asm.WriteLine("ADD " + variable + ", " + resultado);
                                break;
                            case "-=":
                                asm.WriteLine("SUB " + variable + ", " + resultado);
                                break;
                            case "*=":
                                asm.WriteLine("MUL " + variable + ", " + resultado);
                                break;
                            case "/=":
                                asm.WriteLine("DIV " + variable + ", " + resultado);
                                break;
                            case "%=":
                                asm.WriteLine("MOD " + variable + ", " + resultado);
                                break;
                        }
                    }
                    modVariable(variable, incremento);
                    posicion = posisionInit-tamanio+1;
                    linea = lineaInit;
                    setPosicion(posicion);
                    NextToken();
                }
                if(impresion){
                    asm.WriteLine("JMP " +etiquetaInicioFor);
                    asm.WriteLine(etiquetaFinFor+":");
                }
                impresion = false;
            }while(validarFor);
            impresion = true;
        }
        private void setPosicion(int posicion){
            archivo.DiscardBufferedData();
            archivo.BaseStream.Seek(posicion, SeekOrigin.Begin);
        }
        //Incremento -> Identificador ++ | --
        private float Incremento(bool evaluacion, string nombre,bool impresion)
        {
            bool posible = true;
            string aux = getContenido();
            //Requerimiemto 3.1-B
            switch(getTipo(nombre))
            {
                case Variable.TipoDato.Char:
                    if(getValor(nombre)==255)
                    {
                        posible=false;
                    }
                    break;
                case Variable.TipoDato.Int:
                    if(getValor(nombre)==65535)
                    {
                        posible=false;    
                    }
                    break;
                case Variable.TipoDato.Float:
                    break;                    
            }
            switch(aux)
            {
                case "++":
                    match("++");
                    if(posible)
                    {
                        if(impresion)
                            asm.WriteLine("INC " + nombre);
                        return getValor(nombre) + 1;
                    }
                    else
                    {
                        throw new Error("No se puede aumentar la variable <" +nombre+ "> de tipo <"+getTipo(nombre)+"> en linea: " + linea, log); 
                    }
                case "--":
                    match("--");
                    if(impresion)
                        asm.WriteLine("DEC " + nombre);
                    return getValor(nombre) - 1;
            }
            NextToken();
            Factor(impresion);
            float resultado = stack.Pop();
            stack.Push(resultado);
            if(impresion)
                asm.WriteLine("POP AX");
            //Requerimiemto 3.2-C
            switch(getTipo(nombre))
            {
                case Variable.TipoDato.Char:
                    if(getValor(nombre)+resultado>255||getValor(nombre)*resultado>255)
                    {
                        posible=false;    
                    }
                    break;
                case Variable.TipoDato.Int:
                    if(getValor(nombre)+resultado>65535||getValor(nombre)*resultado>65535)
                    {
                        posible=false;    
                    }
                    break;
                case Variable.TipoDato.Float:
                    break;                    
            }
            switch(aux){
                case "+=":
                    if(posible)
                    {
                        if(impresion){
                            asm.WriteLine("ADD AX, " + nombre);
                            asm.WriteLine("MOV " + nombre + ", AX");
                        }
                        return getValor(nombre) + resultado;
                    }
                    else
                    {
                        throw new Error("No se puede aumentar la variable <" +nombre+ "> de tipo <"+getTipo(nombre)+"> en linea: " + linea, log); 
                    }
                case "*=":
                    if(posible)
                    {
                        if(impresion){
                            asm.WriteLine("MUL AX, " + nombre);
                            asm.WriteLine("MOV " + nombre + ", AX");
                        }
                        return getValor(nombre) * resultado;
                    }
                    else
                    {
                        throw new Error("No se puede aumentar la variable <" +nombre+ "> de tipo <"+getTipo(nombre)+"> en linea: " + linea, log); 
                    }
                case "-=":
                    if(impresion){
                        asm.WriteLine("SUB AX, " + nombre);
                        asm.WriteLine("MOV " + nombre + ", AX");
                    }
                    return getValor(nombre) - resultado;
                case "/=":
                    if(impresion){
                        asm.WriteLine("DIV AX, " + nombre);
                        asm.WriteLine("MOV " + nombre + ", AX");
                    }
                    return getValor(nombre) / resultado;
                case "%=":
                    if(impresion){
                        asm.WriteLine("DIV AX, " + nombre);
                        asm.WriteLine("MOV " + nombre + ", DX");
                    }
                    return getValor(nombre) % resultado;
                default: 
                    throw new Error("Error de semantica, no se conoce el operador <" +aux+ "> en linea: " + linea, log);
            }
        }
        //Switch -> switch (Expresion) {Lista de casos} | (default: )
        private void Switch(bool evaluacion,bool impresion)
        {
            match("switch");
            match("(");
            Expresion(impresion);
            stack.Pop();
            if(impresion)
                asm.WriteLine("POP AX");
            match(")");
            match("{");
            ListaDeCasos(evaluacion,impresion);
            if(getContenido() == "default")
            {
                match("default");
                match(":");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(evaluacion,impresion);  
                }
                else
                {
                    Instruccion(evaluacion,impresion);
                }
            }
            match("}");
        }
        //ListaDeCasos -> case Expresion: listaInstruccionesCase (break;)? (ListaDeCasos)?
        private void ListaDeCasos(bool evaluacion,bool impresion)
        {
            match("case");
            Expresion(impresion);
            stack.Pop();
            if(impresion)
                asm.WriteLine("POP AX");
            match(":");
            ListaInstruccionesCase(evaluacion,impresion);
            if(getContenido() == "break")
            {
                match("break");
                match(";");
            }
            if(getContenido() == "case")
            {
                ListaDeCasos(evaluacion,impresion);
            }
        }
        //Condicion -> Expresion operador relacional Expresion
        private bool Condicion(string etiqueta,bool impresion)
        {
            Expresion(impresion);
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion(impresion);
            float e2 = stack.Pop();
            if(impresion)
                asm.WriteLine("POP BX");
            float e1 = stack.Pop();
            if(impresion){
                asm.WriteLine("POP AX");
                asm.WriteLine("CMP AX,BX");
            }
            switch(operador)
            {
                case "==":
                    if(impresion)
                        asm.WriteLine("JNE "+etiqueta);
                    return e1==e2;
                case "<":
                    if(impresion)
                        asm.WriteLine("JGE "+etiqueta);
                    return e1<e2;
                case "<=":
                    if(impresion)
                        asm.WriteLine("JG "+etiqueta);
                    return e1<=e2;
                case ">":
                    if(impresion)
                        asm.WriteLine("JLE "+etiqueta);
                    return e1>e2;
                case ">=":
                    if(impresion)
                        asm.WriteLine("JL "+etiqueta);
                    return e1>=e2;
                default:
                    if(impresion)
                        asm.WriteLine("JE "+etiqueta);
                    return e1!=e2;
            }
        }
        //If -> if(Condicion) bloque de instrucciones (else bloque de instrucciones)?
        private void If(bool evaluacion,bool impresion)
        {
            string etiquetaIf = "if" + ++cIf;
            string etiquetaElse = "else" + ++cIf;
            match("if");
            match("(");
            bool validaIf = Condicion(etiquetaIf,impresion);
            if(!evaluacion)
            {
                validaIf = false;
            }
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(validaIf,impresion);  
            }
            else
            {
                Instruccion(validaIf,impresion);
            }
            if (getContenido() == "else")
            {
                if(impresion){
                    asm.WriteLine("JMP " + etiquetaElse);
                    cIf++;
                    asm.WriteLine(etiquetaIf + ":");
                }
                match("else");
                if(evaluacion == false){
                    validaIf = true;
                }
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!validaIf,impresion);
                }
                else
                {
                    Instruccion(!validaIf,impresion);
                }
                if(impresion)
                    asm.WriteLine(etiquetaElse + ":");
            }else{
                if(impresion){
                    cIf++;
                    asm.WriteLine(etiquetaIf + ":");
                }
            }
        }
        //Printf -> printf(cadena|expresion);
        private void Printf(bool evaluacion,bool impresion)
        {
            match("printf");
            match("(");
            if(getClasificacion()==Tipos.Cadena)
            {
                string nombre=getContenido();
                char [] cadenas = new char[nombre.Length];
                cadenas = nombre.ToCharArray();
                if(nombre=="\"\\n\""){
                    nombre=nombre.Replace( "\\t","\t");
                    nombre=nombre.Replace( "\\n","");
                    nombre=nombre.Replace( "\\'","");
                    if(impresion)
                        asm.WriteLine("PRINTN "+nombre+"");
                }else{
                    for(int i=0;i<nombre.Length;i++){
                        if(cadenas[i]=='\\'){
                            if(cadenas[i+1]=='n'){
                            if(impresion)
                                asm.WriteLine("PRINTN \"\"");
                            }   
                        }
                    }
                    nombre=nombre.Replace( "\\t","\t");
                    nombre=nombre.Replace( "\\n","");
                    nombre=nombre.Replace( "\\'","");
                    if(impresion)
                        asm.WriteLine("PRINT "+nombre+"");
                }
                if(evaluacion)
                {
                    setContenido(getContenido().Replace("\"",""));
                    setContenido(getContenido().Replace( "\\n","\n"));
                    setContenido(getContenido().Replace( "\\t","\t"));
                    Console.Write(getContenido());
                }
                //if(impresion)
                    //asm.WriteLine("PRINTN "+nombre+"");
                match(Tipos.Cadena);
            }
            else
            {
                Expresion(impresion);
                float resultado = stack.Pop();
                if(impresion)
                    asm.WriteLine("POP AX");
                if(evaluacion)
                {
                    if(impresion)
                        asm.WriteLine("CALL PRINT_NUM");
                    Console.Write(resultado);
                }
                //Requerimiento: imprimir el PRINTN para numeros con PRINT_NUM
            }
            match(")");
            match(";");
        }
        //Scanf -> scanf(cadena, & Identificador);
        private void Scanf(bool evaluacion,bool impresion)    
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
                    if(impresion){
                        asm.WriteLine("CALL SCAN_NUM");
                        asm.WriteLine("MOV "+getContenido()+",CX");
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
        private void Expresion(bool impresion)
        {
            Termino(impresion);
            MasTermino(impresion);
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino(bool impresion)
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino(impresion);
                log.Write(operador + " ");
                float n1 = stack.Pop();
                if(impresion)
                    asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                if(impresion)
                    asm.WriteLine("POP AX");
                switch (operador)
                {
                    case "+":
                        stack.Push(n2 + n1);
                        if(impresion){
                            asm.WriteLine("ADD AX,BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "-":
                        stack.Push(n2 - n1);
                        if(impresion){
                            asm.WriteLine("SUB AX,BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino(bool impresion)
        {
            Factor(impresion);
            PorFactor(impresion);
        }
        //PorFactor -> (OperadorFactor Factor)? 
        private void PorFactor(bool impresion)
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor(impresion);
                log.Write(operador + " ");
                float n1 = stack.Pop();
                if(impresion)
                    asm.WriteLine("POP BX");
                float n2 = stack.Pop();
                if(impresion)
                    asm.WriteLine("POP AX");
                //Requerimiento 3.1-A
                //Guardar el residuo en caso de ser %
                switch (operador)
                {
                    case "*":
                        stack.Push(n2 * n1);
                        if(impresion){
                            asm.WriteLine("MUL BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    case "/":
                        stack.Push(n2 / n1);
                        if(impresion){
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH AX");
                        }
                        break;
                    //Requerimiento 3.3-B
                    case "%":
                        stack.Push(n2 % n1);
                        if(impresion){
                            asm.WriteLine("DIV BX");
                            asm.WriteLine("PUSH DX");
                        }
                        break;
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor(bool impresion)
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(getContenido() + " " );
                if (dominante < evaluaNumero(float.Parse(getContenido())))
                {
                    dominante = evaluaNumero(float.Parse(getContenido()));
                }
                stack.Push(float.Parse(getContenido()));
                if(impresion){
                    asm.WriteLine("MOV AX," + getContenido());
                    asm.WriteLine("PUSH AX");
                }
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
                    if(impresion){
                        asm.WriteLine("MOV AX," + getContenido()+";ja");
                        asm.WriteLine("PUSH AX");
                    }
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
                string variable = getContenido();
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
                Expresion(impresion);
                match(")");
                if(huboCasteo)
                {
                    dominante = casteo;
                    float dato = stack.Pop();
                    if(impresion)
                        asm.WriteLine("POP AX");
                    dato = convert(dato,casteo);
                    stack.Push(dato);
                    if(impresion){
                        asm.WriteLine("MOV AX," + dato);
                        asm.WriteLine("PUSH AX");
                    }
                }
            }
        }
    }
}