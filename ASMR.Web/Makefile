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
	@make $(BUILD_TARGET);

clean:
	@dotnet clean;

client:
	@cd ClientApp; yarn; yarn build

cross-platform:
	$(eval @_RELEASE_NAME := "asmr-cross-platform")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH); \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \
	

linux-x86:
	$(eval @_RELEASE_NAME := "asmr-cross-platform")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime linux-x86 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \

linux-x64:
	$(eval @_RELEASE_NAME := "asmr-linux-x64")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime linux-x64 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \
		
linux-x64-noopt:
	$(eval @_RELEASE_NAME := "asmr-linux-x64-noopt")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		--runtime linux-x64 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \

macos-x64:
	$(eval @_RELEASE_NAME := "asmr-macos-x64")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime osx-x64 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \

win-x86:
	$(eval @_RELEASE_NAME := "asmr-win-x86")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime win-x86 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \

win-x64:
	$(eval @_RELEASE_NAME := "asmr-win-x64")
	$(eval @_RELEASE_PATH := "Releases/$(@_RELEASE_NAME)")
	@npx rimraf $(@_RELEASE_PATH); \
	dotnet publish --configuration $(BUILD_CONFIGURATION) \
		--framework $(BUILD_FRAMEWORK) \
		--output $(@_RELEASE_PATH) \
		-p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true -p:PublishSingleFile=false -p:PublishTrimmed=false \
		--runtime win-x64 \
		--self-contained false; \
  	cd Releases; \
    zip -vr -X $(@_RELEASE_NAME).zip . -i $(@_RELEASE_NAME)\*; \
    cd ..; \

release:
	@make linux-x64
	@make macos-x64
	@make win-x64

migrations:
	@dotnet ef database drop; \
	dotnet ef migrations remove; \
	dotnet ef migrations add CreateInitialSchema --output-dir Data/Migrations; \
	dotnet ef database update;

run:
	dotnet run --launch-profile Production

watch:
	dotnet watch run
