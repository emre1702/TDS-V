ARG DOTNET_VER_RUNTIME=5.0.2
ARG DOTNET_VER_SDK=5.0.102-focal
ARG DOTNET_VER_ASPNET=$DOTNET_VER_RUNTIME-focal
ARG NODE_VER=15.6.0
ARG SERVER_URL=https://cdn.rage.mp/updater/prerelease/server-files/linux_x64.tar.gz

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VER_SDK} AS csharp-build-env
WORKDIR /app

COPY . .

RUN dotnet publish ./Server/Core/Core.csproj -p:PublishProfile=LinuxDebug


FROM node:${NODE_VER} AS build-env
COPY --from=csharp-build-env /app /app  

WORKDIR /app/Client/Window/angular

RUN npm install && npm run build



FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VER_ASPNET} AS release
ARG SERVER_URL
ARG DOTNET_VER_RUNTIME

# Install dependencies
RUN apt-get update && apt-get install -y \
	libc6-dev \
	libunwind8 \
    rsync \
    curl \
	&& rm -rf /var/lib/apt/lists/*

# Add rage user
RUN useradd -m -d /home/rage rage

WORKDIR /home/rage

# Download required packages
# RUN curl -O $SERVER_URL \
COPY ./Docker/linux_x64.tar.gz .
#	&& tar --strip-components=1 -xvf linux_x64.tar.gz \
RUN tar --strip-components=1 -xvf linux_x64.tar.gz \
	&& rm -f linux_x64.tar.gz
	
# Create folders 
RUN mkdir -p ./client_packages/cs_packages/TDS.Client \
    && mkdir -p ./client_packages/Window \
    && mkdir -p ./dotnet/resources/tds \
    && mkdir -p ./client_packages/Window/angular/main \
    && mkdir -p ./packages/tds
       
COPY --from=build-env /app /app
COPY --from=build-env /app/meta.xml ./dotnet/resources/tds/
# COPY --from=build-env /app/Maps ./dotnet/resources/tds/ 
COPY --from=build-env /app/TDS.Server.config ./dotnet/runtime/ 

# Init Maps folder in volume if not exists
# RUN [ ! -d /ragemp-server-data ] && mkdir -p /ragemp-server-data/; exit 0
RUN [ ! -d /ragemp-server-data/Maps/maps ] && mkdir -p /ragemp-server-data/Maps/ && cp -R /app/Maps/* /ragemp-server-data/Maps; exit 0
RUN [ ! -d /ragemp-server-data/TDS.Server.config ] && cp /app/TDS.Server.config /ragemp-server-data; exit 0

RUN rm -rf ./dotnet/runtime/* \
    # Set TDS runtimes 
    && rsync -hmrtvzP --exclude='TDS.Server.Core.*' --include="*.dll" --include='*.pdb' --exclude='*' /app/Build/ ./dotnet/runtime \
    # Set TDS.Server.Core
    && rsync -hmrtvzP /app/Build/TDS.Server.Core.dll /app/Build/TDS.Server.Core.pdb ./dotnet/resources/tds \
    # Set serverside JS 
    && rsync -hmrtvzP --delete /app/Server/Core/JavaScript/ ./packages/tds \
    # Set clientside C# files 
    && rsync -hmrtvzP --delete --exclude="bin" --exclude="obj" --exclude="node_modules" --include="*/" --include='*.cs' --exclude='*' /app/Client/ /app/Shared/ ./client_packages/cs_packages/TDS.Client/ \
    # Set clientside JS 
    && rsync -hmrtvzP --delete --include='*.js' --exclude='*' /app/Client/Core/JavaScript/ ./client_packages/ \
    # Set clientside plain HTML CEF 
    && rsync -hmrtvzP --delete --exclude=".vscode" --exclude="angular" --exclude="node_modules" --exclude="*.json" --exclude="*cefminify*" --include="*/" --include='*index.*' --include='*.min.*' --include='*.ttf' --include="*.png" --include='*.jpg' --include='*.mp3' --include='*.wav' --include='*.ogg' --exclude='*' /app/Client/Window/ ./client_packages/Window \
    # Set clientside Angular CEF 
    && rsync -hmrtvzP --delete /app/Client/Window/angular/dist/main/ ./client_packages/Window/angular/main \
    # Add symlink to maps folder in volume
    && ln -s /ragemp-server-data/Maps/* ./dotnet/resources/tds/ \
    # Add TDS.Server.Config from volume
    && cp /ragemp-server-data/TDS.Server.config ./dotnet/runtime/
	
RUN cp /usr/share/dotnet/shared/Microsoft.NETCore.App/$DOTNET_VER_RUNTIME/* ./dotnet/runtime/
    
# Expose Ports and start the Server
ADD ./Docker/entrypoint.sh ./entrypoint.sh
ADD ./Docker/settings.xml ./dotnet/settings.xml
ADD ./Docker/conf.json ./conf.json
EXPOSE 22005/udp 22006

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
RUN chmod +x /wait

RUN chown -R rage:rage . \
    && chown -R rage:rage /wait \
    && chown -R rage:rage /ragemp-server-data/ \
    && chmod +x ragemp-server \
    && chmod +x entrypoint.sh \
    && chmod +x /wait
	
RUN rm -rf /app

ENV DOTNET_VER ${DOTNET_VER_RUNTIME}

USER rage
CMD /wait && ./entrypoint.sh

# ENTRYPOINT ["/bin/sh", ""]