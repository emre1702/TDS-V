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
mkdir -p /cygdrive/c/altV/server/resource/tds/client/altv 
mkdir -p /cygdrive/c/altV/server/resource/tds/client/plainHtml
mkdir -p /cygdrive/c/altV/server/resource/tds/client/angular/main
mkdir -p /cygdrive/c/altV/server/resource/tds/client/angular/map-creator-object-choice
mkdir -p /cygdrive/c/altV/server/resource/tds/client/angular/map-creator-vehicle-choice
mkdir -p /cygdrive/c/altV/server/resource/tds/server 

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}clientside JS ${NOCOLOR}files ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Client/TypeScript/dist && rsync -hmrtvzP --delete  ./main.js /cygdrive/c/altV/server/resources/tds/client/altv

echo -e "${SEPERATOR}"
echo -e "Update clientside ${LIGHTBLUE}plain HTML ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Client/Window && rsync -hmrtvzP --delete --exclude=".vscode" --exclude="angular" --exclude="node_modules" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' . /cygdrive/c/altV/server/resources/tds/client/plainHtml

echo -e "${SEPERATOR}"
echo -e "Update clientside ${LIGHTBLUE}Angular ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Client/Window/angular/dist/main && rsync -hmrtvzP --delete . /cygdrive/c/altV/server/resources/tds/client/angular/main
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Client/Window/angular/dist/map-creator-object-choice && rsync -hmrtvzP --delete . /cygdrive/c/altV/server/resources/tds/client/angular/map-creator-object-choice
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Client/Window/angular/dist/map-creator-vehicle-choice && rsync -hmrtvzP --delete . /cygdrive/c/altV/server/resources/tds/client/angular/map-creator-vehicle-choice

cmd /k
