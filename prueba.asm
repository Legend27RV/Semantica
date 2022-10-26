;Archivo: prueba.cpp
;Fecha: 26/10/2022 10:23:03 a. m.
#make_COM#
include 'emu8086.inc'
ORG 100h
;Variables: 
	area DW ?
	radio DW ?
	pi DW ?
	resultado DW ?
	a DW ?
	d DW ?
	altura DW ?
	x DW ?
	y DW ?
	i DW ?
	j DW ?
	k DW ?
	l DW ?
MOV AX,1
PUSH AX
MOV AX,1
PUSH AX
POP AX
POP BX
CMP AX,BX
JNE if1
MOV AX,10
PUSH AX
POP AX
MOV x, AX
Iniciofor0:
MOV AX,0
PUSH AX
POP AX
MOV i, AX
POP AX
POP BX
CMP AX,BX
JGE 
Iniciofor1:
MOV AX,0
PUSH AX
POP AX
MOV j, AX
POP AX
POP BX
CMP AX,BX
JGE 
Iniciofor2:
MOV AX,0
PUSH AX
POP AX
MOV k, AX
POP AX
POP BX
CMP AX,BX
JGE 
Iniciofor3:
MOV AX,0
PUSH AX
POP AX
MOV l, AX
POP AX
POP BX
CMP AX,BX
JGE 
