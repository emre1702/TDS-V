echo -e "${SEPERATOR}"
NOCOLOR='\033[0m'
RED='\033[0;31m'
GREEN='\033[0;32m'
ORANGE='\033[0;33m'
BLUE='\033[0;34m'
PURPLE='\033[0;35m'
CYAN='\033[0;36m'
LIGHTGRAY='\033[0;37m'
DARKGRAY='\033[1;30m'
LIGHTRED='\033[1;31m'
LIGHTGREEN='\033[1;32m'
YELLOW='\033[1;33m'
LIGHTBLUE='\033[1;34m'
LIGHTPURPLE='\033[1;35m'
LIGHTCYAN='\033[1;36m'
WHITE='\033[1;37m'
SEPERATOR='==============================='

echo -e "${SEPERATOR}"
echo -e "Create ${LIGHTBLUE}folder ${NOCOLOR}..."
mkdir -p /cygdrive/c/RAGEMP/server-files/client_packages/cs_packages/TDS.Client
mkdir -p /cygdrive/c/RAGEMP/server-files/client_packages/Window
mkdir -p /cygdrive/c/RAGEMP/server-files/dotnet/resources/tds/net5.0
mkdir -p /cygdrive/c/RAGEMP/server-files/client_packages/Window/angular/main
mkdir -p /cygdrive/c/RAGEMP/server-files/packages/tds

cd /cygdrive/c/Programming/TDS-V/Server/Core/bin/Debug/net5.0

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS ${NOCOLOR}runtimes ..."
rsync -hmrtvzP --exclude='TDS.Server.Core.*' --include="*.dll" --include='*.pdb' --exclude='*' . /cygdrive/c/RAGEMP/server-files/dotnet/runtime

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS.Server.Core ${NOCOLOR}..."
rsync -hmrtvzP ./TDS.Server.Core.dll ./TDS.Server.Core.pdb /cygdrive/c/RAGEMP/server-files/dotnet/resources/tds/net5.0

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}serverside JS ${NOCOLOR}..."
cd C:/Programming/TDS-V/Server/Core/JavaScript && rsync -hmrtvzP --delete . /cygdrive/c/RAGEMP/server-files/packages/tds

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}clientside C# ${NOCOLOR}files ..."
cd C:/Programming/TDS-V && rsync -hmrtvzP --delete --exclude="bin" --exclude="obj" --exclude="node_modules" --include="*/" --include='*.cs' --exclude='*' Client/. Shared/. /cygdrive/c/RAGEMP/server-files/client_packages/cs_packages/TDS.Client/

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}clientside JS ${NOCOLOR}..."
cd C:/Programming/TDS-V/Client/Core/JavaScript && rsync -hmrtvzP --delete --include='*.js' --exclude='*' . /cygdrive/c/RAGEMP/server-files/client_packages

echo -e "${SEPERATOR}"
echo -e "Update clientside ${LIGHTBLUE}plain HTML ${NOCOLOR}CEF ..."
cd C:/Programming/TDS-V/Client/Window && rsync -hmrtvzP --delete --exclude=".vscode" --exclude="angular" --exclude="node_modules" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' . /cygdrive/c/RAGEMP/server-files/client_packages/Window

echo -e "${SEPERATOR}"
echo -e "Update clientside ${LIGHTBLUE}Angular ${NOCOLOR}CEF ..."
cd C:/Programming/TDS-V/Client/Window/angular/dist/main && rsync -hmrtvzP --delete . /cygdrive/c/RAGEMP/server-files/client_packages/Window/angular/main

cmd /k
