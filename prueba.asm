;Archivo: prueba.cpp
;Fecha: 01/11/2022 09:53:29 a. m.
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
POP AX
MOV i, AX
INC i
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
