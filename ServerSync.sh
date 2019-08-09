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

cd C:/RAGEMP/server-files/bridge/resources/tds/netcoreapp3.0

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS ${NOCOLOR}runtimes ..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 --exclude="TDS_Client.*" --exclude="TDS_Server.*" --include="*/" --include='TDS_*.dll' --include='TDS_*.pdb' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/bridge/runtime/

echo -e "${SEPERATOR}\n"
echo -e "Add ${LIGHTBLUE}missing ${NOCOLOR}runtimes ..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 --ignore-existing --exclude='TDS_*' --include='*.dll' --include='*.so' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/bridge/runtime/

echo -e "${SEPERATOR}\n"
echo -e "Update ${LIGHTBLUE}TDS_Server ${NOCOLOR}..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" ./TDS_Server.dll ./TDS_Server.pdb rage@51.38.99.64:/home/rage/RAGE/bridge/resources/tds/netcoreapp3.0/

echo -e "${SEPERATOR}\n"
echo -e "Update ${LIGHTBLUE}serverside JS ${NOCOLOR}..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Server/JavaScript
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/packages/tds/

echo -e "${SEPERATOR}\n"
echo -e "Update ${LIGHTBLUE}clientside JS ${NOCOLOR}..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/JavaScript
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 --include='*.js' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/client_packages/

echo -e "${SEPERATOR}\n"
echo -e "Update ${LIGHTBLUE}clientside C# ${NOCOLOR}..."

cd C:/RAGEMP/server-files/client_packages/cs_packages/TDS_Client
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --delete --timeout=60 --exclude="bin" --exclude="obj" --include="*/" --include='*.cs' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/client_packages/cs_packages/TDS_Client/

echo -e "${SEPERATOR}\n"
echo -e "Update clientside ${LIGHTBLUE}plain HTML ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/Window
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --delete --timeout=60 --exclude=".vscode" --exclude="mainNew" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/client_packages/Window/

echo -e "${SEPERATOR}\n"
echo -e "Update clientside ${LIGHTBLUE}Angular ${NOCOLOR}CEF ..."
cd B:/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/TDS_Client/Window/mainNew/dist/mainNew
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --delete --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@51.38.99.64:/home/rage/RAGE/client_packages/Window/mainNew/

cmd /k