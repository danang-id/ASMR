BUILD_CONFIGURATION := Release
BUILD_FRAMEWORK := net6.0

ifeq ($(OS),Windows_NT)
    BUILD_TARGET := win
    ifeq ($(PROCESSOR_ARCHITEW6432),AMD64)
        BUILD_TARGET := $(BUILD_TARGET)-x64
    else
        ifeq ($(PROCESSOR_ARCHITECTURE),AMD64)
        	BUILD_TARGET := $(BUILD_TARGET)-x64
        endif
        ifeq ($(PROCESSOR_ARCHITECTURE),x86)
        	BUILD_TARGET := $(BUILD_TARGET)-x86
        endif
    endif
else
    UNAME_S := $(shell uname -s)
    UNAME_P := $(shell uname -p)
    
    ifeq ($(UNAME_S),Linux)
    	BUILD_TARGET := linux
		ifeq ($(UNAME_P),x86_64)
			BUILD_TARGET := $(BUILD_TARGET)-x64
		endif
		ifneq ($(filter %86,$(UNAME_P)),)
			BUILD_TARGET := $(BUILD_TARGET)-x86
		endif
		ifneq ($(filter arm%,$(UNAME_P)),)
			BUILD_TARGET := $(BUILD_TARGET)-arm
		endif
    endif
    ifeq ($(UNAME_S),Darwin)
    	BUILD_TARGET := osx-x64
    endif
endif

build:
	make $(BUILD_TARGET);

clean:
	dotnet clean;

client:
	cd ClientApp; yarn; yarn build

cross-platform:
	npx rimraf Releases/ASMR-CrossPlatform; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-CrossPlatform

linux-x86:
	npx rimraf Releases/ASMR-Linux-x86; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-Linux-x86 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime linux-x86 \
		--self-contained true

linux-x64:
	npx rimraf Releases/ASMR-Linux-x64; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-Linux-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime linux-x64 \
		--self-contained true
		
linux-x64-noopt:
	npx rimraf Releases/ASMR-Linux-x64; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-Linux-x64 \
		--runtime linux-x64 \
		--self-contained true

osx-x64:
	npx rimraf Releases/ASMR-macOS-x64; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-macOS-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime osx-x64 \
		--self-contained true

win-x86:
	npx rimraf Releases/ASMR-Windows-x86; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-Windows-x86 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime win-x86 \
		--self-contained true

win-x64:
	npx rimraf Releases/ASMR-Windows-x64; \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output Releases/ASMR-Windows-x64 \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=true \
		--runtime win-x64 \
		--self-contained true

migrations:
	dotnet ef database drop; \
	dotnet ef migrations remove; \
	dotnet ef migrations add CreateInitialSchema --output-dir Data/Migrations; \
	dotnet ef database update;

run:
	dotnet run --launch-profile Production

watch:
	dotnet watch run
