;Archivo: prueba.cpp
;Fecha: 09/11/2022 08:22:38 a. m.
#make_COM#
include 'emu8086.inc'
ORG 100h
;Variables: 
	area DD 0
	radio DD 0
	pi DD 0
	resultado DD 0
	a DW 0
	d DW 0
	altura DW 0
	cinco DW 0
	x DD 0
	y DB 0
	i DW 0
	j DW 0
	k DW 0
PRINT "Introduce la altura de la piramide: "
CALL SCAN_NUM
MOV altura,CX
MOV AX,altura
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
CMP AX,BX
JLE if1
MOV AX,altura
PUSH AX
POP AX
MOV i, AX
Iniciofor0:
MOV AX,i
PUSH AX
MOV AX,0
PUSH AX
POP BX
POP AX
CMP AX,BX
JLE Finfor0
MOV AX,0
PUSH AX
POP AX
MOV j, AX
Iniciowhile0:
MOV AX,j
PUSH AX
MOV AX,altura
PUSH AX
MOV AX,i
PUSH AX
POP BX
POP AX
SUB AX,BX
PUSH AX
POP BX
POP AX
CMP AX,BX
JGE Finwhile0
MOV AX,j
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
DIV BX
PUSH DX
MOV AX,0
PUSH AX
POP BX
POP AX
CMP AX,BX
JNE if3
PRINT "*"
JMP else4
if3:
PRINT "-"
else4:
MOV AX,1
PUSH AX
POP AX
ADD AX, j
MOV j, AX
JMP Iniciowhile0
Finwhile0:
PRINTN ""
SUB i, 1
JMP Iniciofor0
Finfor0:
MOV AX,0
PUSH AX
POP AX
MOV k, AX
inicioDo0:
PRINT "-"
MOV AX,2
PUSH AX
POP AX
ADD AX, k
MOV k, AX
MOV AX,k
PUSH AX
MOV AX,altura
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
MUL BX
PUSH AX
POP BX
POP AX
CMP AX,BX
JGE finDo0
JMP inicioDo0
finDo0:
PRINTN ""
JMP else2
if1:
PRINTN ""
PRINTN ""
PRINT "Error: la altura debe de ser mayor que 2"
else2:
MOV AX,1
PUSH AX
MOV AX,1
PUSH AX
POP BX
POP AX
CMP AX,BX
JE if37
PRINT "Esto no se debe imprimir"
MOV AX,2
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
CMP AX,BX
JNE if39
PRINT "Esto tampoco"
if39:
if37:
MOV AX,258
PUSH AX
POP AX
MOV a, AX
PRINT "Valor de variable int a antes del casteo: "
MOV AX,a
PUSH AX
POP AX
CALL PRINT_NUM
MOV AX,a
PUSH AX
POP AX
MOV AX,2;valo
PUSH AX
POP AX
MOV y, AX
PRINTN ""
PRINT "Valor de variable char y despues del casteo de a: "
MOV AX,y
PUSH AX
POP AX
CALL PRINT_NUM
PRINTN ""
PRINTN ""
PRINT "A continuacion se intenta asignar un int a un char sin usar casteo: "
RET
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
DEFINE_SCAN_NUM
END
