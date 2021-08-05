build:
	make client
	cd ASMR.Web; dotnet publish --configuration Release \
		--framework net5.0 \
		--output bin/Release/dist/net5.0

clean:
	dotnet clean

client:
	cd ASMR.Web/ClientApp; yarn; yarn build

linux:
	cd ASMR.Web; dotnet publish --configuration Release \
		--framework net5.0 \
		--output Release/Dist/linux-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true -p:PublishTrimmed=true \
		--runtime linux-x64

osx:
	cd ASMR.Web; dotnet publish --configuration Release \
		--framework net5.0 \
		--output Release/Dist/osx-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true -p:PublishTrimmed=true \
		--runtime osx-x64

win:
	cd ASMR.Web; dotnet publish --configuration Release \
		--framework net5.0 \
		--output Release/Dist/win-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=true -p:PublishTrimmed=true \
		--runtime win-x64

refresh-migrations:
	cd ASMR.Web; \
		dotnet ef database drop; \
		dotnet ef migrations remove; \
		dotnet ef migrations add CreateInitialSchema --output-dir Data/Migrations; \
		dotnet ef database update;

run:
	cd ASMR.Web; dotnet run

watch:
	cd ASMR.Web; dotnet watch run
