FROM node:19.6.0-bullseye-slim
RUN apt-get update
RUN apt-get install -y udev git
ENV NO_UPDATE_NOTIFIER true
ENV npm_config_update_notifier false
RUN mkdir /app
WORKDIR /app
RUN npm install -g cncjs@1.10.1
COPY start.sh /app/start
EXPOSE 8000
ENTRYPOINT [ "./start" ]
