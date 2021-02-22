ARG DOTNET_VER=5.0.3
ARG DOTNET_VER_SDK=5.0.103-focal
ARG DOTNET_VER_RUNTIME=${DOTNET_VER}-focal
ARG NODE_VER=15.6.0
ARG SERVER_URL=https://cdn.rage.mp/updater/prerelease/server-files/linux_x64.tar.gz
ARG CERTIFICATE_PASSWORD="tdsv"

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VER_SDK} AS csharp-build-env
ARG CERTIFICATE_PASSWORD
WORKDIR /tds-source

COPY . .

RUN [ ! -d /ragemp-server-data/BonusBotConnector.Server.pfx ] \
    && dotnet dev-certs https --clean \
    && dotnet dev-certs https -ep /ragemp-server-data/BonusBotConnector.Server.pfx -p ${CERTIFICATE_PASSWORD} \
    ; exit 0

RUN dotnet publish ./Server/Core/Core.csproj -p:PublishProfile=LinuxDebug


FROM node:${NODE_VER} AS build-env
COPY --from=csharp-build-env /tds-source /tds-source  

WORKDIR /tds-source/Client/Window/angular

RUN npm install --no-audit && npm run build


FROM mcr.microsoft.com/dotnet/runtime:${DOTNET_VER_RUNTIME} AS release
ARG SERVER_URL
ARG DOTNET_VER
ENV DEBIAN_FRONTEND="noninteractive"

# Install dependencies
RUN apt-get update && apt-get install -y \
	libc6-dev \
	libunwind8 \
    libssl1.1 \
    rsync \
    curl \
    tzdata \
    locales \
	&& rm -rf /var/lib/apt/lists/*

# Add rage user
RUN useradd -m -d /home/rage rage

WORKDIR /home/rage

# Download required packages
RUN curl -O $SERVER_URL \
	&& tar --strip-components=1 -xvf linux_x64.tar.gz \
	&& rm -f linux_x64.tar.gz
	
# Create folders 
RUN mkdir -p ./client_packages/cs_packages/TDS.Client \
    && mkdir -p ./client_packages/Window \
    && mkdir -p ./dotnet/resources/tds \
    && mkdir -p ./client_packages/Window/angular/main \
    && mkdir -p ./packages/tds
       
COPY --from=build-env /tds-source /tds-source
COPY --from=build-env /tds-source/meta.xml ./dotnet/resources/tds/

# Init Maps folder in volume if not exists
# RUN [ ! -d /ragemp-server-data ] && mkdir -p /ragemp-server-data/; exit 0
RUN [ ! -d /ragemp-server-data/Maps/maps ] && mkdir -p /ragemp-server-data/Maps/ && cp -a /tds-source/Maps/. /ragemp-server-data/Maps/; exit 0
RUN [ ! -d /ragemp-server-data/Maps/needcheckmaps ] && mkdir -p /ragemp-server-data/Maps/needcheckmaps/ && cp -a /tds-source/Maps/needcheckmaps/. /ragemp-server-data/Maps/needcheckmaps/; exit 0

RUN rm -rf ./dotnet/runtime/* \
    # Set TDS runtimes 
    && rsync -hmrtvzP --exclude='TDS.Server.Core.*' --include="*" /tds-source/Build/ ./dotnet/runtime \
    # Set TDS.Server.Core
    && rsync -hmrtvzP /tds-source/Build/TDS.Server.Core.dll /tds-source/Build/TDS.Server.Core.pdb ./dotnet/resources/tds \
    # Set serverside JS 
    && rsync -hmrtvzP --delete /tds-source/Server/Core/JavaScript/ ./packages/tds \
    # Set clientside C# files 
    && rsync -hmrtvzP --delete --exclude="bin" --exclude="obj" --exclude="node_modules" --include="*/" --include='*.cs' --exclude='*' /tds-source/Client/ /tds-source/Shared/ ./client_packages/cs_packages/TDS.Client/ \
    # Set clientside JS 
    && rsync -hmrtvzP --delete --include='*.js' --exclude='*' /tds-source/Client/Core/JavaScript/ ./client_packages/ \
    # Set clientside plain HTML CEF 
    && rsync -hmrtvzP --delete --exclude=".vscode" --exclude="angular" --exclude="node_modules" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' /tds-source/Client/Window/ ./client_packages/Window \
    # Set clientside Angular CEF 
    && rsync -hmrtvzP --delete /tds-source/Client/Window/angular/dist/main/ ./client_packages/Window/angular/main
	
RUN cp /usr/share/dotnet/shared/Microsoft.NETCore.App/${DOTNET_VER}/* ./dotnet/runtime/
    
# Expose Ports and start the Server
ADD ./entrypoint.sh ./entrypoint.sh
ADD ./settings.xml ./dotnet/settings.xml
ADD ./conf.json ./conf.json
EXPOSE 22005/udp 22006 5001

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
RUN chmod +x /wait

RUN chown -R rage:rage . \
    && chown -R rage:rage /wait \
    && chown -R rage:rage /ragemp-server-data/ \
    && chmod +x ragemp-server \
    && chmod +x entrypoint.sh \
    && chmod +x /wait
	
RUN rm -rf /tds-source

ENV DOTNET_VER ${DOTNET_VER}

USER rage
CMD /wait && ./entrypoint.sh

# ENTRYPOINT ["/bin/sh", ""]