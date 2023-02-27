FROM node:19.7
RUN mkdir /data
WORKDIR /data
COPY start.sh /start
ENTRYPOINT [ "/start", "npm", "run", "dev" ]