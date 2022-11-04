;Archivo: prueba.cpp
;Fecha: 04/11/2022 09:13:55 a. m.
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
MOV AX,254
PUSH AX
POP AX
MOV i, AX
MOV AX,2
PUSH AX
POP AX
ADD AX, i
MOV i, AX
POP AX
MOV AX,10
PUSH AX
POP AX
POP BX
CMP AX,BX
JGE 
RET
DEFINE_SCAN_NUM
END
