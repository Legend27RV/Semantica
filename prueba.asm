;Archivo: prueba.cpp
;Fecha: 28/10/2022 09:36:45 a. m.
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
PRINTN "Introduzca el radio del cilindro: "
CALL SCAN_NUM
MOV radio,CX
RET
DEFINE_SCAN_NUM
END
