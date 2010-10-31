all:
	echo "Building everything!"
	make -C dll
	make -C commandline
	make -C firmware

clean:
	make clean -C dll
	make clean -C commandline
	make clean -C firmware
