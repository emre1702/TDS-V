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

cd /cygdrive/b/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Server/Core/bin/Debug/net5.0

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS ${NOCOLOR}runtimes ..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 --exclude="TDS.Client.*" --exclude="TDS.Server.Core.*" --include="*/" --include='TDS.*.dll' --include='TDS.*.pdb' --include='BonusBotConnector*' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@185.101.94.212:/home/rage/RAGE/dotnet/runtime/

echo -e "${SEPERATOR}"
echo -e "Add ${LIGHTBLUE}missing ${NOCOLOR}runtimes ..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 --ignore-existing --exclude='TDS.*' --include='*.dll' --include='*.so' --exclude='*' -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@185.101.94.212:/home/rage/RAGE/dotnet/runtime/

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}TDS.Server ${NOCOLOR}..."
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" ./TDS.Server.Core.dll ./TDS.Server.Core.pdb rage@185.101.94.212:/home/rage/RAGE/dotnet/resources/tds/

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}serverside JS ${NOCOLOR}..."
cd /cygdrive/b/Users/EmreKara/Desktop/Tools/GitHub/TDS-V/Server/Core/JavaScript
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@185.101.94.212:/home/rage/RAGE/packages/tds/

echo -e "${SEPERATOR}"
echo -e "Update ${LIGHTBLUE}client packages ${NOCOLOR}..."
cd /cygdrive/c/RAGEMP/server-files/client_packages
rsync -hmrtvzP --chmod=Du=rwx,Dgo=rw,Fu=rw,Fog=r --delete --timeout=60 -e "B:\cygwin64\bin\ssh.exe -p 55555 -i C:/Users/emre1/.ssh/rage_rsa" . rage@185.101.94.212:/home/rage/RAGE/client_packages/

cmd /k
