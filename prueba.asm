;Archivo: prueba.cpp
;Fecha: 04/11/2022 10:02:39 a. m.
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
MOV AX,10
PUSH AX
POP AX
MOV x, AX
MOV AX,152
PUSH AX
POP AX
MOV i, AX
MOV AX,3
PUSH AX
POP AX
DIV AX, i
MOV i, DX
POP AX
MOV AX,1
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
RET
DEFINE_SCAN_NUM
END
