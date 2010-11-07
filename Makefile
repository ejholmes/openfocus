all:
	echo "Building everything!"
	make -C dll
	#make -C commandline
	make -C firmware
	msbuild ASCOM\Hypnofocus.sln /t:Rebuild /p:Configuration=Release

clean:
	make clean -C dll
	make clean -C commandline
	make clean -C firmware
	msbuild ASCOM\Hypnofocus.sln /t:Clean /p:Configuration=Release
