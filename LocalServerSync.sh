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

cd /cygdrive/c/RAGEMP/server-files/bridge/resources/tds/netcoreapp3.0

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS ${NOCOLOR}runtimes ...  /ragemp/server-files/bridge/runtime"
rsync -hmrtvzP --exclude='TDS_Server.*' --include="*.dll" --include='*.pdb' --exclude='*' . /cygdrive/c/RAGEMP/server-files/bridge/runtime

echo -e "${SEPERATOR}/n"
echo -e "Update ${LIGHTBLUE}TDS_Server ${NOCOLOR}..."
rsync -hmrtvzP ./TDS_Server.dll ./TDS_Server.pdb /cygdrive/c/RAGEMP/server-files/bridge/resources/tds/netcoreapp3.0

echo -e "${SEPERATOR}/n"
echo -e "Update ${LIGHTBLUE}serverside JS ${NOCOLOR}..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Server/JavaScript
rsync -hmrtvzP --delete . /cygdrive/c/RAGEMP/server-files/packages/tds

echo -e "${SEPERATOR}/n"
echo -e "Update ${LIGHTBLUE}clientside C# ${NOCOLOR}files ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V
rsync -hmrtvzP --delete --exclude="bin" --exclude="obj" --exclude="node_modules" --include="*/" --include='*.cs' --exclude='*' TDS_Client/. TDS_Common/. /cygdrive/c/RAGEMP/server-files/client_packages/cs_packages/TDS_Client/

echo -e "${SEPERATOR}/n"
echo -e "Update ${LIGHTBLUE}clientside JS ${NOCOLOR}..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/JavaScript
rsync -hmrtvzP --delete --include='*.js' --exclude='*' . /cygdrive/c/RAGEMP/server-files/client_packages

echo -e "${SEPERATOR}/n"
echo -e "Update clientside ${LIGHTBLUE}plain HTML ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/Window
rsync -hmrtvzP --delete --exclude=".vscode" --exclude="angular" --exclude="node_modules" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' . /cygdrive/c/RAGEMP/server-files/client_packages/Window

echo -e "${SEPERATOR}/n"
echo -e "Update clientside ${LIGHTBLUE}Angular ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/Window/angular/dist/main
rsync -hmrtvzP --delete . /cygdrive/c/RAGEMP/server-files/client_packages/Window/angular/main
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/Window/angular/dist/map-creator-object-choice
rsync -hmrtvzP --delete . /cygdrive/c/RAGEMP/server-files/client_packages/Window/angular/map-creator-object-choice

cmd /k
