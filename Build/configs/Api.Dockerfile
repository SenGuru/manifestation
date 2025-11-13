##########################################################################
### Build Backend Code
##########################################################################

FROM mcr.microsoft.com/dotnet/sdk:5.0.103-alpine3.13 AS dotnet-build

WORKDIR /src
COPY Source/Solutions /src/Solutions
COPY Source/Backend/. /src/Backend/.
COPY Source/Tools/. /src/Tools/.


WORKDIR /src/Solutions
RUN dotnet restore
RUN dotnet build -c Release   --no-restore

WORKDIR /src/Backend/Api/All
RUN dotnet publish -c Release   --no-restore   -o .\Release\Publish Northpass.App.Api.csproj


##########################################################################
### Build Angular Code
##########################################################################
FROM node:13-alpine AS node-build
RUN /sbin/apk update && /sbin/apk add --no-cache python2  
WORKDIR /web-src
#COPY Source/Web/All/package.json /web-src/web/All/package.json
COPY Source/Web/ /web-src/web/
WORKDIR /web-src/web
# RUN npm install node-sass@latest
RUN npm install
ARG MAX_OLD_SPACE_SIZE=4096
ENV NODE_OPTIONS=--max-old-space-size=${MAX_OLD_SPACE_SIZE}
#RUN NODE_OPTIONS=--max_old_space_size=32768
RUN npm install -g @angular/cli
#COPY Source/Web/All/ /web-src/web/All/
RUN npm run build

##########################################################################
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS app

#--Copy each and every app to corresponding folder
#--Add supervisord script and copy
#--Add nginx config and copy
#--set supervisord as startup

#---Web apps
RUN /sbin/apk update && /sbin/apk add --no-cache supervisor openssh nginx openrc curl
RUN apk update
WORKDIR /stenograph-files

COPY --from=node-build /web-src/web/dist/AuditDuo /usr/share/nginx/html
COPY Build/docker/ngnix/default.conf /etc/nginx/conf.d/
COPY Build/docker/supervisord/supervisor_nginx.conf /etc/supervisor/conf.d/supervisord.conf

COPY --from=dotnet-build /src/Backend/Api/All/bin/Release/net7.0 /app-files/app/web/
COPY ./Configuration/AuditDuoConfigDocker.json /app-files/Configuration/AuditDuoConfig.json

#RUN chown -R root:root /var/lib/nginx;
RUN mkdir -p /run/nginx;

RUN  nginx -t

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

#RUN pkill nginx
#RUN  service supervisor stop
#RUN  systemctl enable supervisor
#RUN  service supervisor start


#--Final call

EXPOSE 9009 80

WORKDIR /app-files/app/web/

CMD ["/usr/bin/supervisord", "-c","/etc/supervisor/conf.d/supervisord.conf"]