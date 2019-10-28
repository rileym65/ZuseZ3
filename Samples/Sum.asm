var1:	equ	1	; Will use this for the memory position
	pr	0	; Read first memory cell into R1
	rep	9	; Will need to read and add 9 more cells
	pr	var1	; Read next memory cell into R2
	ls1		; and add to sum
	set	var1,var1+1	; Point to next memory cell
	endrep		; end of repeat block
	ld		; display the result

